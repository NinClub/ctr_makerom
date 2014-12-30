using System;
using System.Runtime.InteropServices;
namespace Nintendo.RelocatableObject
{
	[StructLayout(LayoutKind.Sequential)]
	public class RsaData
	{
		private const int RSA_DATA_SIZE = 256;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] value;
		public RsaData()
		{
			this.value = new byte[256];
		}
	}
}
