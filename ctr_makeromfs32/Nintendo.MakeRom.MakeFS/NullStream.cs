using System;
using System.IO;
namespace Nintendo.MakeRom.MakeFS
{
	public class NullStream : Stream
	{
		private long m_Position;
		private long m_Length;
		public override bool CanRead
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
		public override bool CanSeek
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
				return this.m_Length;
			}
		}
		public override long Position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				this.m_Position = value;
				if (this.m_Position > this.m_Length)
				{
					this.m_Length = this.m_Position;
				}
			}
		}
		public override void Flush()
		{
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = this.Position;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = offset;
				break;
			case SeekOrigin.Current:
				num = this.Position + offset;
				break;
			case SeekOrigin.End:
				num = this.Length + offset;
				break;
			}
			if (num < 0L)
			{
				throw new ArgumentException("Attempt to seek before start of stream.");
			}
			this.Position = num;
			return num;
		}
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotImplementedException("This stream doesn't support reading.");
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException("This stream doesn't support reading.");
		}
		public override void SetLength(long value)
		{
			this.m_Length = value;
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.Seek((long)count, SeekOrigin.Current);
		}
	}
}
