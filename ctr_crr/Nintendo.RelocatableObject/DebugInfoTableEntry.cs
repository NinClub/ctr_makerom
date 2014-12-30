using System;
using System.Runtime.InteropServices;
namespace Nintendo.RelocatableObject
{
	[StructLayout(LayoutKind.Sequential)]
	public class DebugInfoTableEntry
	{
		public const int SIZE = 8;
		public int bodyOffset;
		public int bodySize;
	}
}
