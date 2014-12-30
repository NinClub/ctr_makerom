using System;
namespace Nintendo.MakeRom
{
	internal class ARM9AccessControlInfoDesc
	{
		private const int ARM9_CAPABILITY_OFFSET = 496;
		private const int ARM9_LEN = 16;
		private byte[] m_Arm9Capability = new byte[16];
		public byte[] Capability
		{
			get
			{
				return this.m_Arm9Capability;
			}
		}
		public ARM9AccessControlInfoDesc(byte[] desc)
		{
			Array.Copy(desc, 496, this.m_Arm9Capability, 0, 16);
		}
	}
}
