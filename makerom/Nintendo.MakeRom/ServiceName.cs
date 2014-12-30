using System;
using System.Text;
namespace Nintendo.MakeRom
{
	internal class ServiceName : ByteArrayData
	{
		protected override int ByteSize
		{
			get
			{
				return 8;
			}
		}
		public ServiceName()
		{
		}
		public bool IsEmpty()
		{
			byte[] data = base.Data;
			for (int i = 0; i < data.Length; i++)
			{
				byte b = data[i];
				if (b != 0)
				{
					return false;
				}
			}
			return true;
		}
		public ServiceName(string name)
		{
			Encoding.ASCII.GetBytes(name).CopyTo(base.Data, 0);
		}
	}
}
