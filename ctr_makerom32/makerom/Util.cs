using System;
using System.Linq;
namespace makerom
{
	internal class Util
	{
		public static byte[] MakePaddingData(uint paddingSize, byte paddingChar)
		{
			if (paddingSize == 0u)
			{
				return new byte[0];
			}
			byte[] first = new byte[0];
			if (paddingSize > 2147483647u)
			{
				first = first.Concat(Enumerable.Repeat<byte>(paddingChar, 2147483647).ToArray<byte>()).ToArray<byte>();
				paddingSize -= 2147483647u;
			}
			return first.Concat(Enumerable.Repeat<byte>(paddingChar, (int)paddingSize).ToArray<byte>()).ToArray<byte>();
		}
		public static uint CalculatePaddingSize(int size, int align)
		{
			if (size % align == 0)
			{
				return 0u;
			}
			return (uint)(align - size % align);
		}
		public static uint ByteToMediaUnit(long byteSize, byte mediaUnitSize)
		{
			return (uint)(byteSize >> (int)(mediaUnitSize + 9));
		}
		public static long MediaUnitToByte(uint size, byte mediaUnitSize)
		{
			return (long)((long)((ulong)size) << (int)(mediaUnitSize + 9));
		}
		public static string GetRelativePath(string from, string target)
		{
			Uri uri = new Uri(from);
			Uri uri2 = new Uri(uri, target);
			string text = uri.MakeRelativeUri(uri2).ToString();
			return text.Replace('/', '\\');
		}
	}
}
