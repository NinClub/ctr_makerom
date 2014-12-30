using System;
namespace Nintendo.MakeRom
{
	internal class ReservedBlock : ByteArrayData
	{
		private const byte FILLING_BYTE = 0;
		public ReservedBlock(uint size) : base(size)
		{
			for (int i = 0; i < base.Data.Length; i++)
			{
				base.Data[i] = 0;
			}
		}
	}
}
