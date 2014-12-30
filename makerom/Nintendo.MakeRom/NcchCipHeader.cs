using System;
namespace Nintendo.MakeRom
{
	internal class NcchCipHeader : NcchHeader
	{
		public NcchCipHeader(NcchExtendedHeader exHeader, MakeCxiOptions options) : base(exHeader, null, options)
		{
			this.CheckSize();
		}
		protected override void CheckSize()
		{
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_ExtendedHeader
			});
		}
	}
}
