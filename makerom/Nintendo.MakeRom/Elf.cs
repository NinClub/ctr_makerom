using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Nintendo.MakeRom
{
	internal class Elf
	{
		public delegate bool SectionHeaderMatchingFunc(ElfSectionHeader header);
		private readonly Stream m_DataStream;
		private readonly ElfHeader m_Header;
		private readonly ElfSectionHeader[] m_SectionHeaders;
		private readonly MakeCxiOptions m_Options;
		private Dictionary<string, ElfSectionHeaderInfo> m_SectionDictionary;
		private readonly ElfSectionHeaderInfo[] m_SectionHeaderInfos;
		private readonly ElfProgramHeader[] m_ProgramHeaders;
		private readonly ElfSegment[] m_Segments;
		private Dictionary<string, ElfSegment> m_SegmentDictionary;
		public Elf(Stream stream, MakeCxiOptions options)
		{
			this.m_DataStream = stream;
			this.m_Header = new ElfHeader(this.m_DataStream);
			this.m_Options = options;
			ushort numSections = this.m_Header.GetNumSections();
			this.m_SectionHeaders = new ElfSectionHeader[(int)numSections];
			this.m_SectionHeaderInfos = new ElfSectionHeaderInfo[(int)numSections];
			ushort sectionHeaderEntrySize = this.m_Header.GetSectionHeaderEntrySize();
			for (int i = 0; i < this.m_SectionHeaders.Length; i++)
			{
				int offset = (int)(this.m_Header.GetSectionHeaderTableOffset() + (uint)((int)sectionHeaderEntrySize * i));
				this.m_SectionHeaders[i] = new ElfSectionHeader(this.m_DataStream, offset, sectionHeaderEntrySize);
			}
			for (int j = 0; j < this.m_SectionHeaders.Length; j++)
			{
				int arg_A0_0 = this.m_SectionHeaders[j].NameIndex;
				this.m_SectionHeaderInfos[j] = new ElfSectionHeaderInfo(this.m_SectionHeaders[j], j, this.GetSectionName(this.m_SectionHeaders[j].NameIndex));
			}
			this.m_SectionDictionary = new Dictionary<string, ElfSectionHeaderInfo>();
			for (int k = 0; k < this.m_SectionHeaders.Length; k++)
			{
				if (!this.m_SectionDictionary.ContainsKey(this.m_SectionHeaderInfos[k].Name))
				{
					this.m_SectionDictionary.Add(this.m_SectionHeaderInfos[k].Name, this.m_SectionHeaderInfos[k]);
				}
			}
			ushort numPrograms = this.m_Header.GetNumPrograms();
			ushort programHeaderEntrySize = this.m_Header.GetProgramHeaderEntrySize();
			this.m_ProgramHeaders = new ElfProgramHeader[(int)numPrograms];
			for (int l = 0; l < this.m_ProgramHeaders.Length; l++)
			{
				int offset2 = (int)(this.m_Header.GetProgramHeaderTableOffset() + (uint)((int)programHeaderEntrySize * l));
				this.m_ProgramHeaders[l] = new ElfProgramHeader(this.m_DataStream, offset2, programHeaderEntrySize);
			}
			this.m_Segments = ElfSegment.CreateElfSegments(this.m_ProgramHeaders, this.m_SectionHeaderInfos);
			this.m_SegmentDictionary = new Dictionary<string, ElfSegment>();
			for (int m = 0; m < this.m_Segments.Length; m++)
			{
				if (this.m_SegmentDictionary.ContainsKey(this.m_Segments[m].Name))
				{
					throw new MakeromException(string.Format("\"{0}\" segment is already exists", this.m_Segments[m].Name));
				}
				this.m_SegmentDictionary.Add(this.m_Segments[m].Name, this.m_Segments[m]);
			}
		}
		public ElfSectionHeader GetSectionHeader(string name)
		{
			if (this.m_SectionDictionary.ContainsKey(name))
			{
				return this.m_SectionDictionary[name].Header;
			}
			return null;
		}
		public ElfHeader GetHeader()
		{
			return this.m_Header;
		}
		public byte[] GetData(List<ElfSectionHeader> headers)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			foreach (ElfSectionHeader current in headers)
			{
				byte[] buffer = new byte[current.Size];
				if (!current.IsBss())
				{
					this.m_DataStream.Seek((long)((ulong)current.Offset), SeekOrigin.Begin);
					this.m_DataStream.Read(buffer, 0, (int)current.Size);
				}
				binaryWriter.Write(buffer);
			}
			binaryWriter.Flush();
			return memoryStream.ToArray();
		}
		public byte[] GetData(uint offset, uint size)
		{
			byte[] array = new byte[size];
			this.m_DataStream.Seek((long)((ulong)offset), SeekOrigin.Begin);
			this.m_DataStream.Read(array, 0, (int)size);
			return array;
		}
		public ElfSectionHeader GetSectionHeader(int index)
		{
			return this.m_SectionHeaders[index];
		}
		public List<ElfSectionHeader> FindSectionHeaders(params string[] sectionNames)
		{
			List<ElfSectionHeader> list = new List<ElfSectionHeader>();
			int num = 0;
			for (int i = 0; i < sectionNames.Length; i++)
			{
				string text = sectionNames[i];
				if (!this.m_SectionDictionary.Keys.Contains(text))
				{
					break;
				}
				ElfSectionHeaderInfo elfSectionHeaderInfo = this.m_SectionDictionary[text];
				if (list.Count != 0)
				{
					if (num + 1 != elfSectionHeaderInfo.Index)
					{
						throw new Exception(string.Format("{0} and {1} are must be continuous.", this.m_SectionHeaderInfos[num].Name, this.m_SectionHeaderInfos[elfSectionHeaderInfo.Index].Name));
					}
				}
				else
				{
					if (!this.m_Options.AllowsUnalignedSection && elfSectionHeaderInfo.Header.Address % this.m_Options.PageSize != 0u)
					{
						throw new InvalidDataException("First found section's address is not page aligned.");
					}
				}
				num = elfSectionHeaderInfo.Index;
				list.Add(elfSectionHeaderInfo.Header);
			}
			return list;
		}
		public List<ElfSegment> GetContinuousSegments(params string[] names)
		{
			List<ElfSegment> segments = this.GetSegments(names);
			if (segments == null || segments.Count == 0)
			{
				return null;
			}
			if (segments.Count == 1)
			{
				return segments;
			}
			uint num = segments[0].VAddr + segments[0].Header.MemorySize;
			for (int i = 1; i < segments.Count; i++)
			{
				uint num2 = num + segments[i].Header.Align - 1u & ~(segments[i].Header.Align - 1u);
				if (segments[i].VAddr != num2)
				{
					throw new MakeromException(string.Format("{0} segment and {1} segment are not continuous", segments[i].Name, segments[i - 1].Name));
				}
			}
			return segments;
		}
		public List<ElfSegment> GetSegments(params string[] names)
		{
			if (names == null)
			{
				return null;
			}
			List<ElfSegment> list = new List<ElfSegment>();
			for (int i = 0; i < names.Length; i++)
			{
				string key = names[i];
				if (this.m_SegmentDictionary.ContainsKey(key))
				{
					list.Add(this.m_SegmentDictionary[key]);
				}
			}
			return list;
		}
		public List<ElfSectionHeader> FindSectionHeaders(Elf.SectionHeaderMatchingFunc matchingFunc, string sectionName, Dictionary<string, List<string>> sections)
		{
			List<ElfSectionHeader> list = new List<ElfSectionHeader>();
			bool flag = false;
			bool flag2 = false;
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			foreach (KeyValuePair<string, List<string>> current in sections)
			{
				if (current.Key == sectionName)
				{
					list2 = current.Value;
				}
				else
				{
					list3.AddRange(current.Value);
				}
			}
			ElfSectionHeaderInfo[] sectionHeaderInfos = this.m_SectionHeaderInfos;
			for (int i = 0; i < sectionHeaderInfos.Length; i++)
			{
				ElfSectionHeaderInfo elfSectionHeaderInfo = sectionHeaderInfos[i];
				if (!flag2 && !list3.Contains(elfSectionHeaderInfo.Name) && (matchingFunc(elfSectionHeaderInfo.Header) || list2.Contains(elfSectionHeaderInfo.Name)))
				{
					if (flag)
					{
						if (elfSectionHeaderInfo.Header.Size == 0u)
						{
							goto IL_141;
						}
					}
					else
					{
						if (!elfSectionHeaderInfo.Header.IsBss() && !this.m_Options.AllowsUnalignedSection && elfSectionHeaderInfo.Header.Address % 4096u != 0u)
						{
							throw new InvalidDataException("First found section's address is not page aligned.");
						}
					}
					list.Add(elfSectionHeaderInfo.Header);
					flag = true;
				}
				else
				{
					if (flag)
					{
						flag2 = true;
					}
					if (flag2 && list2.Contains(elfSectionHeaderInfo.Name))
					{
						throw new MakeromException(string.Format("\"{0}\" section's address is invalid", elfSectionHeaderInfo.Name));
					}
				}
				IL_141:;
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list;
		}
		private string GetSectionName(int nameIndex)
		{
			ElfSectionHeader elfSectionHeader = this.m_SectionHeaders[(int)this.m_Header.GetSectionNameTableIndex()];
			byte[] data = this.GetData(elfSectionHeader.Offset, elfSectionHeader.Size);
			List<byte> list = new List<byte>();
			int num = nameIndex;
			while (num < data.Length && data[num] != 0)
			{
				list.Add(data[num]);
				num++;
			}
			return Encoding.UTF8.GetString(list.ToArray());
		}
	}
}
