using System;
using System.Xml.Serialization;
namespace Nintendo.MakeRom
{
	public class CodeInfo
	{
		private int m_CompressedSize;
		public bool Compressed
		{
			get;
			set;
		}
		[XmlIgnore]
		public int OriginalSize
		{
			get;
			set;
		}
		[XmlIgnore]
		public int CompressedSize
		{
			get
			{
				if (!this.Compressed)
				{
					return 0;
				}
				return this.m_CompressedSize;
			}
			set
			{
				this.m_CompressedSize = value;
				this.Compressed = true;
			}
		}
		[XmlElement(ElementName = "OriginalSize")]
		public string OriginalSizeString
		{
			get
			{
				return this.GetByteSizeString(this.OriginalSize);
			}
			set
			{
			}
		}
		[XmlElement(ElementName = "CompressedSize")]
		public string CompressedSizeString
		{
			get
			{
				if (this.CompressedSize == 0)
				{
					return null;
				}
				return this.GetByteSizeString(this.CompressedSize);
			}
			set
			{
			}
		}
		public string CompressedRate
		{
			get
			{
				if (!this.Compressed || this.OriginalSize == 0)
				{
					return null;
				}
				return string.Format("{0:f1} %", (float)this.CompressedSize * 100f / (float)this.OriginalSize);
			}
			set
			{
			}
		}
		private string GetByteSizeString(int size)
		{
			return string.Format("{0:#,#} byte", size);
		}
	}
}
