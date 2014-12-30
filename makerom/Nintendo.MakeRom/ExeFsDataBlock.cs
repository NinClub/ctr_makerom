using System;
namespace Nintendo.MakeRom
{
	internal class ExeFsDataBlock : WritableBinaryRegistory
	{
		private CodeBinaries m_CodeBinaries;
		public ExeFsDataBlock(int paddingAlign, byte paddingChar)
		{
			this.m_CodeBinaries = new CodeBinaries(paddingAlign, paddingChar);
		}
		public void AddDataBlock(ByteArrayData data)
		{
			this.m_CodeBinaries.AddData(data);
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_CodeBinaries
			});
		}
	}
}
