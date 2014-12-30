using System;
namespace Nintendo.MakeRom
{
	internal class ServiceAccessControl : UInt64NameArray
	{
		private const int NUM_SERVICE_NAMES = 32;
		public ServiceAccessControl(string[] serviceList) : base(32)
		{
			if (serviceList.Length > 32)
			{
				throw new MakeromException("Too many ServiceAccesses");
			}
			if (serviceList != null)
			{
				for (int i = 0; i < serviceList.Length; i++)
				{
					string programIdDesc = serviceList[i];
					base.AddName(new UInt64Name(programIdDesc));
				}
			}
		}
	}
}
