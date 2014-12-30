using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	unsafe internal struct NcchCommonHeaderStruct
	{
		public uint Signature;
		public uint ContentSize;
		public ulong PartitionId;
		public ushort MakerCode;
		public ushort NcchVersion;
		public fixed byte Reserved0[4];
		public ulong ProgramId;
		public byte TempFlag;
		public fixed byte Reserved1[47];
		public fixed byte ProductCode[16];
		public fixed byte ExtendedHeaderHash[32];
		public uint ExtendedHeaderSize;
		public fixed byte Reserved2[4];
		public fixed byte Flags[8];
		public uint PlainRegionOffset;
		public uint PlainRegionSize;
		public fixed byte Reserved3[8];
		public uint ExeFsOffset;
		public uint ExeFsSize;
		public uint ExeFsHashRegionSize;
		public fixed byte Reserved4[4];
		public uint RomFsOffset;
		public uint RomFsSize;
		public uint RomFsHashRegionSize;
		public fixed byte Reserved5[4];
		public fixed byte ExeFsSuperBlockHash[32];
		public fixed byte RomFsSuperBlockHash[32];
	}
}
