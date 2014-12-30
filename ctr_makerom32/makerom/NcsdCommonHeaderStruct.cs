using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace makerom
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	unsafe internal struct NcsdCommonHeaderStruct
	{
		public uint Signature;
		public uint MediaSize;
		public ulong MediaId;
		public fixed byte PartitionFsType[8];
		public fixed byte PartitionCryptType[8];
		public fixed uint ParitionOffsetAndSize[16];
		public fixed byte ExtendedHeaderHash[32];
		public uint AdditionalHeaderSize;
		public uint SectorZeroOffset;
		public fixed byte Flags[8];
		public fixed ulong PartitionId[8];
		public fixed byte Reserved[48];
	}
}
