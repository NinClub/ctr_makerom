using Nintendo.MakeRom.MakeFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
namespace Nintendo.MakeRom.Ncch.FastBuildRomfs
{
	[XmlRoot(ElementName = "RomFsInfo")]
	public class FastBuildRomFsInfo
	{
		[XmlIgnore]
		private List<Entry.Input> m_entries = new List<Entry.Input>();
		[XmlIgnore]
		private List<Creator.Input> m_creators = new List<Creator.Input>();
		public string Root
		{
			get;
			set;
		}
		[XmlArrayItem(ElementName = "Name")]
		public List<string> Files
		{
			get;
			set;
		}
		[XmlArrayItem(ElementName = "Entry")]
		public List<LayoutLocal> Entries
		{
			get;
			set;
		}
		public FastBuildRomFsInfo()
		{
			this.Entries = new List<LayoutLocal>();
			this.Files = new List<string>();
		}
		internal void AddFile(Entry.Input entryName, Creator.Input creator)
		{
			LayoutLocal layoutLocal = new LayoutLocal();
			layoutLocal.CtrPath = entryName.m_PathName;
			layoutLocal.Resource = new DataResource(creator.m_PCName);
			layoutLocal.m_Position = new HexableNumber((long)creator.m_Offset);
			this.Entries.Add(layoutLocal);
			this.m_entries.Add(entryName);
			this.Files.Add(creator.m_PCName.Replace(this.Root, ""));
			this.m_creators.Add(creator);
		}
		private void LoadFileResource(LayoutLocal layout)
		{
			if (layout.Resource.m_offset.GetInt64() != 0L)
			{
				throw new NotImplementedException("Currently Resource Offset is not Supported");
			}
			if (layout.Resource.Filename == null)
			{
				throw new FormatException("No Filename Attribute");
			}
			long length = new FileInfo(layout.Resource.Filename).Length;
			if (layout.Resource.m_size.GetInt64() != 0L && layout.Resource.m_size.GetInt64() != length)
			{
				throw new NotImplementedException("Currently Resource Size is not Supported");
			}
			layout.Resource.m_size.SetInt64(length);
		}
		private void LoadInlineStringResource(LayoutLocal layout)
		{
			throw new NotImplementedException("Currently Supported Type is only File");
		}
		private void LoadInlineHexResource(LayoutLocal layout)
		{
			throw new NotImplementedException("Currently Supported Type is only File");
		}
		internal void MakeEntriesAndCreators()
		{
			this.m_entries.Clear();
			this.m_creators.Clear();
			LayoutLocal[] array = this.Entries.ToArray();
			LayoutLocal[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				LayoutLocal layoutLocal = array2[i];
				if (layoutLocal.CtrPath == null)
				{
					throw new FormatException("No CtrPath Attribute");
				}
				switch (layoutLocal.Resource.Type)
				{
				case ResourceType.File:
					this.LoadFileResource(layoutLocal);
					break;
				case ResourceType.InlineString:
					this.LoadInlineStringResource(layoutLocal);
					break;
				case ResourceType.InlineHex:
					this.LoadInlineHexResource(layoutLocal);
					break;
				}
			}
			Array.Sort<LayoutLocal>(array);
			LayoutLocal[] array3 = array;
			for (int j = 0; j < array3.Length; j++)
			{
				LayoutLocal layoutLocal2 = array3[j];
				this.m_entries.Add(new Entry.Input
				{
					m_Offset = (ulong)layoutLocal2.m_Position.GetInt64(),
					m_PathName = layoutLocal2.CtrPath,
					m_Size = (ulong)layoutLocal2.Resource.m_size.GetInt64()
				});
				this.m_creators.Add(new Creator.Input
				{
					m_Offset = (ulong)layoutLocal2.m_Position.GetInt64(),
					m_PCName = layoutLocal2.Resource.Filename,
					m_Size = (ulong)layoutLocal2.Resource.m_size.GetInt64()
				});
			}
		}
		internal Entry.Input[] GetEntries()
		{
			return this.m_entries.ToArray();
		}
		internal Creator.Input[] GetCreators()
		{
			return this.m_creators.ToArray();
		}
	}
}
