using System;
namespace Nintendo.MakeRom
{
	internal class NcchCommonHeaderFlag
	{
		public enum ContentPlatform
		{
			CTR
		}
		public enum Index
		{
			ContentPlatform = 4,
			ContentType,
			SizeType,
			Flag
		}
		public enum Flag
		{
			FixedCryptoKey,
			NoMountRomFs,
			NoEncrypto
		}
		public enum SizeType
		{
			Byte
		}
		public enum FormType
		{
			NotAssign,
			SimpleContent,
			ExecutableContentWithoutRomFs,
			ExecutableContent
		}
		public enum ContentType
		{
			Application,
			SystemUpdate,
			Manual,
			Child,
			Trial
		}
		private byte[] m_Flags;
		public NcchCommonHeaderFlag()
		{
			this.m_Flags = new byte[8];
		}
		public void SetFlag(params NcchCommonHeaderFlag.Flag[] flags)
		{
			byte b = this.m_Flags[7];
			for (int i = 0; i < flags.Length; i++)
			{
				NcchCommonHeaderFlag.Flag index = flags[i];
				b = this.SetBit(b, (int)index);
			}
			this.m_Flags[7] = b;
		}
		public void SetContentType(NcchCommonHeaderFlag.FormType formType, NcchCommonHeaderFlag.ContentType contentType)
		{
			byte b = (byte)(formType | (NcchCommonHeaderFlag.FormType)((int)contentType << 2));
			this.m_Flags[5] = b;
		}
		public void SetContentPlatform(params NcchCommonHeaderFlag.ContentPlatform[] types)
		{
			byte b = this.m_Flags[4];
			for (int i = 0; i < types.Length; i++)
			{
				NcchCommonHeaderFlag.ContentPlatform index = types[i];
				b = this.SetBit(b, (int)index);
			}
			this.m_Flags[4] = b;
		}
		private byte SetBit(byte data, int index)
		{
			return (byte)(data | (byte)(1 << index));
		}
		private byte ClearBit(byte data, int index)
		{
			return (byte)(data & ~(byte)(1 << index));
		}
		public void SetUnitSize(byte unitSizeLog)
		{
			this.m_Flags[6] = unitSizeLog;
		}
		public byte[] GetArray()
		{
			return this.m_Flags;
		}
	}
}
