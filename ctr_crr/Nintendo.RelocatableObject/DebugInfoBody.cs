using System;
using System.Runtime.InteropServices;
namespace Nintendo.RelocatableObject
{
	[StructLayout(LayoutKind.Sequential)]
	public class DebugInfoBody
	{
		public const int LOCAL_PATH_OFFSET = 8;
		public int pathOffset;
		public int pathLength;
		public byte[] path;
		public int Size
		{
			get
			{
				return 8 + DebugInfoBody.CalcPathArraySize(this.pathLength);
			}
		}
		private static int RoundUp(int a, int b)
		{
			return (a + (b - 1)) / b * b;
		}
		public static int CalcPathArraySize(int pl)
		{
			return DebugInfoBody.RoundUp((pl + 1) * 2, 4);
		}
	}
}
