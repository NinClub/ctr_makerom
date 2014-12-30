using System;
using System.IO;
namespace Nintendo.MakeRom
{
	public class ByteArrayData : IWritableBinary
	{
		protected virtual int ByteSize
		{
			get
			{
				return -1;
			}
		}
		public byte[] Data
		{
			get;
			private set;
		}
		public long Size
		{
			get
			{
				return (long)this.Data.Length;
			}
		}
		protected ByteArrayData()
		{
			this.Data = new byte[this.ByteSize];
		}
		public ByteArrayData(uint size)
		{
			this.Data = new byte[size];
		}
		public ByteArrayData(byte[] bytes)
		{
			this.Data = bytes;
		}
		public static implicit operator ByteArrayData(byte[] bytes)
		{
			return new ByteArrayData(bytes);
		}
		public void WriteBinary(BinaryWriter writer)
		{
			writer.Write(this.Data);
		}
	}
}
