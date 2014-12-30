using System;
namespace Nintendo.MakeRom
{
	internal abstract class MappingDescriptor : ARM11KernelCapabilityDescriptor
	{
		private const int ADDRESS_SHIFT = 12;
		protected MappingDescriptor(uint address, uint prefixVal, int prefixLength, bool flag) : base(prefixLength, prefixVal)
		{
			base.Data = ((address >> 12 & ~base.PrefixMask) | base.PrefixBits);
			if (flag)
			{
				base.Data |= 1048576u;
			}
		}
	}
}
