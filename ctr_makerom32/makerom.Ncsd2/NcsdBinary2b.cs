using System;
using System.IO;
namespace makerom.Ncsd2
{
	internal class NcsdBinary2b : Ncsd
	{
		private MakeCciOptions m_options;
		public override long Size
		{
			get
			{
				throw new NotSupportedException("");
			}
		}
		public NcsdBinary2b(MakeCciOptions options) : base(options)
		{
			this.m_options = options;
		}
		protected override void Update()
		{
			if (this.m_NcchArray[0] == null)
			{
				throw new Exception("Not found cxi on partition 0");
			}
			base.UpdatePartitionIds(this.m_NcchArray[0].GetProgramId());
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			this.Update();
			for (int i = 0; i < 8; i++)
			{
				if (this.m_NcchArray[i] != null)
				{
					this.m_NcchArray[i].WriteBinary(writer);
				}
			}
		}
	}
}
