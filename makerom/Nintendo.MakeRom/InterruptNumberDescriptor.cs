using System;
namespace Nintendo.MakeRom
{
	internal class InterruptNumberDescriptor : ARM11KernelCapabilityDescriptor
	{
		public InterruptNumberDescriptor() : base(4, 14u)
		{
		}
		public InterruptNumberDescriptor(string[] intNums) : this()
		{
			base.Data |= 268435455u;
			for (int i = 0; i < intNums.Length; i++)
			{
				uint num = (uint)(127L << ((3 - i) * 7 & 31) ^ 0xffffffffL);
				base.Data &= num;
				base.Data |= (uint)((uint)sbyte.Parse(intNums[i]) << (3 - i) * 7);
			}
		}
	}
}
