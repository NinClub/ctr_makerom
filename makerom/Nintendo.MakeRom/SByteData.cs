using System;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class SByteData : TData<sbyte>
	{
		public SByteData()
		{
		}
		public SByteData(sbyte i) : base(i)
		{
		}
		public static implicit operator SByteData(sbyte i)
		{
			return new SByteData(i);
		}
		public static implicit operator sbyte(SByteData data)
		{
			return data.Data;
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			writer.Write(base.Data);
		}
	}
}
