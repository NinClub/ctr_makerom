using System;
namespace Nintendo.MakeRom
{
	internal class ParentalControlInfo : ByteArrayData
	{
		protected override int ByteSize
		{
			get
			{
				return 16;
			}
		}
	}
}
