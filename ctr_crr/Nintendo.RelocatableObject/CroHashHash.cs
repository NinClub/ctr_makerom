using System;
using System.Runtime.InteropServices;
namespace Nintendo.RelocatableObject
{
	[StructLayout(LayoutKind.Sequential)]
	public class CroHashHash
	{
		private const int HASH_SIZE = 32;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] hash = new byte[32];
		public CroHashHash()
		{
		}
		public CroHashHash(byte[] hash)
		{
			this.hash = hash;
		}
		public static int Comparison(CroHashHash x, CroHashHash y)
		{
			byte[] array = x.hash;
			byte[] array2 = y.hash;
			int i = 0;
			while (i < 32)
			{
				if (array[i] != array2[i])
				{
					if (array[i] < array2[i])
					{
						return -1;
					}
					return 1;
				}
				else
				{
					i++;
				}
			}
			return 0;
		}
	}
}
