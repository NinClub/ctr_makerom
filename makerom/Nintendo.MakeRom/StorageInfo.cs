using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	internal class StorageInfo : StructData<StorageInfoStruct>
	{
		public enum FileSystemAccess
		{
			CATEGORY_SYSTEM_APPLICATION,
			CATEGORY_HARDWARE_CHECK,
			CATEGORY_FILE_SYSTEM_TOOL,
			DEBUG,
			TWL_CARD_BACKUP,
			TWL_NAND_DATA,
			BOSS,
			DIRECT_SDMC,
			CORE,
			CTR_NAND_RO,
			CTR_NAND_RW,
			CTR_NAND_RO_WRITE,
			CATEGORY_SYSTEM_SETTINGS,
			CARD_BOARD,
			EXPORT_IMPORT_IVS,
			DIRECT_SDMC_WRITE,
			SWITCH_CLEANUP
		}
		private enum AttributeName
		{
			NOT_USE_ROMFS
		}
		public StorageInfo(MakeCxiOptions options)
		{
			this.Struct.ExtSaveDataId = options.ExtSaveDataId;
			this.Struct.StorageAccessableUniqueIds = options.StorageAccessableUniqueIds;
			this.Struct.SystemSaveDataId = options.SystemSaveDataId;
			this.SetAttributes(StorageInfo.AttributeName.NOT_USE_ROMFS, !options.UseRomFs);
			this.SetFileSystemAccess(options.FileSystemAccess);
		}
		private void SetFileSystemAccess(IEnumerable<StorageInfo.FileSystemAccess> list)
		{
			using (IEnumerator<StorageInfo.FileSystemAccess> enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = (int)enumerator.Current;
					this.Struct.InfoFlags.SetStorageAccessInfoBit(current, true);
				}
			}
		}
		private void SetAttributes(StorageInfo.AttributeName name, bool value)
		{
			if (value)
			{
				this.Struct.InfoFlags.OtherAttributes = (byte)(this.Struct.InfoFlags.OtherAttributes | (byte)(1 << (int)name));
				return;
			}
			this.Struct.InfoFlags.OtherAttributes = (byte)(this.Struct.InfoFlags.OtherAttributes & (byte)(~(byte)(1 << (int)name)));
		}
	}
}
