using System;
namespace Nintendo.MakeRom
{
	internal class SystemControlInfo : WritableBinaryRegistory
	{
		private CoreInfo m_CoreInfo;
		private SystemInfo m_SystemInfo;
		public SystemControlInfo(CoreInfo coreInfo, SystemInfo systemInfo)
		{
			this.m_CoreInfo = coreInfo;
			this.m_SystemInfo = systemInfo;
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_CoreInfo,
				this.m_SystemInfo
			});
		}
	}
}
