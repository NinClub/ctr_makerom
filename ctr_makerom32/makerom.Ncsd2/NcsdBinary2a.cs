using Nintendo.MakeRom;
using System;
namespace makerom.Ncsd2
{
	internal class NcsdBinary2a : Ncsd
	{
		public NcsdBinary2a(MakeCciOptions options) : base(options)
		{
			options.CxiOption.DotRomfsPath = options.Cci2b;
		}
		protected override void Update()
		{
			base.Update();
			base.ClearBinaries();
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_Header,
				this.m_CardInfo,
				this.m_Padding
			});
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_NcchArray[0]
			});
		}
	}
}
