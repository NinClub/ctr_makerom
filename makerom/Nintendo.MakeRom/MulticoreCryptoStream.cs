using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
namespace Nintendo.MakeRom
{
	public class MulticoreCryptoStream : Stream
	{
		private const int s_blockSize = 4194304;
		private static int s_workers;
		private Thread m_tailThread;
		private int m_activeThreadNum;
		private byte[][] m_workingMemory;
		private int m_currentMemoryIndex;
		private MemoryStream m_currentMemoryStream;
		private int m_currentSize;
		private MulticoreCryptoWorker[] m_workers;
		private Stream m_writeStream;
		private ulong m_position;
		private byte[] m_key;
		private ulong m_partitionID;
		private ulong m_baseInitCount;
		private List<MulticoreCryptoWorker> m_completeWorkers = new List<MulticoreCryptoWorker>();
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}
		public override long Position
		{
			get
			{
				return (long)this.m_position;
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		static MulticoreCryptoStream()
		{
			MulticoreCryptoStream.s_workers = Environment.ProcessorCount;
		}
		public static void SetUsingThreadNumber(int threads)
		{
			MulticoreCryptoStream.s_workers = threads;
		}
		public MulticoreCryptoStream(Stream writeTarget, ICryptoTransform crypto, CryptoStreamMode mode)
		{
			AesCtr aesCtr = (AesCtr)crypto;
			this.m_writeStream = writeTarget;
			this.m_workingMemory = new byte[MulticoreCryptoStream.s_workers][];
			for (int i = 0; i < MulticoreCryptoStream.s_workers; i++)
			{
				this.m_workingMemory[i] = new byte[4194304];
			}
			this.m_currentMemoryIndex = 0;
			this.m_currentMemoryStream = new MemoryStream(this.m_workingMemory[this.m_currentMemoryIndex]);
			this.m_currentSize = 0;
			this.m_workers = new MulticoreCryptoWorker[MulticoreCryptoStream.s_workers];
			for (int j = 0; j < MulticoreCryptoStream.s_workers; j++)
			{
				this.m_workers[j] = new MulticoreCryptoWorker(4194304);
				this.m_workers[j].m_handler += new FinishAesEventHandler(this.ReserveWriteStreamAndDecrementThreadNum);
			}
			this.m_position = 0uL;
			this.m_key = aesCtr.key;
			byte[] value = aesCtr.GetCounter().Reverse<byte>().ToArray<byte>();
			this.m_baseInitCount = BitConverter.ToUInt64(value, 0);
			this.m_partitionID = BitConverter.ToUInt64(value, 8);
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}
		internal void IncrementThreadNum()
		{
			this.m_activeThreadNum++;
		}
		internal void WriteStreamAndDecrementThreadNum()
		{
			List<MulticoreCryptoWorker> completeWorkers;
			Monitor.Enter(completeWorkers = this.m_completeWorkers);
			try
			{
				foreach (MulticoreCryptoWorker current in this.m_completeWorkers)
				{
					current.Write(this.m_writeStream);
					this.m_activeThreadNum--;
				}
				this.m_completeWorkers.Clear();
			}
			finally
			{
				Monitor.Exit(completeWorkers);
			}
		}
		internal void ReserveWriteStreamAndDecrementThreadNum(object sender, FinishAesEventArgs args)
		{
			List<MulticoreCryptoWorker> completeWorkers;
			Monitor.Enter(completeWorkers = this.m_completeWorkers);
			try
			{
				this.m_completeWorkers.Add((MulticoreCryptoWorker)sender);
			}
			finally
			{
				Monitor.Exit(completeWorkers);
			}
		}
		protected void RunThread()
		{
			if (this.m_currentSize == 0)
			{
				return;
			}
			MulticoreCryptoWorker multicoreCryptoWorker = this.m_workers[this.m_currentMemoryIndex];
			this.m_currentMemoryStream.Seek(0L, SeekOrigin.Begin);
			multicoreCryptoWorker.SetupMemory(this.m_workingMemory[this.m_currentMemoryIndex], this.m_currentSize);
			multicoreCryptoWorker.SetupAes(this.m_key, this.m_partitionID, this.m_baseInitCount + (this.m_position >> 4));
			multicoreCryptoWorker.SetupDependency(this.m_tailThread);
			this.m_position += (ulong)((long)this.m_currentSize);
			this.m_currentSize = 0;
			Thread thread = new Thread(new ThreadStart(this.m_workers[this.m_currentMemoryIndex].DoWork));
			thread.Start();
			this.m_tailThread = thread;
			this.IncrementThreadNum();
			this.m_currentMemoryIndex++;
			if (this.m_currentMemoryIndex >= MulticoreCryptoStream.s_workers)
			{
				this.m_currentMemoryIndex = 0;
			}
			this.m_currentMemoryStream.Dispose();
			this.m_currentMemoryStream = new MemoryStream(this.m_workingMemory[this.m_currentMemoryIndex]);
		}
		protected void WaitThread()
		{
			if (this.m_tailThread != null)
			{
				this.m_tailThread.Join();
				this.m_tailThread = null;
			}
		}
		public void WriteAllFromStream(Stream st)
		{
			int num = 0;
			do
			{
				if (this.m_activeThreadNum == MulticoreCryptoStream.s_workers)
				{
					this.WriteStreamAndDecrementThreadNum();
					if (this.m_activeThreadNum >= MulticoreCryptoStream.s_workers)
					{
						Thread.Sleep(100);
						continue;
					}
				}
				num = st.Read(this.m_workingMemory[this.m_currentMemoryIndex], 0, 4194304);
				this.m_currentSize += num;
				this.RunThread();
			}
			while (num == 4194304);
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			int i = 0;
			while (i < count)
			{
				while (this.m_activeThreadNum == MulticoreCryptoStream.s_workers)
				{
					this.WriteStreamAndDecrementThreadNum();
					if (this.m_activeThreadNum < MulticoreCryptoStream.s_workers)
					{
						break;
					}
					Thread.Sleep(100);
				}
				int num = count - i;
				if (num < 4194304 - this.m_currentSize)
				{
					this.m_currentMemoryStream.Write(buffer, offset + i, num);
					i += num;
					this.m_currentSize += num;
				}
				else
				{
					num = 4194304 - this.m_currentSize;
					this.m_currentMemoryStream.Write(buffer, offset + i, num);
					i += num;
					this.m_currentSize += num;
					this.RunThread();
				}
			}
		}
		public override void Flush()
		{
			this.WriteStreamAndDecrementThreadNum();
			this.RunThread();
			this.WaitThread();
			this.WriteStreamAndDecrementThreadNum();
		}
		protected override void Dispose(bool disposing)
		{
			this.Flush();
			base.Dispose(disposing);
		}
	}
}
