using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	internal class CodeBinaries : WritableBinaryRegistory
	{
		private List<ByteArrayData> m_Data = new List<ByteArrayData>();
		private List<ByteArrayData> m_Paddings = new List<ByteArrayData>();
		private int m_PaddingAlign;
		private byte m_PaddingChar;
		private uint GetPaddingSize(int length, int paddingAlign)
		{
			if (length % paddingAlign == 0)
			{
				return 0u;
			}
			return (uint)(paddingAlign - length % paddingAlign);
		}
		public CodeBinaries(int paddingAlign, byte paddingChar)
		{
			this.m_PaddingAlign = paddingAlign;
			this.m_PaddingChar = paddingChar;
		}
		public void AddData(ByteArrayData data)
		{
			this.m_Data.Add(data);
			this.m_Paddings.Add(new ByteArrayData(Util.MakePaddingData(this.GetPaddingSize((int)data.Size, this.m_PaddingAlign), this.m_PaddingChar)));
		}
		protected override void Update()
		{
			base.ClearBinaries();
			for (int i = 0; i < this.m_Data.Count; i++)
			{
				base.AddBinaries(new IWritableBinary[]
				{
					this.m_Data[i]
				});
				base.AddBinaries(new IWritableBinary[]
				{
					this.m_Paddings[i]
				});
			}
		}
	}
}
