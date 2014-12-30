using System;
namespace Nintendo.MakeRom
{
	internal class ReleaseKernelVersion : BinaryArray<ARM11KernelCapabilityDescriptor>
	{
		private const int NUM_DESCRIPTORS = 1;
		public ReleaseKernelVersion(MakeCxiOptions options) : base(1)
		{
			try
			{
				base.Array[0] = new ReleaseKernelVersionDescriptor(options);
			}
			catch (MakeromException)
			{
				base.Array[0] = new ARM11KernelCapabilityDescriptor();
			}
		}
	}
}
