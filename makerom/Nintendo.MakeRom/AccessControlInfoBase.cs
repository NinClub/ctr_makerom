using System;
namespace Nintendo.MakeRom
{
	internal class AccessControlInfoBase : WritableBinaryRegistory
	{
		protected readonly ARM11SystemLocalCapabilities m_ARM11SystemLocalCapabilities;
		protected readonly ARM11KernelCapabilities m_ARM11KernelCapabilities;
		protected readonly ARM9AccessControlInfo m_ARM9AccessControlInfo;
		public AccessControlInfoBase(ARM11SystemLocalCapabilities sysLocalCap, ARM11KernelCapabilities kernelCap, ARM9AccessControlInfo arm9Cont)
		{
			this.m_ARM11SystemLocalCapabilities = sysLocalCap;
			this.m_ARM11KernelCapabilities = kernelCap;
			this.m_ARM9AccessControlInfo = arm9Cont;
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_ARM11SystemLocalCapabilities,
				this.m_ARM11KernelCapabilities,
				this.m_ARM9AccessControlInfo
			});
		}
	}
}
