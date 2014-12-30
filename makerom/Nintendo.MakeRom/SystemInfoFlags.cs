using System;
namespace Nintendo.MakeRom
{
	internal class SystemInfoFlags : StructData<SystemInfoFlagStruct>
	{
		public SystemInfoFlags(MakeCxiOptions options)
		{
			this.Struct.remasterVersion = options.RemasterVersion;
			this.SetFlags(options);
		}
		private void SetFlags(MakeCxiOptions options)
		{
			this.Struct.flag = (byte)(this.Struct.flag | (options.Compressed ? 1 : 0));
		}
	}
}
