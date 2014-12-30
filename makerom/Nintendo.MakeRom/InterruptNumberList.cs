using System;
using System.Collections.Generic;
using System.Linq;
namespace Nintendo.MakeRom
{
	internal class InterruptNumberList : WritableBinaryRegistory
	{
		private const int NUM_DESCRIPTORS = 8;
		private const int NUM_INTNUM_PER_DESC = 4;
		private List<string> m_InterruptNumberList = new List<string>();
		private List<InterruptNumberDescriptor> m_InterruptNumberDescList = new List<InterruptNumberDescriptor>();
		public InterruptNumberList(string intNumListText)
		{
			char[] separator = new char[]
			{
				','
			};
			List<string> list = new List<string>(intNumListText.Split(separator, StringSplitOptions.RemoveEmptyEntries));
			int i = list.Count;
			int num = 0;
			while (i > 0)
			{
				int num2 = (i > 4) ? 4 : i;
				string[] array = list.Take(num2).ToArray<string>();
				this.m_InterruptNumberDescList.Add(new InterruptNumberDescriptor(array));
				this.m_InterruptNumberList.AddRange(array);
				list.RemoveRange(0, num2);
				i -= num2;
				num++;
			}
		}
		public string[] GetInterruptNumberList()
		{
			return this.m_InterruptNumberList.ToArray();
		}
		protected override void Update()
		{
			base.ClearBinaries();
			base.AddBinaries(new IWritableBinary[]
			{
				new ByteArrayData(0u)
			});
			base.AddBinaries(this.m_InterruptNumberDescList.ToArray());
		}
	}
}
