using System;
namespace Nintendo.MakeRom
{
	internal class ElfSectionHeaderInfo
	{
		public ElfSectionHeader Header
		{
			get;
			private set;
		}
		public int Index
		{
			get;
			private set;
		}
		public string Name
		{
			get;
			private set;
		}
		public uint Offset
		{
			get;
			private set;
		}
		public ElfSectionHeaderInfo(ElfSectionHeader elfSectionHeader, int index, string name)
		{
			this.Header = elfSectionHeader;
			this.Index = index;
			this.Name = name;
			this.Offset = elfSectionHeader.Offset;
		}
	}
}
