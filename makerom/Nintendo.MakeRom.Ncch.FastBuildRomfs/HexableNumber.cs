using System;
using System.Xml.Serialization;
namespace Nintendo.MakeRom.Ncch.FastBuildRomfs
{
	public class HexableNumber
	{
		[XmlIgnore]
		private long m_number;
		[XmlText]
		public string Number
		{
			get
			{
				return string.Format("0x{0}", this.m_number.ToString("X"));
			}
			set
			{
				this.m_number = Convert.ToInt64(value, 16);
			}
		}
		public HexableNumber()
		{
			this.m_number = 0L;
		}
		public HexableNumber(long number)
		{
			this.SetInt64(number);
		}
		public long GetInt64()
		{
			return this.m_number;
		}
		public void SetInt64(long value)
		{
			this.m_number = value;
		}
	}
}
