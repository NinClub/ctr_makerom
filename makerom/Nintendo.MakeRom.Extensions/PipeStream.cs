using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
namespace Nintendo.MakeRom.Extensions
{
	public class PipeStream : Stream, IDisposable
	{
		private byte[][] m_Buffers;
		private MemoryStream m_reader;
		private MemoryStream m_writer;
		private static readonly int s_BufferSize = 2097152;
		private static readonly int s_NumberOfBuffers = 4;
		private byte[] m_currentRead;
		private byte[] m_currentWrite;
		private ThreadSafeBlockQueue<byte[]> m_readQueue = new ThreadSafeBlockQueue<byte[]>();
		private ThreadSafeBlockQueue<byte[]> m_writeQueue = new ThreadSafeBlockQueue<byte[]>();
		private bool m_isEndWriting;
		public override bool CanRead
		{
			get
			{
				return true;
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
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		[Conditional("DEBUG")]
		public static void RunTest()
		{
			using (PipeStream p2 = new PipeStream())
			{
				Thread thread = new Thread((ThreadStart)delegate
				{
					byte[] array = new byte[1];
					for (int j = 0; j < 10485760; j++)
					{
						array[0] = (byte)j;
						p2.Write(array, 0, 1);
					}
					p2.EndWriting();
				});
				Thread thread2 = new Thread((ThreadStart)delegate
				{
					byte[] buffer = new byte[1];
					for (int j = 0; j < 10485760; j++)
					{
						p2.Read(buffer, 0, 1);
					}
				});
				thread.Start();
				thread2.Start();
				thread2.Join();
				thread.Join();
			}
			using (PipeStream p = new PipeStream())
			{
				Thread thread3 = new Thread((ThreadStart)delegate
				{
					byte[] array = new byte[1];
					for (int j = 0; j < 11678806; j++)
					{
						array[0] = (byte)j;
						p.Write(array, 0, 1);
					}
					p.EndWriting();
				});
				Thread thread4 = new Thread((ThreadStart)delegate
				{
					byte[] buffer = new byte[1];
					for (int j = 0; j < 11678806; j++)
					{
						p.Read(buffer, 0, 1);
					}
				});
				thread3.Start();
				thread4.Start();
				thread4.Join();
				thread3.Join();
			}
			for (int i = 1; i < 5; i++)
			{
				using (PipeStream p2 = new PipeStream())
				{
					byte[] sample = new byte[PipeStream.s_BufferSize + PipeStream.s_BufferSize / i];
					new Random().NextBytes(sample);
					Thread thread5 = new Thread((ThreadStart)delegate
					{
						for (int j = 0; j < 10; j++)
						{
							p2.Write(sample, 0, sample.Length);
						}
						p2.EndWriting();
					});
					Thread thread6 = new Thread((ThreadStart)delegate
					{
						byte[] array = new byte[sample.Length];
						for (int j = 0; j < 10; j++)
						{
							p2.Read(array, 0, array.Length);
						}
					});
					thread5.Start();
					thread6.Start();
					thread6.Join();
					thread5.Join();
				}
			}
		}
		public override void Flush()
		{
		}
		public PipeStream()
		{
			this.m_Buffers = new byte[PipeStream.s_NumberOfBuffers][];
			for (int i = 0; i < PipeStream.s_NumberOfBuffers; i++)
			{
				this.m_Buffers[i] = new byte[PipeStream.s_BufferSize];
				this.m_writeQueue.Enqueue(this.m_Buffers[i]);
			}
			this.m_currentWrite = this.m_writeQueue.DequeueBlock();
			this.m_writer = new MemoryStream(this.m_currentWrite);
		}
		protected override void Dispose(bool disposing)
		{
			if (this.m_reader != null)
			{
				this.m_reader.Dispose();
			}
			if (this.m_writer != null)
			{
				this.m_writer.Dispose();
			}
			base.Dispose(disposing);
		}
		private MemoryStream OpenReadBuffer()
		{
			if (this.m_isEndWriting && this.m_readQueue.Count == 0)
			{
				return null;
			}
			this.m_currentRead = this.m_readQueue.DequeueBlock();
			this.m_reader = new MemoryStream(this.m_currentRead);
			return this.m_reader;
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.m_reader == null && this.OpenReadBuffer() == null)
			{
				return 0;
			}
			int i = 0;
			while (i < count)
			{
				int num = this.m_reader.Read(buffer, offset + i, count - i);
				i += num;
				if (num < count - i)
				{
					this.m_writeQueue.EnqueueBlock(this.m_currentRead);
					this.m_reader.Dispose();
					if (this.OpenReadBuffer() == null)
					{
						break;
					}
				}
			}
			return i;
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			int i = 0;
			while (i < count)
			{
				int num = count - i;
				if ((long)num > (long)PipeStream.s_BufferSize - this.m_writer.Position)
				{
					num = (int)((long)PipeStream.s_BufferSize - this.m_writer.Position);
				}
				this.m_writer.Write(buffer, offset + i, num);
				i += num;
				if (this.m_writer.Position == (long)PipeStream.s_BufferSize)
				{
					this.m_readQueue.EnqueueBlock(this.m_currentWrite);
					this.m_writer.Dispose();
					this.m_currentWrite = this.m_writeQueue.DequeueBlock();
					this.m_writer = new MemoryStream(this.m_currentWrite);
				}
			}
		}
		public void EndWriting()
		{
			Array.Resize<byte>(ref this.m_currentWrite, (int)this.m_writer.Position);
			this.m_readQueue.EnqueueBlock(this.m_currentWrite);
			this.m_isEndWriting = true;
			this.m_writer.Dispose();
			this.m_writer = null;
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}
	}
}
