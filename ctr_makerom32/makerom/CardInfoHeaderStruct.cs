using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace makerom
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	unsafe internal struct CardInfoHeaderStruct
	{
		public const int MAC_SIZE = 16;
		public const int RANDOM_SIZE = 16;
		public const int NONCE_SIZE = 12;
		public const int NCCH_HEADER_SIZE = 256;
		public const int INITIAL_DATA_SIZE = 48;
		public const int TITLE_KEY_SIZE = 16;
		public ulong CardInfo;
		public fixed byte Reserved1[3576];
		public ulong MediaId;
		public ulong Reserved2;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
		public byte[] InitialData;
		public fixed byte Reserved3[192];
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] NcchHeader;
		public fixed byte CardDeviceReserved1[512];
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] TitleKey;
		public fixed byte CardDeviceReserved2[240];
	}
}
