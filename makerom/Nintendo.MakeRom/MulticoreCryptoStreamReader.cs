using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
namespace Nintendo.MakeRom
{
	public class MulticoreCryptoStreamReader : Stream
	{
		private const int s_blockSize = 4194304;
		private static int s_workers;
		private Thread m_tailThread;
		private int m_activeThreadNum;
		private byte[][] m_workingMemory;
		private int m_currentMemoryIndex;
		private int m_currentReadMemoryIndex;
		private MemoryStream m_currentMemoryStream;
		private int m_currentSize;
		private MulticoreCryptoWorker[] m_workers;
		private Stream m_readStream;
		private ulong m_position;
		private ulong m_aesPosition;
		private byte[] m_key;
		private ulong m_partitionID;
		private ulong m_baseInitCount;
		private List<MulticoreCryptoWorker> m_completeWorkers = new List<MulticoreCryptoWorker>();
		private object obj = new object();
		public override bool CanRead
		{
			get
			{
				return true;
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
				return false;
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
		static MulticoreCryptoStreamReader()
		{
			MulticoreCryptoStreamReader.s_workers = Environment.ProcessorCount + 1;
		}
		public static void SetUsingThreadNumber(int threads)
		{
			MulticoreCryptoStreamReader.s_workers = threads;
		}
		public MulticoreCryptoStreamReader(Stream readTarget, ICryptoTransform crypto, CryptoStreamMode mode)
		{
			AesCtr aesCtr = (AesCtr)crypto;
			this.m_readStream = readTarget;
			this.m_workingMemory = new byte[MulticoreCryptoStreamReader.s_workers][];
			for (int i = 0; i < MulticoreCryptoStreamReader.s_workers; i++)
			{
				this.m_workingMemory[i] = new byte[4194304];
			}
			this.m_currentMemoryIndex = 0;
			this.m_currentReadMemoryIndex = 0;
			this.m_currentMemoryStream = new MemoryStream(this.m_workingMemory[this.m_currentReadMemoryIndex]);
			this.m_currentSize = 0;
			this.m_workers = new MulticoreCryptoWorker[MulticoreCryptoStreamReader.s_workers];
			for (int j = 0; j < MulticoreCryptoStreamReader.s_workers; j++)
			{
				this.m_workers[j] = new MulticoreCryptoWorker(4194304);
				this.m_workers[j].m_handler += new FinishAesEventHandler(this.DecrementThreadNum);
			}
			this.m_position = 0uL;
			this.m_aesPosition = 0uL;
			this.m_key = aesCtr.key;
			byte[] value = aesCtr.GetCounter().Reverse<byte>().ToArray<byte>();
			this.m_baseInitCount = BitConverter.ToUInt64(value, 0);
			this.m_partitionID = BitConverter.ToUInt64(value, 8);
			for (int k = 0; k < MulticoreCryptoStreamReader.s_workers; k++)
			{
				if (this.RunThread() != 4194304)
				{
					return;
				}
			}
		}
		protected int RunThread()
		{
			int num = this.m_readStream.Read(this.m_workingMemory[this.m_currentMemoryIndex], 0, 4194304);
			if (num == 0)
			{
				return num;
			}
			MulticoreCryptoWorker multicoreCryptoWorker = this.m_workers[this.m_currentMemoryIndex];
			multicoreCryptoWorker.SetupMemory(this.m_workingMemory[this.m_currentMemoryIndex], num);
			multicoreCryptoWorker.SetupAes(this.m_key, this.m_partitionID, this.m_baseInitCount + (this.m_aesPosition >> 4));
			multicoreCryptoWorker.SetupDependency(this.m_tailThread);
			this.m_aesPosition += (ulong)((long)num);
			Thread thread = new Thread(new ThreadStart(multicoreCryptoWorker.DoWork));
			thread.Start();
			this.m_tailThread = thread;
			this.IncrementThreadNum();
			this.m_currentMemoryIndex++;
			if (this.m_currentMemoryIndex >= MulticoreCryptoStreamReader.s_workers)
			{
				this.m_currentMemoryIndex = 0;
			}
			return num;
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			int i = 0;
			while (i < count)
			{
				while (this.m_activeThreadNum == MulticoreCryptoStreamReader.s_workers || (this.m_activeThreadNum != 0 && this.m_currentReadMemoryIndex == (this.m_currentMemoryIndex - this.m_activeThreadNum + MulticoreCryptoStreamReader.s_workers) % MulticoreCryptoStreamReader.s_workers))
				{
					Thread.Sleep(100);
				}
				int num = count - i;
				if (num < 4194304 - this.m_currentSize && (long)num < (long)(this.m_aesPosition - this.m_position))
				{
					this.m_currentMemoryStream.Read(buffer, i + offset, num);
					i += num;
					this.m_currentSize += num;
					this.m_position += (ulong)((long)num);
				}
				else
				{
					if ((long)(4194304 - this.m_currentSize) < (long)(this.m_aesPosition - this.m_position))
					{
						num = 4194304 - this.m_currentSize;
						this.m_currentMemoryStream.Read(buffer, i + offset, num);
						i += num;
						this.m_position += (ulong)((long)num);
						this.m_currentReadMemoryIndex++;
						this.m_currentSize = 0;
						if (this.m_currentReadMemoryIndex >= MulticoreCryptoStreamReader.s_workers)
						{
							this.m_currentReadMemoryIndex = 0;
						}
						this.m_currentMemoryStream.Dispose();
						this.m_currentMemoryStream = new MemoryStream(this.m_workingMemory[this.m_currentReadMemoryIndex]);
						this.RunThread();
					}
					else
					{
						num = (int)(this.m_aesPosition - this.m_position);
						this.m_currentMemoryStream.Read(buffer, i + offset, num);
						i += num;
						this.m_position += (ulong)((long)num);
						this.m_currentSize += num;
						if (this.RunThread() != 0)
						{
							throw new NotImplementedException();
						}
						break;
					}
				}
			}
			return i;
		}
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}
		public override void Flush()
		{
		}
		internal void IncrementThreadNum()
		{
			object obj;
			Monitor.Enter(obj = this.obj);
			try
			{
				this.m_activeThreadNum++;
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		internal void DecrementThreadNum(object sender, FinishAesEventArgs args)
		{
			object obj;
			Monitor.Enter(obj = this.obj);
			try
			{
				this.m_activeThreadNum--;
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		protected void WaitThread()
		{
			if (this.m_tailThread != null)
			{
				this.m_tailThread.Join();
				this.m_tailThread = null;
			}
		}
	}
}
