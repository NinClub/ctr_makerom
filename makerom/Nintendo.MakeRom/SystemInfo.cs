using System;
namespace Nintendo.MakeRom
{
	internal class SystemInfo : StructData<SystemInfoStruct>
	{
		public SystemInfo(MakeCxiOptions options)
		{
			this.Struct.SaveDataSize = options.SaveDataSize;
			this.Struct.JumpId = options.JumpId;
		}
	}
}
