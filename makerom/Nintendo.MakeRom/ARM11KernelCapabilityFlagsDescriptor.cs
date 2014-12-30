using System;
namespace Nintendo.MakeRom
{
	internal class ARM11KernelCapabilityFlagsDescriptor : ARM11KernelCapabilityDescriptor
	{
		public ARM11KernelCapabilityFlagsDescriptor(ARM11KernelCapabilityFlag flag) : base(9, 510u)
		{
			base.Data = (uint)flag;
		}
	}
}
