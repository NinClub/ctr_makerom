using System;
namespace Nintendo.MakeRom
{
	internal class NcchCipExtendedHeader : NcchExtendedHeader
	{
		public NcchCipExtendedHeader(AccessControlInfo accContInfo, SystemControlInfo sysContInfo, MakeCxiOptions options) : base(accContInfo, sysContInfo, options)
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
				this.m_SystemControlInfo,
				this.m_AccessControlInfo
			});
		}
	}
}
