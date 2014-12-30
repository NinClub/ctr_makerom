using System;
namespace Nintendo.MakeRom
{
	internal class HandleTableSize : BinaryArray<ARM11KernelCapabilityDescriptor>
	{
		private const int NUM_DESCRIPTORS = 1;
		public HandleTableSize(MakeCxiOptions options) : base(1)
		{
			base.Array[0] = new HandleTableSizeDescriptor(options);
		}
	}
}
