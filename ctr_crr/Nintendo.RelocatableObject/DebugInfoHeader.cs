using System;
using System.Runtime.InteropServices;
namespace Nintendo.RelocatableObject
{
	[StructLayout(LayoutKind.Sequential)]
	public class DebugInfoHeader
	{
		public const int SIZE = 32;
		public int tableOffset;
		public int numTableEntry;
		public int bodyOffset;
		public int bodySize;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private uint[] padding;
		public DebugInfoHeader()
		{
			uint[] array = new uint[4];
			this.padding = array;
		}
	}
}
