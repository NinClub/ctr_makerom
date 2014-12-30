using System;
using System.Text;
namespace Nintendo.MakeRom
{
	public static class StringExtensions
	{
		public static uint ToUInt32ASCII(this string str)
		{
			return BitConverter.ToUInt32(Encoding.ASCII.GetBytes(str), 0);
		}
		public static ulong ToUInt64ASCII(this string str)
		{
			return BitConverter.ToUInt64(Encoding.ASCII.GetBytes(str), 0);
		}
		public static ushort ToUInt16ASCII(this string str)
		{
			return BitConverter.ToUInt16(Encoding.ASCII.GetBytes(str), 0);
		}
	}
}
