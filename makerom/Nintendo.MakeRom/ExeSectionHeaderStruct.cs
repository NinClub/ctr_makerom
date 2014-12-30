using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	unsafe internal struct ExeSectionHeaderStruct
	{
		public fixed byte name[8];
		public uint offset;
		public uint size;
	}
}
