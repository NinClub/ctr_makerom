using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
namespace Nintendo.MakeRom
{
	internal class NcchHeader : WritableBinaryRegistory
	{
		private ByteArrayData m_HeaderRsaSignature = new ByteArrayData(256u);
		private NcchCommonHeader m_Header = new NcchCommonHeader();
		protected NcchExtendedHeader m_ExtendedHeader;
		protected NcchAccessControlExtended m_AccessControlExtended;
		private NcchCommonHeaderFlag m_HeaderFlag = new NcchCommonHeaderFlag();
		private RSAParameters m_CommonHeaderKeyParams;
		private bool m_IsCfa;
		public UInt64Data ProgramId
		{
			get
			{
				return this.m_Header.Struct.ProgramId;
			}
			set
			{
				this.m_Header.Struct.ProgramId = value;
			}
		}
		public NcchCommonHeader CommonHeader
		{
			get
			{
				return this.m_Header;
			}
		}
		public uint RomFsOffset
		{
			get
			{
				return this.m_Header.Struct.RomFsOffset;
			}
			set
			{
				this.m_Header.Struct.RomFsOffset = value;
			}
		}
		public uint RomFsSize
		{
			get
			{
				return this.m_Header.Struct.RomFsSize;
			}
			set
			{
				this.m_Header.Struct.RomFsSize = value;
			}
		}
		public uint ExeFsOffset
		{
			get
			{
				return this.m_Header.Struct.ExeFsOffset;
			}
			set
			{
				this.m_Header.Struct.ExeFsOffset = value;
			}
		}
		public uint ExeFsSize
		{
			get
			{
				return this.m_Header.Struct.ExeFsSize;
			}
			set
			{
				this.m_Header.Struct.ExeFsSize = value;
			}
		}
		public unsafe byte[] ExtendedHeaderHash
		{
			set
			{
				fixed (byte* ptr = this.m_Header.Struct.ExtendedHeaderHash)
				{
					IntPtr destination = new IntPtr((void*)ptr);
					Marshal.Copy(value, 0, destination, value.Length);
				}
			}
		}
		public unsafe byte[] RomFsSuperBlockHash
		{
			set
			{
				fixed (byte* ptr = this.m_Header.Struct.RomFsSuperBlockHash)
				{
					IntPtr destination = new IntPtr((void*)ptr);
					Marshal.Copy(value, 0, destination, value.Length);
				}
			}
		}
		public unsafe byte[] ExeFsSuperBlockHash
		{
			set
			{
				fixed (byte* ptr = this.m_Header.Struct.ExeFsSuperBlockHash)
				{
					IntPtr destination = new IntPtr((void*)ptr);
					Marshal.Copy(value, 0, destination, value.Length);
				}
			}
		}
		public byte CryptoType
		{
			get
			{
				return this.m_HeaderFlag.GetArray()[7];
			}
		}
		public uint PlainRegionOffset
		{
			set
			{
				this.m_Header.Struct.PlainRegionOffset = value;
			}
		}
		public uint PlainRegionSize
		{
			set
			{
				this.m_Header.Struct.PlainRegionSize = value;
			}
		}
		public uint RomFsHashRegionSize
		{
			set
			{
				this.m_Header.Struct.RomFsHashRegionSize = value;
			}
		}
		public uint ExeFsHashRegionSize
		{
			set
			{
				this.m_Header.Struct.ExeFsHashRegionSize = value;
			}
		}
		public uint ContentSize
		{
			set
			{
				this.m_Header.Struct.ContentSize = value;
			}
		}
		public ulong PartitionId
		{
			get
			{
				return this.m_Header.Struct.PartitionId;
			}
			set
			{
				this.m_Header.Struct.PartitionId = value;
			}
		}
		public ushort MakerCode
		{
			get
			{
				return this.m_Header.Struct.MakerCode;
			}
			set
			{
				this.m_Header.Struct.MakerCode = value;
			}
		}
		public unsafe string ProductCode
		{
			set
			{
				byte[] array = new byte[16];
				Encoding.ASCII.GetBytes(value, 0, value.Length, array, 0);
				fixed (byte* ptr = this.m_Header.Struct.ProductCode)
				{
					IntPtr destination = new IntPtr((void*)ptr);
					Marshal.Copy(array, 0, destination, 16);
				}
			}
		}
		public NcchHeader(NcchExtendedHeader exHeader, NcchAccessControlExtended acExtended, MakeCxiOptions options)
		{
			this.m_CommonHeaderKeyParams = options.NcchCommonHeaderKeyParams;
			this.m_ExtendedHeader = exHeader;
			this.m_AccessControlExtended = acExtended;
			this.m_IsCfa = options.IsCfa;
			this.m_Header.Struct.NcchVersion = options.NcchVersion;
			this.SetNcchFlags(options);
			this.ProductCode = options.ProductCode;
			this.ProgramId = TitleIdUtil.MakeProgramId(options);
			this.PartitionId = Util.MakePartitionId(options);
			this.MakerCode = options.CompanyCode;
			if (exHeader != null)
			{
				this.m_Header.Struct.ExtendedHeaderSize = (uint)exHeader.Size;
				this.ExtendedHeaderHash = exHeader.GetExtendedHeaderHash();
			}
			else
			{
				this.m_Header.Struct.ExtendedHeaderSize = 0u;
				this.ExtendedHeaderHash = new byte[32];
			}
			this.CheckSize();
		}
		private void SetNcchFlags(MakeCxiOptions options)
		{
			this.m_HeaderFlag.SetUnitSize(options.MediaUnitSize);
			NcchCommonHeaderFlag arg_1F_0 = this.m_HeaderFlag;
			NcchCommonHeaderFlag.ContentPlatform[] contentPlatform = new NcchCommonHeaderFlag.ContentPlatform[1];
			arg_1F_0.SetContentPlatform(contentPlatform);
			if (options.IsCfa)
			{
				this.m_HeaderFlag.SetContentType(NcchCommonHeaderFlag.FormType.SimpleContent, options.ContentType);
			}
			else
			{
				if (options.UseRomFs)
				{
					this.m_HeaderFlag.SetContentType(NcchCommonHeaderFlag.FormType.ExecutableContent, options.ContentType);
				}
				else
				{
					this.m_HeaderFlag.SetContentType(NcchCommonHeaderFlag.FormType.ExecutableContentWithoutRomFs, options.ContentType);
				}
			}
			if (!options.UseRomFs)
			{
				this.m_HeaderFlag.SetFlag(new NcchCommonHeaderFlag.Flag[]
				{
					NcchCommonHeaderFlag.Flag.NoMountRomFs
				});
			}
			if (!options.UseAes)
			{
				this.m_HeaderFlag.SetFlag(new NcchCommonHeaderFlag.Flag[]
				{
					NcchCommonHeaderFlag.Flag.NoEncrypto
				});
			}
			NcchCommonHeaderFlag arg_BA_0 = this.m_HeaderFlag;
			NcchCommonHeaderFlag.Flag[] flag = new NcchCommonHeaderFlag.Flag[1];
			arg_BA_0.SetFlag(flag);
			this.SetCommonFlags();
		}
		private unsafe void SetCommonFlags()
		{
			fixed (byte* ptr = this.m_Header.Struct.Flags)
			{
				IntPtr destination = new IntPtr((void*)ptr);
				Marshal.Copy(this.m_HeaderFlag.GetArray(), 0, destination, this.m_HeaderFlag.GetArray().Length);
			}
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			this.Update();
			base.WriteBinary(writer);
		}
		protected virtual void CheckSize()
		{
			bool arg_06_0 = this.m_IsCfa;
		}
		protected override void Update()
		{
			this.m_HeaderRsaSignature = this.m_Header.GetRsaSignature(this.m_CommonHeaderKeyParams);
			if (!this.m_IsCfa)
			{
				base.SetBinaries(new IWritableBinary[]
				{
					this.m_HeaderRsaSignature,
					this.m_Header,
					this.m_ExtendedHeader,
					this.m_AccessControlExtended
				});
				return;
			}
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_HeaderRsaSignature,
				this.m_Header
			});
		}
	}
}
