using System;
namespace Nintendo.MakeRom
{
	internal class ReleaseKernelVersionDescriptor : ARM11KernelCapabilityDescriptor
	{
		private const int PREFIX_VALUE = 126;
		private const int PREFIX_BITS_NUM = 7;
		public ReleaseKernelVersionDescriptor(MakeCxiOptions options) : base(7, 126u)
		{
			uint releaseKernelMajor = options.ReleaseKernelMajor;
			uint releaseKernelMinor = options.ReleaseKernelMinor;
			if (releaseKernelMajor > 255u || releaseKernelMinor > 255u)
			{
				throw new MakeromException("Invalid release kernel version");
			}
			base.Data = (releaseKernelMajor << 8 | releaseKernelMinor);
		}
	}
}
