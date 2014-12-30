using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	unsafe internal struct StorageInfoFlags
	{
		public fixed byte StorageAccessInfo[7];
		public byte OtherAttributes;
		public unsafe void SetStorageAccessInfoBit(int index, bool value)
		{
			int num = index / 8;
			int num2 = index % 8;
			fixed (byte* ptr = this.StorageAccessInfo)
			{
				byte b = ptr[num];
				if (value)
				{
					b |= (byte)(1 << num2);
				}
				else
				{
					b &= (byte)(~(byte)(1 << num2));
				}
				ptr[num] = b;
			}
		}
		public unsafe bool GetStorageAccessInfoBit(int index)
		{
			int num = index / 8;
			int num2 = index % 8;
			fixed (byte* ptr = this.StorageAccessInfo)
			{
				byte b = ptr[num];
				return (b & (byte)(1 << num2)) != 0;
			}
		}
	}
}
