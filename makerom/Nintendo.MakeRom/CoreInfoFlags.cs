using System;
namespace Nintendo.MakeRom
{
	internal class CoreInfoFlags : ByteArrayData
	{
		protected override int ByteSize
		{
			get
			{
				return 8;
			}
		}
	}
}
