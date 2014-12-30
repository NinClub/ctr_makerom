using System;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class UInt16Data : TData<ushort>
	{
		public UInt16Data(ushort i) : base(i)
		{
		}
		public static implicit operator UInt16Data(ushort i)
		{
			return new UInt16Data(i);
		}
		public static implicit operator ushort(UInt16Data data)
		{
			return data.Data;
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			writer.Write(base.Data);
		}
	}
}
