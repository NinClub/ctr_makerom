using Nintendo.MakeRom;
using System;
namespace makerom
{
	internal class NcsdHeader : WritableBinaryRegistory
	{
		public enum PartitionFsType
		{
			FS_TYPE_DEFAULT
		}
		public enum PartitionEncryptoType
		{
			ENCRYPTO_TYPE_DEFAULT
		}
		public enum MediaPlatform
		{
			CTR
		}
		public enum MediaType
		{
			INNER_DEVICE,
			CARD1,
			CARD2,
			EXTENDED_DEVICE
		}
		private enum FlagIndex
		{
			MEDIA_CARD_DEVICE = 3,
			MEDIA_PLATFORM_INDEX,
			MEDIA_TYPE_INDEX,
			MEDIA_UNIT_SIZE
		}
		private const int NUM_MAX_PARTITION = 8;
		private const uint OFFSET_FIRST_NCCH = 16384u;
		private ByteArrayData m_RsaSignature;
		private uint[] m_NcchSize = new uint[8];
		public NcsdCommonHeader CommonHeader
		{
			get;
			private set;
		}
		public NcsdHeader()
		{
			this.CommonHeader = new NcsdCommonHeader();
			this.m_RsaSignature = this.CommonHeader.GetRsaSignature();
		}
		public unsafe void SetCardDevice(NyamlOption.CardDeviceName cardDevice)
		{
			fixed (byte* ptr = this.CommonHeader.Struct.Flags)
			{
				byte b = (byte)cardDevice;
				ptr[3] = b;
			}
		}
		public unsafe void SetMediaPlatforms(NcsdHeader.MediaPlatform[] platforms)
		{
			if (platforms == null)
			{
				return;
			}
			fixed (byte* ptr = this.CommonHeader.Struct.Flags)
			{
				byte b = 0;
				for (int i = 0; i < platforms.Length; i++)
				{
					b |= (byte)(1 << (int)((byte)platforms[i]));
				}
				ptr[4] = b;
			}
		}
		public unsafe void SetMediaUnitSize(byte unitSize)
		{
			fixed (byte* ptr = this.CommonHeader.Struct.Flags)
			{
				ptr[6] = unitSize;
			}
		}
		public unsafe void SetMediaType(NcsdHeader.MediaType mediaType)
		{
			fixed (byte* ptr = this.CommonHeader.Struct.Flags)
			{
				ptr[5] = (byte)mediaType;
			}
		}
		public unsafe void SetPartitionInfo(int index, NcsdHeader.PartitionFsType fsType, NcsdHeader.PartitionEncryptoType encType, ulong partitionId, long ncchSize, byte mediaUnitSize)
		{
			uint num = Nintendo.MakeRom.Util.Int64ToMediaUnitSize(ncchSize, mediaUnitSize);
			this.m_NcchSize[index] = num;
			fixed (byte* ptr = this.CommonHeader.Struct.PartitionFsType)
			{
				ptr[index] = (byte)fsType;
			}
			fixed (byte* ptr2 = this.CommonHeader.Struct.PartitionCryptType)
			{
				ptr2[index] = (byte)encType;
			}
			fixed (ulong* ptr3 = this.CommonHeader.Struct.PartitionId)
			{
				ptr3[index] = partitionId;
			}
		}
		private unsafe void UpdatePartitionAddressAndSize()
		{
			uint num;
			fixed (byte* ptr = this.CommonHeader.Struct.Flags)
			{
				num = Nintendo.MakeRom.Util.Int64ToMediaUnitSize(16384L, ptr[6]);
			}
			fixed (uint* ptr2 = this.CommonHeader.Struct.ParitionOffsetAndSize)
			{
				for (int i = 0; i < 8; i++)
				{
					if (this.m_NcchSize[i] != 0u)
					{
						ptr2[i * 2] = num;
						ptr2[i * 2 + 1] = this.m_NcchSize[i];
						num += this.m_NcchSize[i];
					}
				}
			}
		}
		protected override void Update()
		{
			this.UpdatePartitionAddressAndSize();
			this.m_RsaSignature = this.CommonHeader.GetRsaSignature();
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_RsaSignature,
				this.CommonHeader
			});
		}
	}
}
