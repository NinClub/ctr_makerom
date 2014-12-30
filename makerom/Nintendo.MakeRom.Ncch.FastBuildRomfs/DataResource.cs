using System;
using System.IO;
using System.Xml.Serialization;
namespace Nintendo.MakeRom.Ncch.FastBuildRomfs
{
	public class DataResource
	{
		[XmlAttribute]
		public ResourceType Type;
		[XmlIgnore]
		public HexableNumber m_offset = new HexableNumber();
		[XmlIgnore]
		public HexableNumber m_size = new HexableNumber();
		[XmlAttribute]
		public string Offset
		{
			get
			{
				if (this.m_offset.GetInt64() != 0L)
				{
					return this.m_offset.Number;
				}
				return null;
			}
			set
			{
				this.m_offset.Number = value;
			}
		}
		[XmlAttribute]
		public string Size
		{
			get
			{
				if (this.m_size.GetInt64() != 0L)
				{
					return this.m_size.Number;
				}
				return null;
			}
			set
			{
				this.m_size.Number = value;
			}
		}
		[XmlAttribute]
		public string Filename
		{
			get;
			set;
		}
		[XmlText]
		public string Inline
		{
			get;
			set;
		}
		public DataResource()
		{
		}
		public DataResource(string filename)
		{
			this.Filename = filename;
			this.m_offset = new HexableNumber(0L);
			this.m_size = new HexableNumber(new FileInfo(filename).Length);
		}
	}
}
