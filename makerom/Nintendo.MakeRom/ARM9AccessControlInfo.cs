using System;
using System.Collections.Generic;
using System.Linq;
namespace Nintendo.MakeRom
{
	internal class ARM9AccessControlInfo : ByteArrayData
	{
		public enum Arm9Capability
		{
			FS_MOUNT_NAND,
			FS_MOUNT_NAND_RO_WRITE,
			FS_MOUNT_TWLN,
			FS_MOUNT_WNAND,
			FS_MOUNT_CARD_SPI,
			USE_SDIF3,
			CREATE_SEED,
			USE_CARD_SPI,
			SD_APPLICATION,
			USE_DIRECT_SDMC
		}
		protected override int ByteSize
		{
			get
			{
				return 16;
			}
		}
		private void SetArm9Flag(ARM9AccessControlInfo.Arm9Capability capability)
		{
			int num = (int)capability / (int)ARM9AccessControlInfo.Arm9Capability.SD_APPLICATION;
			int num2 = (int)capability % (int)ARM9AccessControlInfo.Arm9Capability.SD_APPLICATION;
			byte[] expr_16_cp_0 = base.Data;
			int expr_16_cp_1 = num;
			expr_16_cp_0[expr_16_cp_1] |= (byte)(1 << num2);
		}
		public ARM9AccessControlInfo(MakeCxiOptions options)
		{
			base.Data[15] = options.DescVersion;
			if (options.FileSystemAccess.Contains(StorageInfo.FileSystemAccess.DIRECT_SDMC) || options.FileSystemAccess.Contains(StorageInfo.FileSystemAccess.DIRECT_SDMC_WRITE))
			{
				this.SetArm9Flag(ARM9AccessControlInfo.Arm9Capability.USE_DIRECT_SDMC);
			}
			this.SetArm9AccessControl(options);
			ARM9AccessControlInfoDesc aRM9AccessControlInfoDesc = new ARM9AccessControlInfoDesc(options.AccControlDescBin);
			int i = 0;
			while (i < base.Data.Length)
			{
				if ((base.Data[i] | aRM9AccessControlInfoDesc.Capability[i]) != aRM9AccessControlInfoDesc.Capability[i])
				{
					if (i == 1 && (base.Data[i] & 2) == 2 && (aRM9AccessControlInfoDesc.Capability[i] & 2) == 0)
					{
						throw new MakeromException(string.Format("DirectSdmc is not allowed.\n", i, aRM9AccessControlInfoDesc.Capability[i], base.Data[i]));
					}
					throw new MakeromException(string.Format("Not allowed IoAccessControl: desc[{0}] = {1:x2}, rsf[{0}] = {2:x2}\n", i, aRM9AccessControlInfoDesc.Capability[i], base.Data[i]));
				}
				else
				{
					i++;
				}
			}
		}
		private void SetArm9AccessControl(MakeCxiOptions options)
		{
			IEnumerable<ARM9AccessControlInfo.Arm9Capability> arm9AccessControl = options.Arm9AccessControl;
			if (arm9AccessControl == null)
			{
				return;
			}
			foreach (ARM9AccessControlInfo.Arm9Capability current in arm9AccessControl)
			{
				this.SetArm9Flag(current);
			}
		}
	}
}
