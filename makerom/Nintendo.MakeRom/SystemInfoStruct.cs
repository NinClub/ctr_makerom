using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	unsafe internal struct SystemInfoStruct
	{
		public ulong SaveDataSize;
		public ulong JumpId;
		public fixed byte Reserved2[48];
	}
}
