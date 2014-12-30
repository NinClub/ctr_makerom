using System;
namespace Nintendo.MakeRom
{
	internal class SystemCallAccessControlDescriptor : ARM11KernelCapabilityDescriptor
	{
		private const int SHIFT_INDEX = 24;
		private const uint MASK_FLAGS = 4278190080u;
		public SystemCallAccessControlDescriptor() : base(5, 30u)
		{
		}
		public SystemCallAccessControlDescriptor(uint index, uint flags) : this()
		{
			base.Data = (index << 24 | flags);
		}
	}
}
