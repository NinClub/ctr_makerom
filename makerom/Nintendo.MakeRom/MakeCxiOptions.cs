using Nintendo.MakeRom.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	internal class MakeCxiOptions
	{
		public enum ProcessTypeName
		{
			SYSTEM,
			APPLICATION,
			DEFAULT = -1
		}
		public enum MemoryTypeName
		{
			APPLICATION = 1,
			SYSTEM,
			BASE
		}
		public enum TitleCategoryName
		{
			APPLICATION,
			FIRMWARE,
			DLP_CHILD,
			ADDITIONAL_CONTENTS,
			TRIAL,
			SYSTEM1,
			SYSTEM2
		}
		public enum TitlePlatformName
		{
			CTR
		}
		public enum TitleUse
		{
			APPLICATION,
			SYSTEM,
			EVALUATION
		}
		public enum LogoName
		{
			NONE,
			NINTENDO,
			LICENSED,
			PUBLISHED,
			DISTRIBUTED,
			IQUE,
			IQUEFORSYSTEM
		}
		public enum ResourceLimitCategoryName
		{
			APPLICATION,
			SYS_APPLET,
			LIB_APPLET,
			OTHER
		}
		private const sbyte USER_THREAD_PRIORITY_BASE = 32;
		internal const uint RELEASE_KERNEL_VERSION_NONE = 4294967295u;
		public string SourcePath
		{
			get;
			private set;
		}
		public FileStream Source
		{
			get;
			private set;
		}
		public string BannerPath
		{
			get;
			private set;
		}
		public string IconPath
		{
			get;
			private set;
		}
		public string LogoPath
		{
			get;
			private set;
		}
		public string ObjectName
		{
			get;
			private set;
		}
		public AddressMapping Mapping
		{
			get;
			private set;
		}
		public DependencyList DependencyList
		{
			get;
			private set;
		}
		public ServiceAccessControl ServiceAccessControl
		{
			get;
			private set;
		}
		public UInt64Name Name
		{
			get;
			private set;
		}
		public UInt32Data MainThreadStackSize
		{
			get;
			private set;
		}
		public SByteData MainThreadPriority
		{
			get;
			private set;
		}
		public ulong SystemSaveDataId
		{
			get;
			private set;
		}
		public ulong StorageAccessableUniqueIds
		{
			get;
			private set;
		}
		public IEnumerable<StorageInfo.FileSystemAccess> FileSystemAccess
		{
			get;
			private set;
		}
		public IEnumerable<ARM9AccessControlInfo.Arm9Capability> Arm9AccessControl
		{
			get;
			private set;
		}
		public AffinityMask AffinityMask
		{
			get;
			private set;
		}
		public InterruptNumberList InterruptNumberList
		{
			get;
			private set;
		}
		public SByteData IdealProcessor
		{
			get;
			private set;
		}
		public bool AllowsUnalignedSection
		{
			get;
			private set;
		}
		public SystemCallAccessControl SystemCallAccessControl
		{
			get;
			private set;
		}
		public MakeCxiOptions.ProcessTypeName ProcessType
		{
			get;
			private set;
		}
		public bool PermitDebug
		{
			get;
			private set;
		}
		public bool ForceDebug
		{
			get;
			private set;
		}
		public bool CanUseNonAlphabetAndNumber
		{
			get;
			private set;
		}
		public bool CanWriteSharedPage
		{
			get;
			private set;
		}
		public bool CanUsePrivilegePriority
		{
			get;
			private set;
		}
		public bool PermitMainFunctionArgument
		{
			get;
			private set;
		}
		public bool CanShareDeviceMemory
		{
			get;
			private set;
		}
		public bool RunnableOnSleep
		{
			get;
			private set;
		}
		public bool SpecialMemoryArrange
		{
			get;
			private set;
		}
		public bool IsCip
		{
			get;
			private set;
		}
		public bool IsCfa
		{
			get;
			private set;
		}
		public bool IsCaa
		{
			get;
			private set;
		}
		public bool MakeCci2
		{
			get;
			private set;
		}
		public uint PageSize
		{
			get;
			private set;
		}
		public byte[] AccControlDescRsaSign
		{
			get;
			private set;
		}
		public byte[] AccControlDescBin
		{
			get;
			private set;
		}
		public ushort RemasterVersion
		{
			get;
			private set;
		}
		public ushort NcchVersion
		{
			get;
			private set;
		}
		public RSAParameters NcchCommonHeaderKeyParams
		{
			get;
			private set;
		}
		public bool UseRomFs
		{
			get;
			private set;
		}
		public ulong PartitionId
		{
			get;
			private set;
		}
		public uint HandleTableSize
		{
			get;
			private set;
		}
		public bool AllCode
		{
			get;
			private set;
		}
		public Dictionary<string, List<string>> ExeFs
		{
			get;
			private set;
		}
		public List<string> PlainRegionSections
		{
			get;
			private set;
		}
		public string RomFsRoot
		{
			get;
			private set;
		}
		public string[] RejectFiles
		{
			get;
			private set;
		}
		public string[] DefaultRejectFiles
		{
			get;
			private set;
		}
		public string[] IncludeFiles
		{
			get;
			private set;
		}
		public string[] Files
		{
			get;
			private set;
		}
		public MakeCxiOptions.MemoryTypeName MemoryType
		{
			get;
			private set;
		}
		public int AlignSize
		{
			get;
			private set;
		}
		public string MediaSize
		{
			get;
			private set;
		}
		public bool MediaFootPadding
		{
			get;
			private set;
		}
		public byte Padding
		{
			get;
			private set;
		}
		public uint CoreVersion
		{
			get;
			private set;
		}
		public MakeCxiOptions.TitleCategoryName TitleCategory
		{
			get;
			private set;
		}
		public MakeCxiOptions.TitlePlatformName TitlePlatform
		{
			get;
			private set;
		}
		public uint TitleUniqueId
		{
			get;
			private set;
		}
		public byte TitleVariation
		{
			get;
			private set;
		}
		public string ProductCode
		{
			get;
			private set;
		}
		public byte[] AesKey
		{
			get;
			private set;
		}
		public byte MediaUnitSize
		{
			get;
			private set;
		}
		public ushort CompanyCode
		{
			get;
			private set;
		}
		public NcchCommonHeaderFlag.ContentType ContentType
		{
			get;
			private set;
		}
		public MakeCxiOptions.TitleUse Use
		{
			get;
			private set;
		}
		public bool FreeProductCode
		{
			get;
			private set;
		}
		public ulong SaveDataSize
		{
			get;
			private set;
		}
		public UInt64Data JumpId
		{
			get;
			private set;
		}
		public string TopExefsSectionName
		{
			get;
			private set;
		}
		public bool Compress
		{
			get;
			private set;
		}
		public bool Compressed
		{
			get;
			set;
		}
		public byte DependencyVariation
		{
			get;
			private set;
		}
		public MakeCxiOptions.LogoName Logo
		{
			get;
			private set;
		}
		public ulong ExtSaveDataId
		{
			get;
			private set;
		}
		public byte DescVersion
		{
			get;
			private set;
		}
		public byte SystemMode
		{
			get;
			private set;
		}
		public MakeCxiOptions.ResourceLimitCategoryName ResourceLimitCategory
		{
			get;
			private set;
		}
		public uint CategoryFlags
		{
			get;
			private set;
		}
		public uint TargetCategoryFlags
		{
			get;
			private set;
		}
		public string DotRomfsPath
		{
			get;
			private set;
		}
		public string RomfsLayoutPath
		{
			get;
			private set;
		}
		public uint ReleaseKernelMajor
		{
			get;
			private set;
		}
		public uint ReleaseKernelMinor
		{
			get;
			private set;
		}
		public long ExefsReserveSize
		{
			get;
			private set;
		}
		public byte MaxCpu
		{
			get;
			private set;
		}
		public bool CipCompress
		{
			get;
			private set;
		}
		public NyamlOption UserOptions
		{
			get;
			private set;
		}
		public NyamlOption DefaultOptions
		{
			get;
			private set;
		}
		public NyamlAccessControlDescriptor AccessControlDescriptor
		{
			get;
			private set;
		}
		private string UserOptionFile
		{
			get;
			set;
		}
		private string DescriptorFile
		{
			get;
			set;
		}
		public NyamlParameter UserParam
		{
			get;
			private set;
		}
		public NyamlParameter DefaultParam
		{
			get;
			private set;
		}
		public NyamlDescParameter DescParam
		{
			get;
			private set;
		}
		public Dictionary<string, string> UserVariables
		{
			get;
			private set;
		}
		public bool UseAes
		{
			get;
			private set;
		}
		public bool OutputCoreInfo
		{
			get;
			private set;
		}
		public MakeCxiOptions(CxiOption option)
		{
			this.ProcessType = MakeCxiOptions.ProcessTypeName.DEFAULT;
			this.PermitDebug = true;
			this.ForceDebug = false;
			this.PageSize = 4096u;
			this.CoreVersion = 0u;
			this.PartitionId = 18446744073709551615uL;
			this.MediaUnitSize = option.MediaUnitSize;
			this.ExefsReserveSize = option.ExefsReserveSize;
			this.FreeProductCode = false;
			this.ReleaseKernelMajor = 4294967295u;
			this.ReleaseKernelMinor = 4294967295u;
			this.IsCip = option.Cip;
			this.IsCfa = option.Cfa;
			this.IsCaa = option.Caa;
			this.MakeCci2 = option.ForCci2;
			this.AlignSize = option.Align;
			this.UserOptionFile = option.RsfPath;
			this.DescriptorFile = option.DescPath;
			this.TopExefsSectionName = option.TopExefsSectionName;
			this.CipCompress = option.CipCompress;
			this.DotRomfsPath = option.DotRomfsPath;
			this.RomfsLayoutPath = option.RomfsLayoutPath;
			if (option.ElfPath != null)
			{
				this.SourcePath = option.ElfPath;
				this.Source = new FileStream(option.ElfPath, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			this.UserVariables = option.UserVariables;
			this.OutputCoreInfo = option.OutputCoreInfo;
			this.AesKey = option.AesKey;
			this.DependencyVariation = option.DependencyVariation;
			this.BannerPath = option.BannerPath;
			this.IconPath = option.IconPath;
			Parameter parameter = new Parameter(this.UserOptionFile, this.DescriptorFile, this.UserVariables);
			if (this.DescriptorFile != null)
			{
				this.DescParam = parameter.GetDescParameter();
				this.AccessControlDescriptor = new NyamlAccessControlDescriptor(this.DescParam);
				this.DefaultParam = parameter.GetDescDefaultParameter();
				DependencyList.MakeProgramIdMapping(this.DefaultParam.DependencyList);
			}
			NyamlParameter mergedUserParameter = parameter.GetMergedUserParameter();
			this.UserOptions = new NyamlOption(mergedUserParameter, this.AccessControlDescriptor);
			this.SetOptionsFromYaml();
			this.SetupSaveDataSize(option);
			this.SetupEncryptionParameters(option);
			if (!this.IsCfa && (this.ProcessType == MakeCxiOptions.ProcessTypeName.APPLICATION || this.ProcessType == MakeCxiOptions.ProcessTypeName.DEFAULT))
			{
				this.MainThreadPriority += 32;
			}
			this.CheckParameters();
		}
		private void SetupEncryptionParameters(CxiOption option)
		{
			if (option.Cip || option.Cdi)
			{
				this.UseAes = false;
			}
			if (this.AccessControlDescriptor != null && this.AccessControlDescriptor.GetCryptoKey() != null)
			{
				byte[] cryptoKey = this.AccessControlDescriptor.GetCryptoKey();
				if (this.AesKey == null)
				{
					this.AesKey = cryptoKey;
				}
				else
				{
					string text = "";
					byte[] array = cryptoKey;
					for (int i = 0; i < array.Length; i++)
					{
						byte b = array[i];
						text += string.Format("{0:x2}", b);
					}
					string text2 = "";
					byte[] aesKey = this.AesKey;
					for (int j = 0; j < aesKey.Length; j++)
					{
						byte b2 = aesKey[j];
						text2 += string.Format("{0:x2}", b2);
					}
					Util.PrintWarning(string.Format("Desc key '{0}' is ignored. \n-key value '{1}' is in use.\n", text, text2));
				}
			}
			this.AesKey = ((this.AesKey == null) ? Resources.key : this.AesKey);
		}
		private void SetupExtSaveDataId(NyamlOption option)
		{
			if (!option.GetUseExtSaveData())
			{
				if (option.GetExtSaveDataId() != null || option.GetExtSaveDataNumber() != null)
				{
					throw new MakeromException("Failed to set ExtSaveDataNumber. UseExtSaveData must be true.");
				}
				this.ExtSaveDataId = 0uL;
				return;
			}
			else
			{
				if (option.GetExtSaveDataId() != null && option.GetExtSaveDataNumber() != null)
				{
					throw new MakeromException("Cannot set ExtSaveDataId and ExtSaveDataNumber at once.");
				}
				if (option.GetExtSaveDataNumber() != null)
				{
					this.ExtSaveDataId = option.GetExtSaveDataNumber();
					return;
				}
				if (option.GetExtSaveDataId() != null)
				{
					this.ExtSaveDataId = option.GetExtSaveDataId();
					return;
				}
				this.ExtSaveDataId = (ulong)this.TitleUniqueId;
				return;
			}
		}
		private void CheckSaveDataSize(CxiOption option, ulong saveDataSize)
		{
			if (TitleIdUtil.IsSystemCategory(TitleIdUtil.GetCategory(TitleIdUtil.MakeProgramId(this))))
			{
				return;
			}
			NyamlOption.CardMediaType cardMediaType = this.UserOptions.GetCardMediaType();
			if (cardMediaType == NyamlOption.CardMediaType.CARD2)
			{
				if (saveDataSize % 1048576uL != 0uL)
				{
					throw new MakeromException("CARD2's SaveDataSize has to be 0MB, 1MB ... [MediaSize / 2].\n");
				}
				ulong num = Util.SizeStringToUInt64(this.UserOptions.GetMediaSize());
				if (saveDataSize > num / 2uL)
				{
					throw new MakeromException("Too large SaveDataSize. SaveDataSize has be less than or equal to half of MediaSize.\n");
				}
			}
			else
			{
				if (cardMediaType != NyamlOption.CardMediaType.CARD1)
				{
					throw new MakeromException("Unknown card media type.");
				}
				ulong[] source = new ulong[]
				{
					0uL,
					131072uL,
					524288uL
				};
				if (!source.Contains(saveDataSize))
				{
					Util.PrintWarning("CARD1's SaveDataSize has to be 0KB, 128KB or 512KB.\n");
					return;
				}
			}
		}
		private void SetupSaveDataSize(CxiOption option)
		{
			if (!this.IsCfa)
			{
				this.SaveDataSize = this.UserOptions.GetSaveDataSize();
				this.CheckSaveDataSize(option, this.SaveDataSize);
			}
		}
		private void SetOptionsFromYaml()
		{
			this.RomFsRoot = this.UserOptions.GetRomFsRoot();
			this.UseRomFs = (this.RomFsRoot != null && this.RomFsRoot != "");
			this.RejectFiles = this.UserOptions.GetRejectFiles();
			this.IncludeFiles = this.UserOptions.GetIncludeFiles();
			this.DefaultRejectFiles = this.UserOptions.GetDefaultRejectFiles();
			this.Files = this.UserOptions.GetFiles();
			this.UseAes = this.UserOptions.GetUseAesFlag();
			this.NcchCommonHeaderKeyParams = this.GetCommonHeaderKeyParams(this.IsCfa);
			this.MediaSize = this.UserOptions.GetMediaSize();
			this.TitlePlatform = this.UserOptions.GetTitlePlatformName();
			this.CategoryFlags = this.UserOptions.GetCategoryFlags();
			this.TargetCategoryFlags = this.UserOptions.GetTargetCategoryFlags();
			this.TitleUniqueId = this.UserOptions.GetTitleUniqueId();
			this.TitleVariation = this.UserOptions.GetTitleVariation();
			this.ProductCode = this.UserOptions.GetProductCode();
			this.Name = this.UserOptions.GetName();
			this.CompanyCode = this.UserOptions.GetCompanyCode();
			this.ContentType = this.UserOptions.GetContentType();
			this.Use = this.UserOptions.GetTitleUse();
			this.FreeProductCode = this.UserOptions.GetFreeProductCode();
			this.RemasterVersion = this.UserOptions.GetRemasterVersion();
			this.Compress = this.UserOptions.GetEnableCompress();
			this.JumpId = this.UserOptions.GetJumpId();
			this.Logo = this.UserOptions.GetLogoName();
			if (this.JumpId == null)
			{
				this.JumpId = new UInt64Data(TitleIdUtil.MakeProgramId(this));
			}
			if (!this.IsCfa)
			{
				this.DependencyList = this.UserOptions.GetDependencyList();
				this.AffinityMask = this.UserOptions.GetAffinityMask();
				this.IdealProcessor = this.UserOptions.GetIdealProcessor();
				this.MainThreadPriority = this.UserOptions.GetMainThreadPriority();
				this.ServiceAccessControl = this.UserOptions.GetServiceAccessControl();
				this.MainThreadStackSize = this.UserOptions.GetMainThreadStackSize();
				this.ProcessType = this.UserOptions.GetProcessType();
				this.SystemCallAccessControl = this.UserOptions.GetSystemCallAccessControl();
				this.InterruptNumberList = this.UserOptions.GetInterruptNumberList();
				this.Mapping = this.UserOptions.GetAddressMapping();
				this.AccControlDescRsaSign = this.AccessControlDescriptor.GetAccControlDescRsaSign();
				this.AccControlDescBin = this.AccessControlDescriptor.GetAccControlDesc();
				this.PermitDebug = !this.UserOptions.GetDisableDebug();
				this.ForceDebug = this.UserOptions.GetEnableForceDebug();
				this.CanUseNonAlphabetAndNumber = this.UserOptions.GetCanUseNonAlphabetAndNumber();
				this.CanWriteSharedPage = this.UserOptions.GetCanWriteSharedPage();
				this.CanUsePrivilegePriority = this.UserOptions.GetCanUsePrivilegedPriority();
				this.PermitMainFunctionArgument = this.UserOptions.GetPermitMainFunctionArgument();
				this.CanShareDeviceMemory = this.UserOptions.GetCanShareDeviceMemory();
				this.PageSize = this.UserOptions.GetPageSize();
				this.AllowsUnalignedSection = this.UserOptions.GetAllowUnalignedSection();
				this.SystemSaveDataId = this.UserOptions.GetSystemSaveDataId();
				this.StorageAccessableUniqueIds = this.UserOptions.GetStorageAccessableUniqueIds();
				this.FileSystemAccess = this.UserOptions.GetFileSystemAccess();
				this.Arm9AccessControl = this.UserOptions.GetArm9AccessControl();
				this.NcchVersion = this.UserOptions.GetNcchVersion();
				this.HandleTableSize = this.UserOptions.GetHandleTableSize();
				this.RunnableOnSleep = this.UserOptions.GetRunnableOnSleepFlag();
				this.SpecialMemoryArrange = this.UserOptions.GetSpecialMemoryArrangeFlag();
				this.ExeFs = this.UserOptions.GetExeFs();
				this.PlainRegionSections = this.UserOptions.GetPlainRegionSections();
				this.MemoryType = this.UserOptions.GetMemoryType();
				this.MediaFootPadding = this.UserOptions.GetMediaFootPadding();
				this.Padding = this.UserOptions.GetPadding();
				this.CoreVersion = this.UserOptions.GetCoreVersion();
				this.SetupExtSaveDataId(this.UserOptions);
				this.DescVersion = this.UserOptions.GetDescVersion();
				this.SystemMode = this.UserOptions.GetSystemMode();
				this.ResourceLimitCategory = this.UserOptions.GetResourceLimitCategory();
				this.ReleaseKernelMajor = this.UserOptions.GetReleaseKernelMajor();
				this.ReleaseKernelMinor = this.UserOptions.GetReleaseKernelMinor();
				this.MaxCpu = this.UserOptions.GetMaxCpu();
			}
		}
		private RSAParameters GetCommonHeaderKeyParams(bool IsCfa)
		{
			if (!IsCfa)
			{
				return this.AccessControlDescriptor.GetCommonHeaderKeyParams();
			}
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(Resources.DevNcsdCfa);
			return rSACryptoServiceProvider.ExportParameters(true);
		}
		private MakeCxiOptions.ProcessTypeName DecideProcessType(string arg)
		{
			if (arg == null)
			{
				return MakeCxiOptions.ProcessTypeName.APPLICATION;
			}
			string a = arg.ToLower();
			if (a == "system")
			{
				return MakeCxiOptions.ProcessTypeName.SYSTEM;
			}
			return MakeCxiOptions.ProcessTypeName.APPLICATION;
		}
		private void CheckParameters()
		{
			if (!this.IsCfa)
			{
				this.AccessControlDescriptor.CheckProgramId(new UInt64ProgramId(TitleIdUtil.MakeTargetProgramId(this)));
				this.AccessControlDescriptor.CheckAffinityMask(this.AffinityMask);
				this.AccessControlDescriptor.CheckIdealProcessor(this.IdealProcessor);
				this.AccessControlDescriptor.CheckPriority(this.MainThreadPriority);
				this.AccessControlDescriptor.CheckInterruptNumbers(this.InterruptNumberList);
				this.AccessControlDescriptor.CheckHandleTableSize(this.HandleTableSize);
				this.AccessControlDescriptor.CheckMemoryType(this.MemoryType);
			}
			if (this.IsCfa && this.RomFsRoot == null)
			{
				throw new MakeromException("Rom/HostRoot must be set");
			}
			if (Util.IsOldProductCode(this))
			{
				Util.PrintWarning(string.Format("ProductCode \"{0}\" is OLD format.", this.ProductCode));
				this.ProductCode = this.ProductCode.Substring(0, 10);
			}
			if (!Util.IsValidProductCode(this))
			{
				throw new MakeromException("Invalid product code: " + this.ProductCode);
			}
			if (this.AlignSize <= 0)
			{
				throw new ArgumentException("invalid align size: " + this.AlignSize);
			}
			Util.CheckTitleIdRange(this);
		}
	}
}
