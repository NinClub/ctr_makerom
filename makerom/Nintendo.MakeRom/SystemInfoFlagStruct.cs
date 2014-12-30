using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	unsafe internal struct SystemInfoFlagStruct
	{
		public fixed byte reserved[5];
		public byte flag;
		public ushort remasterVersion;
	}
}
