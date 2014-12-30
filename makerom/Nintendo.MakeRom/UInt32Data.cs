using System;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class UInt32Data : TData<uint>
	{
		public UInt32Data(uint i) : base(i)
		{
		}
		public static implicit operator UInt32Data(uint i)
		{
			return new UInt32Data(i);
		}
		public static implicit operator uint(UInt32Data data)
		{
			return data.Data;
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			writer.Write(base.Data);
		}
	}
}
