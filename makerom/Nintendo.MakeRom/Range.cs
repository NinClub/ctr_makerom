using System;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Nintendo.MakeRom
{
	internal class Range
	{
		private uint m_Min;
		private uint m_Max;
		public Range(string value)
		{
			string[] array = value.Split(new char[]
			{
				'-'
			});
			if (array == null || array.Length == 0)
			{
				this.m_Min = 4294967295u;
				this.m_Max = 0u;
				return;
			}
			if (array.Length == 1)
			{
				this.m_Min = (this.m_Max = this.Convert(array[0]));
				return;
			}
			this.m_Min = this.Convert(array[0]);
			this.m_Max = this.Convert(array[1]);
		}
		public override string ToString()
		{
			return string.Format("{0,0:X8}-{1,0:X8}", this.m_Min, this.m_Max);
		}
		public bool IsInclude(Range other)
		{
			return this.m_Min <= other.m_Min && other.m_Max <= this.m_Max;
		}
		public bool IsInclude(uint num)
		{
			return this.m_Min <= num && num <= this.m_Max;
		}
		private uint Convert(string data)
		{
			data = data.Trim().ToLower();
			if (Regex.Match(data, "^[0-9]+\\z").Success)
			{
				return uint.Parse(data);
			}
			if (data.Substring(0, 2).Equals("0x") || Regex.Match(data, "^[0-9a-z]+\\z").Success)
			{
				return uint.Parse(data, NumberStyles.AllowHexSpecifier);
			}
			throw new Exception(data + " : failed convert to uint32");
		}
	}
}
