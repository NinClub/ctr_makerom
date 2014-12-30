using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	internal class ElfSegment
	{
		public ElfProgramHeader Header
		{
			get;
			private set;
		}
		public ElfSectionHeaderInfo[] Sections
		{
			get;
			private set;
		}
		public string Name
		{
			get;
			private set;
		}
		public uint VAddr
		{
			get;
			private set;
		}
		private static bool IsIgnoreSection(ElfSectionHeaderInfo info)
		{
			if (info.Header.Address != 0u)
			{
				return false;
			}
			if (info.Header.Type != 1u && info.Header.Type != 0u)
			{
				return true;
			}
			string[] array = new string[]
			{
				".debug_abbrev",
				".debug_frame",
				".debug_info",
				".debug_line",
				".debug_loc",
				".debug_pubnames",
				".comment"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string a = array2[i];
				if (a == info.Name)
				{
					return true;
				}
			}
			return false;
		}
		public static ElfSegment[] CreateElfSegments(ElfProgramHeader[] headers, ElfSectionHeaderInfo[] elfSections)
		{
			List<ElfSegment> list = new List<ElfSegment>();
			int num = 0;
			for (int i = 0; i < headers.Length; i++)
			{
				ElfProgramHeader elfProgramHeader = headers[i];
				if (elfProgramHeader.MemorySize != 0u && elfProgramHeader.Type == 1u)
				{
					bool flag = false;
					uint num2 = 0u;
					uint vAddr = elfProgramHeader.VAddr;
					uint memorySize = elfProgramHeader.MemorySize;
					ElfSegment elfSegment = new ElfSegment();
					List<ElfSectionHeaderInfo> list2 = new List<ElfSectionHeaderInfo>();
					int j = num;
					while (j < elfSections.Length)
					{
						if (flag)
						{
							goto IL_CC;
						}
						if (elfSections[j].Header.Address == vAddr)
						{
							while (j < elfSections.Length && elfSections[j].Header.Address == vAddr && !ElfSegment.IsIgnoreSection(elfSections[j]))
							{
								j++;
							}
							j--;
							flag = true;
							elfSegment.VAddr = elfSections[j].Header.Address;
							elfSegment.Name = elfSections[j].Name;
							goto IL_CC;
						}
						IL_114:
						j++;
						continue;
						IL_CC:
						list2.Add(elfSections[j]);
						num2 += elfSections[j].Header.Size;
						if (num2 == memorySize)
						{
							break;
						}
						if (num2 > memorySize)
						{
							throw new ArgumentException(string.Format("Too large section size.\n Segment size = {0:x}\n Section Size = {1:x}\n", memorySize, num2));
						}
						goto IL_114;
					}
					elfSegment.Header = elfProgramHeader;
					elfSegment.Sections = list2.ToArray();
					list.Add(elfSegment);
				}
			}
			return list.ToArray();
		}
	}
}
