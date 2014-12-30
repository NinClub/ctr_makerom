using System;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class UInt64Data : TData<ulong>
	{
		public UInt64Data()
		{
		}
		public UInt64Data(ulong i) : base(i)
		{
		}
		public static implicit operator UInt64Data(ulong i)
		{
			return new UInt64Data(i);
		}
		public static implicit operator ulong(UInt64Data data)
		{
			return data.Data;
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			writer.Write(base.Data);
		}
	}
}
