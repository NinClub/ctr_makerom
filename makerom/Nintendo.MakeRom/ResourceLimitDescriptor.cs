using System;
namespace Nintendo.MakeRom
{
	internal class ResourceLimitDescriptor : ByteArrayData
	{
		protected override int ByteSize
		{
			get
			{
				return 2;
			}
		}
	}
}
