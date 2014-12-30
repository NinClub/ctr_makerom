using System;
namespace Nintendo.MakeRom
{
	internal class AccessControlInfo : AccessControlInfoBase
	{
		public AccessControlInfo(ARM11SystemLocalCapabilities sysLocalCap, ARM11KernelCapabilities kernelCap, ARM9AccessControlInfo arm9Cont) : base(sysLocalCap, kernelCap, arm9Cont)
		{
		}
	}
}
