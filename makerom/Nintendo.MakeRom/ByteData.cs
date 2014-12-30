using System;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class ByteData : TData<byte>
	{
		public ByteData()
		{
		}
		public ByteData(byte i) : base(i)
		{
		}
		public static implicit operator ByteData(byte i)
		{
			return new ByteData(i);
		}
		public static implicit operator byte(ByteData data)
		{
			return data.Data;
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			writer.Write(base.Data);
		}
	}
}
