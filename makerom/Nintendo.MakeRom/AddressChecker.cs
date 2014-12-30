using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	internal class AddressChecker
	{
		private List<Range> m_RWAddresses = new List<Range>();
		private List<Range> m_RAddresses = new List<Range>();
		public AddressChecker(string[] desc)
		{
			if (desc == null)
			{
				return;
			}
			for (int i = 0; i < desc.Length; i++)
			{
				string text = desc[i];
				string text2 = text.ToLower();
				List<Range> list;
				if (text2.IndexOf(":r") != -1)
				{
					text2 = text2.Remove(text2.IndexOf(":r"), 2);
					list = this.m_RAddresses;
				}
				else
				{
					list = this.m_RWAddresses;
				}
				list.Add(new Range(text2));
			}
		}
		public void CheckAddress(string[] user)
		{
			if (user == null)
			{
				return;
			}
			for (int i = 0; i < user.Length; i++)
			{
				string text = user[i];
				string text2 = text.ToLower();
				bool flag = false;
				if (text2.IndexOf(":r") != -1)
				{
					text2 = text2.Remove(text2.IndexOf(":r"), 2);
					flag = true;
				}
				Range range = new Range(text2);
				if (!this.IsRangeIncluded(this.m_RWAddresses, range) && (!flag || !this.IsRangeIncluded(this.m_RAddresses, range)))
				{
					throw new NotPermittedValueException("Map address", text2);
				}
			}
		}
		private bool IsRangeIncluded(List<Range> descList, Range range)
		{
			foreach (Range current in descList)
			{
				if (current.IsInclude(range))
				{
					return true;
				}
			}
			return false;
		}
	}
}
