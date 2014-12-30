using System;
namespace Nintendo.MakeRom
{
	internal class Region : ByteArrayData
	{
		protected override int ByteSize
		{
			get
			{
				return 4;
			}
		}
	}
}
