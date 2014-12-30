using Nintendo.MakeRom.Properties;
using System;
using System.IO;
using System.Linq;
namespace Nintendo.MakeRom
{
	public class MakeCxi
	{
		private static byte[] s_SystemFixedKey;
		public static byte[] SystemFixedKey
		{
			get
			{
				return MakeCxi.s_SystemFixedKey;
			}
		}
		public static NcchBinary Make(CxiOption cxiOption)
		{
			MakeCxiOptions makeCxiOptions = new MakeCxiOptions(cxiOption);
			MakeCxi.SetSystemFixedKey(makeCxiOptions);
			return new NcchBinaryCore(makeCxiOptions);
		}
		public static NcchFileBinary Load(string filename)
		{
			return new NcchFileBinary(filename);
		}
		public static NcchFileBinary LoadFromStream(Stream reader)
		{
			return new NcchFileBinary(reader);
		}
		private static void SetSystemFixedKey(MakeCxiOptions options)
		{
			byte[] aesKey = options.AesKey;
			if (!aesKey.SequenceEqual(Resources.key))
			{
				MakeCxi.s_SystemFixedKey = aesKey;
			}
		}
		public static void SetSystemFixedKey(byte[] aesKey)
		{
			MakeCxi.s_SystemFixedKey = aesKey;
		}
		public static RomfsOnlyNcchBinary MakeRomfsOnlyNcchBinary(CxiOption option)
		{
			return new RomfsOnlyNcchBinary(option);
		}
	}
}
