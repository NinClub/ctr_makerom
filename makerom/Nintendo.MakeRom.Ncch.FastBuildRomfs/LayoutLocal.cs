using System;
using System.Xml.Serialization;
namespace Nintendo.MakeRom.Ncch.FastBuildRomfs
{
	public class LayoutLocal : IComparable<LayoutLocal>
	{
		[XmlAttribute]
		public string CtrPath
		{
			get;
			set;
		}
		public DataResource Resource
		{
			get;
			set;
		}
		[XmlAttribute]
		public string Position
		{
			get
			{
				return this.m_Position.Number;
			}
			set
			{
				this.m_Position = new HexableNumber();
				this.m_Position.Number = value;
			}
		}
		[XmlIgnore]
		public HexableNumber m_Position
		{
			get;
			set;
		}
		public int CompareTo(LayoutLocal other)
		{
			if (this == other)
			{
				return 0;
			}
			int num = this.m_Position.GetInt64().CompareTo(other.m_Position.GetInt64());
			if (num != 0)
			{
				return num;
			}
			if (this.Resource.m_size.GetInt64() == 0L && other.Resource.m_size.GetInt64() == 0L)
			{
				return 0;
			}
			if (this.Resource.m_size.GetInt64() == 0L)
			{
				return -1;
			}
			if (other.Resource.m_size.GetInt64() == 0L)
			{
				return 1;
			}
			throw new FormatException("Same Position Layout Occurs");
		}
	}
}
