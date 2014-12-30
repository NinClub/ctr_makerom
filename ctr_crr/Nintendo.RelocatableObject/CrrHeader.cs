using System;
using System.Runtime.InteropServices;
namespace Nintendo.RelocatableObject
{
	[StructLayout(LayoutKind.Sequential)]
	public class CrrHeader
	{
		public uint signature;
		private uint reserved0;
		private uint node0;
		private uint node1;
		public int debugInfoOffset;
		public int debugInfoSize;
		private uint reserved3;
		private uint reserved4;
		public uint uniqueIdMask;
		public uint uniqueIdPattern;
		private uint reserved5;
		private uint reserved6;
		private uint reserved7;
		private uint reserved8;
		private uint reserved9;
		private uint reserved10;
		public RsaData signPublicKey;
		public RsaData signPublicKeySign;
		public RsaData sign;
		public uint uniqueId;
		public uint size;
		private uint reserved11;
		private uint reserved12;
		public uint hashOffset;
		public uint numHash;
		public uint moduleIdOffset;
		public uint moduleIdSize;
		public CrrHeader()
		{
			this.signPublicKey = new RsaData();
			this.signPublicKeySign = new RsaData();
			this.sign = new RsaData();
		}
	}
}
