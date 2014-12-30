using System;
namespace Nintendo.MakeRom
{
	internal class ExeSectionHeader : StructData<ExeSectionHeaderStruct>
	{
		public ExeSectionHeader()
		{
			this.CheckSize();
		}
		private void CheckSize()
		{
		}
	}
}
