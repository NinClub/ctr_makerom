using System;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct StorageInfoStruct
	{
		public ulong ExtSaveDataId;
		public ulong SystemSaveDataId;
		public ulong StorageAccessableUniqueIds;
		public StorageInfoFlags InfoFlags;
	}
}
