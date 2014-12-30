using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	public class Warning
	{
		private static List<int> s_NoPrintList = new List<int>();
		public static List<int> NoPrintList
		{
			get
			{
				return Warning.s_NoPrintList;
			}
			set
			{
				Warning.s_NoPrintList = value;
			}
		}
		public static void PrintWarning(string message, int warningNum)
		{
			if (!Warning.NoPrintList.Contains(warningNum))
			{
				Console.WriteLine("[MAKEROM WARNING] {0}", message);
			}
		}
	}
}
