using System;
namespace Nintendo.MakeRom
{
	internal class OtherCapabilities : BinaryArray<ARM11KernelCapabilityDescriptor>
	{
		private const int NUM_DESCRIPTORS = 1;
		public OtherCapabilities(MakeCxiOptions options) : base(1)
		{
			base.Array[0] = new OtherCapabilityDescriptor(options);
		}
	}
}
