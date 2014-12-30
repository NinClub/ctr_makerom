using Nintendo.MakeRom;
using System;
using System.IO;
namespace makerom
{
	internal class Ncsd : WritableBinaryRegistory
	{
		internal const int MAX_NCCH_NUM = 8;
		protected ByteArrayData m_Padding;
		private ByteArrayData m_FootPadding = new byte[0];
		protected NcsdHeader m_Header;
		protected CardInfo m_CardInfo;
		protected NcchBinary[] m_NcchArray = new NcchBinary[8];
		private bool m_RequrieFootPadding;
		private byte m_PaddingChar;
		private byte m_MediaUnitSize;
		private uint m_UnusedSize;
		public CciInfo CciInfo
		{
			get;
			set;
		}
		public Ncsd(MakeCciOptions options)
		{
			this.CciInfo = new CciInfo();
			this.m_Header = new NcsdHeader();
			this.m_CardInfo = new CardInfo(options);
			this.m_MediaUnitSize = options.MediaUnitSize;
			this.m_Padding = new byte[16384L - this.m_Header.Size - this.m_CardInfo.Size];
			this.m_Header.CommonHeader.MediaSize = options.GetMediaSize();
			this.m_Header.SetMediaType(options.NcsdMediaType);
			this.m_Header.SetMediaPlatforms(options.NcsdMediaPlatforms);
			this.m_Header.SetMediaUnitSize(this.m_MediaUnitSize);
			this.m_Header.SetCardDevice(options.CardDevice);
			this.m_RequrieFootPadding = options.MediaFootPadding;
			this.m_PaddingChar = options.Padding;
			this.m_UnusedSize = options.GetUnusedSize();
		}
		public void AddNcch(NcchBinary ncch, int partitionIndex)
		{
			if (partitionIndex >= 8)
			{
				throw new ArgumentException("Too large partition index: " + partitionIndex);
			}
			if (this.m_NcchArray[partitionIndex] != null)
			{
				throw new ArgumentException(string.Format("Failed set cxi. Partition {0} is not empty.", partitionIndex));
			}
			this.m_NcchArray[partitionIndex] = ncch;
			this.CciInfo.Contents.Add(ncch.CxiInfo);
		}
		private long CalcFootPaddingSize()
		{
			if (!this.m_RequrieFootPadding)
			{
				return 0L;
			}
			long size = this.Size;
			long num = Util.MediaUnitToByte(this.m_Header.CommonHeader.MediaSize, this.m_MediaUnitSize);
			if (num < size)
			{
				throw new MakeromException(string.Format("Too large cci size.\nCCI: {0}\nMediaSize: {1}\n", size, num));
			}
			return num - size;
		}
		private void CheckNcsdSize()
		{
			this.Update();
			long num = Util.MediaUnitToByte(this.m_Header.CommonHeader.MediaSize, this.m_MediaUnitSize);
			long num2 = num - (long)((ulong)this.m_UnusedSize);
			if (this.Size > num)
			{
				throw new MakeromException(string.Format("Too large rom size.\nyour cci: {0}\nlimit   : {1}\n", this.Size, num));
			}
			if (this.Size > num2)
			{
				Nintendo.MakeRom.Util.PrintWarning(string.Format("ROM IMAGE size ({0} byte) exceeds media size ({1} bytes)", this.Size, num2));
			}
		}
		private void UpdateNcsdHeader()
		{
			for (int i = 0; i < 8; i++)
			{
				if (i == 0 && this.m_NcchArray[i] == null)
				{
					throw new ArgumentException("Not found cxi on partition 0");
				}
				if (this.m_NcchArray[i] != null)
				{
					this.m_Header.SetPartitionInfo(i, NcsdHeader.PartitionFsType.FS_TYPE_DEFAULT, NcsdHeader.PartitionEncryptoType.ENCRYPTO_TYPE_DEFAULT, this.m_NcchArray[i].GetPartitionId(), this.m_NcchArray[i].Size, this.m_MediaUnitSize);
				}
			}
		}
		protected void UpdatePartitionIds(ulong programId)
		{
			for (int i = 0; i < 8; i++)
			{
				if (this.m_NcchArray[i] != null)
				{
					this.m_NcchArray[i].SetProgramId(programId);
					this.m_NcchArray[i].SetPartitionId((programId & 281474976710655uL) | (ulong)((ulong)((long)(4 + i)) << 48));
				}
			}
		}
		protected override void Update()
		{
			base.ClearBinaries();
			this.UpdatePartitionIds(this.m_NcchArray[0].GetProgramId());
			this.m_Header.CommonHeader.Struct.MediaId = this.m_NcchArray[0].GetPartitionId();
			this.m_CardInfo.MediaId = this.m_NcchArray[0].GetPartitionId();
			this.m_CardInfo.NcchHeader = this.m_NcchArray[0].GetCommonHeader();
			this.UpdateNcsdHeader();
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_Header,
				this.m_CardInfo,
				this.m_Padding
			});
			for (int i = 0; i < 8; i++)
			{
				if (this.m_NcchArray[i] != null)
				{
					base.AddBinaries(new IWritableBinary[]
					{
						this.m_NcchArray[i]
					});
				}
			}
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_FootPadding
			});
		}
		private void WritePaddingData(BinaryWriter writer, long size, byte paddingChar)
		{
			byte[] buffer = Util.MakePaddingData(1048576u, paddingChar);
			while (size > 0L)
			{
				uint num = (size > 1048576L) ? 1048576u : ((uint)size);
				writer.Write(buffer, 0, (int)num);
				size -= (long)((ulong)num);
			}
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			this.CheckNcsdSize();
			base.WriteBinary(writer);
			if (this.m_RequrieFootPadding)
			{
				long size = this.CalcFootPaddingSize();
				this.WritePaddingData(writer, size, 255);
			}
		}
	}
}
