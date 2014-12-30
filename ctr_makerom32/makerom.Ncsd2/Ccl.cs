using Nintendo.MakeRom.Ncch.FastBuildRomfs;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace makerom.Ncsd2
{
	public class Ccl
	{
		public class Image
		{
			[XmlIgnore]
			internal HexableNumber m_loadAddress = new HexableNumber();
			[XmlIgnore]
			internal HexableNumber m_size = new HexableNumber();
			[XmlIgnore]
			internal HexableNumber m_offset = new HexableNumber();
			[XmlIgnore]
			internal HexableNumber m_lastModified = new HexableNumber();
			[XmlAttribute]
			public string LoadAddress
			{
				get
				{
					return string.Format("0x{0}", this.m_loadAddress.GetInt64().ToString("x16"));
				}
				set
				{
					this.m_loadAddress.Number = value;
				}
			}
			[XmlAttribute]
			public string Size
			{
				get
				{
					return string.Format("0x{0}", this.m_size.GetInt64().ToString("x16"));
				}
				set
				{
					this.m_size.Number = value;
				}
			}
			[XmlAttribute]
			public string FileOffset
			{
				get
				{
					return string.Format("0x{0}", this.m_offset.GetInt64().ToString("x16"));
				}
				set
				{
					this.m_offset.Number = value;
				}
			}
			[XmlAttribute]
			public string LastModified
			{
				get
				{
					return string.Format("0x{0}", this.m_lastModified.GetInt64().ToString("x16"));
				}
				set
				{
					this.m_lastModified.Number = value;
				}
			}
			[XmlAttribute]
			public string Path
			{
				get;
				set;
			}
			public Image()
			{
			}
			public Image(long addr, long size, long offset, long modified)
			{
				this.m_loadAddress = new HexableNumber(addr);
				this.m_size = new HexableNumber(size);
				this.m_offset = new HexableNumber(offset);
				this.m_lastModified = new HexableNumber(modified);
			}
		}
		[XmlArray(ElementName = "Images")]
		public List<Ccl.Image> Images = new List<Ccl.Image>();
	}
}
