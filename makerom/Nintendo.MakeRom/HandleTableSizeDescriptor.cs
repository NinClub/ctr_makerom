using System;
namespace Nintendo.MakeRom
{
	internal class HandleTableSizeDescriptor : ARM11KernelCapabilityDescriptor
	{
		private const int PREFIX_VALUE = 254;
		private const int PREFIX_BITS_NUM = 8;
		private const int SIZE_BITS_NUM = 10;
		private const int SIZE_BITS_MASK = 1023;
		public HandleTableSizeDescriptor(MakeCxiOptions options) : base(8, 254u)
		{
			uint handleTableSize = options.HandleTableSize;
			if ((handleTableSize & 1023u) != handleTableSize)
			{
				throw new MakeromException("Too large handle table size");
			}
			base.Data = handleTableSize;
		}
	}
}
