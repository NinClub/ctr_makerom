using nyaml;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Nintendo.MakeRom
{
	public class NyamlOption
	{
		private enum BackupMemoryType
		{
			None,
			NOR_128K,
			NOR_512K
		}
		public enum CardMediaType
		{
			CARD1 = 1,
			CARD2
		}
		public enum CardDeviceName
		{
			CARD_DEVICE_NOR_FLASH = 1,
			CARD_DEVICE_NONE,
			CARD_DEVICE_BT
		}
		private const string CATEGORY_APPLICATION = "Application";
		private const string CATEGORY_SYSTEM_APPLICATION = "SystemApplication";
		private const string CATEGORY_APPLET = "Applet";
		private const string CATEGORY_FIRMWARE = "Firmware";
		private const string CATEGORY_BASE = "Base";
		private const string CATEGORY_DLP_CHILD = "DlpChild";
		private const string CATEGORY_TRIAL = "Demo";
		private const string CATEGORY_CONTENTS = "Contents";
		private const string CATEGORY_SYSTEM_CONTENTS = "SystemContents";
		private const string CATEGORY_SHARED_CONTENTS = "SharedContents";
		private const string CATEGORY_ADDON_CONTENTS = "AddOnContents";
		private const string CATEGORY_PATCH = "Patch";
		private readonly AddressMapping DEFAULT_MAPPING = new AddressMapping(new string[0], null);
		private readonly UInt64ProgramId DEFAULT_PROGRAM_ID = new UInt64ProgramId(0uL);
		private readonly DependencyList DEFAULT_DEPENDENCY_LIST = new DependencyList(new string[0]);
		private readonly AffinityMask DEFAULT_AFFINITY_MASK = new AffinityMask(1);
		private readonly ServiceAccessControl DEFAULT_SERVICE_ACCESS_CONTROL = new ServiceAccessControl(new string[0]);
		private readonly UInt64Name DEFAULT_NAME = new UInt64Name("default");
		private readonly UInt64Name DEFAULT_STORAGE_ID = new UInt64Name("0");
		private byte m_DescVersion;
		private NyamlParameter m_Param;
		private NyamlAccessControlDescriptor m_AccCtrlDescriptor;
		public Nyaml Nyaml
		{
			get;
			private set;
		}
		public CollectionElement Capability
		{
			get;
			private set;
		}
		public CollectionElement DescriptorData
		{
			get;
			private set;
		}
		public NyamlOption(NyamlParameter param, NyamlAccessControlDescriptor desc)
		{
			this.m_Param = param;
			this.m_AccCtrlDescriptor = desc;
			this.SetParametersFromDesc();
		}
		public NyamlOption(NyamlParameter param)
		{
			this.m_Param = param;
		}
		private void CheckParameter(CollectionElement param, string name)
		{
			if (param == null)
			{
				throw new ParameterNotFoundException(name);
			}
		}
		private void SetParametersFromDesc()
		{
			if (this.m_AccCtrlDescriptor == null)
			{
				return;
			}
			if (this.m_Param.ServiceAccessControl == null)
			{
				this.m_Param.ServiceAccessControl = this.m_AccCtrlDescriptor.GetDefaultServiceAccesses();
			}
			if (this.m_Param.DefaultSystemCalls == null)
			{
				this.m_Param.DefaultSystemCalls = this.m_AccCtrlDescriptor.GetDefaultSystemCalls();
			}
			if (this.m_Param.HandleTableSize == null)
			{
				this.m_Param.HandleTableSize = this.m_AccCtrlDescriptor.GetHandleTableSize();
			}
			if (this.m_Param.MemoryType == null)
			{
				this.m_Param.MemoryType = this.m_AccCtrlDescriptor.GetDefaultMemoryType();
			}
			if (this.m_AccCtrlDescriptor.GetSystemMode() != null)
			{
				this.m_Param.SystemMode = this.m_AccCtrlDescriptor.GetSystemMode();
			}
			this.m_DescVersion = this.m_AccCtrlDescriptor.GetDescVersion();
			this.m_Param.ResourceLimitCategory = this.m_AccCtrlDescriptor.GetResourceLimitCategory();
			this.m_Param.RunnableOnSleep = this.m_AccCtrlDescriptor.GetRunnableOnSleepFlag();
			this.m_Param.SpecialMemoryArrange = this.m_AccCtrlDescriptor.GetSpecialMemoryArrangeFlag();
			this.m_Param.ReleaseKernelMajor = this.m_AccCtrlDescriptor.GetReleaseKernelMajor();
			this.m_Param.ReleaseKernelMinor = this.m_AccCtrlDescriptor.GetReleaseKernelMinor();
			this.m_Param.MaxCpu = this.m_AccCtrlDescriptor.GetMaxCpu();
		}
		internal AddressMapping GetAddressMapping()
		{
			string[] stringArray = Nyaml.GetStringArray(this.m_Param.IORegisterMapping);
			string[] stringArray2 = Nyaml.GetStringArray(this.m_Param.MemoryMapping);
			this.m_AccCtrlDescriptor.CheckMemoryMapping(stringArray2);
			this.m_AccCtrlDescriptor.CheckIORegisterMapping(stringArray);
			return new AddressMapping(stringArray, stringArray2);
		}
		internal DependencyList GetDependencyList()
		{
			this.CheckParameter(this.m_Param.DependencyList, NyamlOptionStrings.DEPENDENCY_LIST_STRING);
			if (this.m_Param.DependencyList.GetType() == typeof(Sequence) || this.m_Param.DependencyList.GetType() == typeof(ScalarNull))
			{
				string[] stringArray = Nyaml.GetStringArray(this.m_Param.DependencyList);
				return new DependencyList(stringArray);
			}
			if (this.m_Param.DependencyList.GetType() == typeof(Mapping))
			{
				return new DependencyList((Mapping)this.m_Param.DependencyList);
			}
			throw new MakeromException("Inavlid dependency list");
		}
		internal AffinityMask GetAffinityMask()
		{
			this.CheckParameter(this.m_Param.AffinityMask, NyamlOptionStrings.AFFINITY_MASK_STRING);
			return new AffinityMask(this.m_Param.AffinityMask.GetInteger());
		}
		public sbyte GetIdealProcessor()
		{
			if (this.m_Param.IdealProcessor != null)
			{
				return (sbyte)this.m_Param.IdealProcessor.GetInteger();
			}
			throw new ParameterNotFoundException(NyamlOptionStrings.IDEAL_PROCESSOR_STRING);
		}
		public sbyte GetMainThreadPriority()
		{
			this.CheckParameter(this.m_Param.MainThreadPriority, NyamlOptionStrings.PRIORITY_STRING);
			return (sbyte)this.m_Param.MainThreadPriority.GetInteger();
		}
		internal ServiceAccessControl GetServiceAccessControl()
		{
			this.CheckParameter(this.m_Param.ServiceAccessControl, NyamlOptionStrings.SERVICE_ACCESS_CONTROL_STRING);
			this.m_AccCtrlDescriptor.CheckServiceAccessControl(Nyaml.GetStringArray(this.m_Param.ServiceAccessControl));
			return new ServiceAccessControl(Nyaml.GetStringArray(this.m_Param.ServiceAccessControl));
		}
		public uint GetMainThreadStackSize()
		{
			CollectionElement mainThreadStackSize = this.m_Param.MainThreadStackSize;
			if (mainThreadStackSize == null || mainThreadStackSize.IsNullScalar)
			{
				throw new ParameterNotFoundException(NyamlOptionStrings.STACK_SIZE_STRING);
			}
			return (uint)mainThreadStackSize.GetInteger();
		}
		internal MakeCxiOptions.ProcessTypeName GetProcessType()
		{
			CollectionElement processType = this.m_Param.ProcessType;
			if (processType == null || processType.IsNullScalar)
			{
				throw new ParameterNotFoundException(NyamlOptionStrings.APP_TYPE_STRING);
			}
			if (processType.GetString().ToLower() == "system")
			{
				return MakeCxiOptions.ProcessTypeName.SYSTEM;
			}
			if (processType.GetString().ToLower() == "application")
			{
				return MakeCxiOptions.ProcessTypeName.APPLICATION;
			}
			return MakeCxiOptions.ProcessTypeName.DEFAULT;
		}
		internal SystemCallAccessControl GetSystemCallAccessControl()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			Mapping enableSystemCalls = this.m_AccCtrlDescriptor.GetEnableSystemCalls();
			string[] stringArray = Nyaml.GetStringArray(this.m_Param.DefaultSystemCalls);
			if (stringArray != null)
			{
				string[] array = stringArray;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					CollectionElement collectionElement = enableSystemCalls[text];
					if (collectionElement == null)
					{
						throw new NotPermittedValueException("SystemCallAccess", text);
					}
					list.Add(collectionElement.GetInteger());
				}
			}
			stringArray = Nyaml.GetStringArray(this.m_Param.AppendSystemCalls);
			if (stringArray != null)
			{
				string[] array2 = stringArray;
				for (int j = 0; j < array2.Length; j++)
				{
					string text2 = array2[j];
					CollectionElement collectionElement2 = enableSystemCalls[text2];
					if (collectionElement2 == null)
					{
						throw new NotPermittedValueException("SystemCallAccess", text2);
					}
					list2.Add(collectionElement2.GetInteger());
				}
			}
			return new SystemCallAccessControl(list.ToArray(), list2.ToArray());
		}
		internal InterruptNumberList GetInterruptNumberList()
		{
			if (this.m_Param.InterruptNumbers != null)
			{
				string[] stringArray = Nyaml.GetStringArray(this.m_Param.InterruptNumbers);
				return new InterruptNumberList(string.Join(",", stringArray));
			}
			if (this.m_AccCtrlDescriptor.GetEnableInterruptNumbers() != null)
			{
				return new InterruptNumberList(string.Join(",", this.m_AccCtrlDescriptor.GetEnableInterruptNumbers()));
			}
			throw new ParameterNotFoundException(NyamlOptionStrings.INTERRUPT_NUMBERS_STRING);
		}
		internal UInt64Name GetName()
		{
			this.CheckParameter(this.m_Param.Title, NyamlOptionStrings.TITLE_STRING);
			return new UInt64Name(this.m_Param.Title.GetString());
		}
		public string GetRomFsRoot()
		{
			if (this.m_Param.HostRoot != null && !this.m_Param.HostRoot.IsNullScalar && this.m_Param.HostRoot.GetString() != "")
			{
				return this.m_Param.HostRoot.GetString();
			}
			return null;
		}
		internal ulong GetStorageId()
		{
			this.CheckParameter(this.m_Param.StorageId, NyamlOptionStrings.STORAGE_ID_STRING);
			if (this.m_Param.StorageId.GetType() == typeof(ScalarString))
			{
				return new UInt64Name(this.m_Param.StorageId.GetString());
			}
			if (this.m_Param.StorageId.GetType() == typeof(ScalarLong))
			{
				return (ulong)this.m_Param.StorageId.GetLong();
			}
			if (this.m_Param.StorageId.GetType() == typeof(ScalarInteger))
			{
				return (ulong)((long)this.m_Param.StorageId.GetInteger());
			}
			throw new MakeromException("StorageId type is invalid: " + this.m_Param.StorageId.GetType());
		}
		internal ulong ConvertUInt64(CollectionElement element, string elmentName)
		{
			if (element.GetType() == typeof(ScalarString))
			{
				return new UInt64Name(element.GetString());
			}
			if (element.GetType() == typeof(ScalarLong))
			{
				return (ulong)element.GetLong();
			}
			if (element.GetType() == typeof(ScalarInteger))
			{
				return (ulong)((long)element.GetInteger());
			}
			throw new MakeromException(elmentName + " type is invalid: " + element.GetType());
		}
		internal ulong GetSystemSaveDataId()
		{
			this.CheckParameter(this.m_Param.SystemSaveDataId1, NyamlOptionStrings.SYSTEM_SAVEDATA_ID_STRING1);
			this.CheckParameter(this.m_Param.SystemSaveDataId2, NyamlOptionStrings.SYSTEM_SAVEDATA_ID_STRING2);
			ulong num = this.ConvertUInt64(this.m_Param.SystemSaveDataId2, "SystemSaveDataId2");
			num <<= 32;
			return num + this.ConvertUInt64(this.m_Param.SystemSaveDataId1, "SystemSaveDataId1");
		}
		internal ulong GetStorageAccessableUniqueIds()
		{
			this.CheckParameter(this.m_Param.OtherUserSaveDataId1, NyamlOptionStrings.OTHER_USER_SAVEDATA_ID_STRING1);
			this.CheckParameter(this.m_Param.OtherUserSaveDataId2, NyamlOptionStrings.OTHER_USER_SAVEDATA_ID_STRING2);
			this.CheckParameter(this.m_Param.OtherUserSaveDataId3, NyamlOptionStrings.OTHER_USER_SAVEDATA_ID_STRING3);
			this.CheckParameter(this.m_Param.UseOtherVariationSaveData, NyamlOptionStrings.USE_OTHER_VARIATION_SAVE_DATA_STRING);
			ulong num = this.ConvertUInt64(this.m_Param.OtherUserSaveDataId1, "OtherUserSaveDataId1");
			num <<= 20;
			num |= this.ConvertUInt64(this.m_Param.OtherUserSaveDataId2, "OtherUserSaveDataId2");
			num <<= 20;
			num |= this.ConvertUInt64(this.m_Param.OtherUserSaveDataId3, "OtherUserSaveDataId3");
			if (this.m_Param.UseOtherVariationSaveData.GetType() == typeof(ScalarBool))
			{
				if (this.m_Param.UseOtherVariationSaveData.GetBoolean())
				{
					num |= 1152921504606846976uL;
				}
				return num;
			}
			throw new MakeromException(string.Format("Invalid parameter \"UseOtherVariationSaveData: {0}\"", this.m_Param.UseOtherVariationSaveData.GetString()));
		}
		public bool GetIsForCard()
		{
			this.CheckParameter(this.m_Param.NoPadding, NyamlOptionStrings.NO_PADDING_STRING);
			return !this.m_Param.NoPadding.GetBoolean();
		}
		public bool GetDisableDebug()
		{
			this.CheckParameter(this.m_Param.DisableDebug, NyamlOptionStrings.DISABLE_DEBUG_STRING);
			return this.m_Param.DisableDebug.GetBoolean();
		}
		public bool GetEnableForceDebug()
		{
			this.CheckParameter(this.m_Param.EnableForceDebug, NyamlOptionStrings.ENABLE_FORCE_DEBUG_STRING);
			return this.m_Param.EnableForceDebug.GetBoolean();
		}
		public bool GetCanUseNonAlphabetAndNumber()
		{
			this.CheckParameter(this.m_Param.CanUseNonAlphabetAndNumber, NyamlOptionStrings.CAN_USE_NON_ALPHABET_STRING);
			return this.m_Param.CanUseNonAlphabetAndNumber.GetBoolean();
		}
		public bool GetCanWriteSharedPage()
		{
			this.CheckParameter(this.m_Param.CanWriteSharedPage, NyamlOptionStrings.CAN_WRITE_SHARED_PAGE_STRING);
			return this.m_Param.CanWriteSharedPage.GetBoolean();
		}
		public bool GetCanUsePrivilegedPriority()
		{
			this.CheckParameter(this.m_Param.CanUsePrivilegePriority, NyamlOptionStrings.CAN_USE_PRIVILEGE_PRIORITY_STRING);
			return this.m_Param.CanUsePrivilegePriority.GetBoolean();
		}
		public bool GetPermitMainFunctionArgument()
		{
			this.CheckParameter(this.m_Param.PermitMainFunctionArgument, NyamlOptionStrings.PERMIT_MAIN_FUNCTION_ARGUMENT);
			return this.m_Param.PermitMainFunctionArgument.GetBoolean();
		}
		public bool GetCanShareDeviceMemory()
		{
			this.CheckParameter(this.m_Param.CanShareDeviceMemory, NyamlOptionStrings.CAN_SHARE_DEVICE_MEMORY_STRING);
			return this.m_Param.CanShareDeviceMemory.GetBoolean();
		}
		public uint GetPageSize()
		{
			this.CheckParameter(this.m_Param.PageSize, NyamlOptionStrings.PAGE_SIZE_STRING);
			return (uint)this.m_Param.PageSize.GetInteger();
		}
		public bool GetAllowUnalignedSection()
		{
			this.CheckParameter(this.m_Param.AllowUnalignedSection, NyamlOptionStrings.ALLOW_UNALIGNED_SECTION_STRING);
			return this.m_Param.AllowUnalignedSection.GetBoolean();
		}
		public ushort GetRemasterVersion()
		{
			this.CheckParameter(this.m_Param.RemasterVersion, NyamlOptionStrings.REMASTER_VERSION_STRING);
			return (ushort)this.m_Param.RemasterVersion.GetInteger();
		}
		public bool GetUseAesFlag()
		{
			this.CheckParameter(this.m_Param.UseAes, NyamlOptionStrings.USE_AES_STRING);
			return this.m_Param.UseAes.GetBoolean();
		}
		public ushort GetNcchVersion()
		{
			return 2;
		}
		public string[] GetRejectFiles()
		{
			if (this.m_Param.Reject != null && !this.m_Param.Reject.IsNullScalar)
			{
				return Nyaml.GetStringArray(this.m_Param.Reject);
			}
			return null;
		}
		public string[] GetIncludeFiles()
		{
			if (this.m_Param.Include != null && !this.m_Param.Include.IsNullScalar)
			{
				return Nyaml.GetStringArray(this.m_Param.Include);
			}
			return null;
		}
		public string[] GetDefaultRejectFiles()
		{
			if (this.m_Param.DefaultReject != null && !this.m_Param.DefaultReject.IsNullScalar)
			{
				return Nyaml.GetStringArray(this.m_Param.DefaultReject);
			}
			return null;
		}
		public string[] GetFiles()
		{
			if (this.m_Param.Files != null && !this.m_Param.Files.IsNullScalar)
			{
				return Nyaml.GetStringArray(this.m_Param.Files);
			}
			return null;
		}
		public uint GetHandleTableSize()
		{
			this.CheckParameter(this.m_Param.HandleTableSize, NyamlOptionStrings.HANDLE_TABLE_SIZE_STRING);
			return (uint)this.m_Param.HandleTableSize.GetInteger();
		}
		public Dictionary<string, List<string>> GetExeFs()
		{
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			string[] array = new string[]
			{
				"Text",
				"ReadOnly",
				"ReadWrite"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string key = array2[i];
				dictionary[key] = new List<string>();
			}
			if (this.m_Param.ExeFs == null)
			{
				return dictionary;
			}
			Mapping mapping = (Mapping)this.m_Param.ExeFs;
			string[] array3 = array;
			for (int j = 0; j < array3.Length; j++)
			{
				string key2 = array3[j];
				if (mapping.Elements.ContainsKey(key2) && !mapping.Elements[key2].IsNullScalar)
				{
					Sequence sequence = (Sequence)mapping.Elements[key2];
					if (sequence != null && !sequence.IsNullScalar)
					{
						foreach (CollectionElement current in sequence.Elements)
						{
							dictionary[key2].Add(current.GetString());
						}
					}
				}
			}
			return dictionary;
		}
		public List<string> GetPlainRegionSections()
		{
			List<string> list = new List<string>();
			if (this.m_Param.PlainRegion != null)
			{
				list.AddRange(Nyaml.GetStringArray(this.m_Param.PlainRegion));
			}
			return list;
		}
		internal static MakeCxiOptions.MemoryTypeName StringToMemoryTypeName(string type)
		{
			if (type.ToLower() == "application")
			{
				return MakeCxiOptions.MemoryTypeName.APPLICATION;
			}
			if (type.ToLower() == "system")
			{
				return MakeCxiOptions.MemoryTypeName.SYSTEM;
			}
			if (type.ToLower() == "base")
			{
				return MakeCxiOptions.MemoryTypeName.BASE;
			}
			throw new MakeromException(string.Format("Invalid memory type: \"{0}\"", type));
		}
		internal MakeCxiOptions.MemoryTypeName GetMemoryType()
		{
			this.CheckParameter(this.m_Param.MemoryType, NyamlOptionStrings.MEMORY_TYPE_STRING);
			return NyamlOption.StringToMemoryTypeName(this.m_Param.MemoryType.GetString());
		}
		public byte GetPadding()
		{
			this.CheckParameter(this.m_Param.Padding, NyamlOptionStrings.PADDING_STRING);
			return (byte)this.m_Param.Padding.GetInteger();
		}
		public bool GetMediaFootPadding()
		{
			this.CheckParameter(this.m_Param.MediaFootPadding, NyamlOptionStrings.MEDIA_FOOT_PADDING_STRING);
			return this.m_Param.MediaFootPadding.GetBoolean();
		}
		public string GetMediaSize()
		{
			this.CheckParameter(this.m_Param.MediaSize, NyamlOptionStrings.MEDIA_SIZE_STRING);
			return this.m_Param.MediaSize.GetString();
		}
		internal bool GetRunnableOnSleepFlag()
		{
			return this.m_Param.RunnableOnSleep != null && this.m_Param.RunnableOnSleep.GetBoolean();
		}
		internal bool GetSpecialMemoryArrangeFlag()
		{
			return this.m_Param.SpecialMemoryArrange != null && this.m_Param.SpecialMemoryArrange.GetBoolean();
		}
		public uint GetCoreVersion()
		{
			if (this.m_Param.FirmwareVersion != null && this.m_Param.CoreVersion != null)
			{
				throw new MakeromException("Already FirmwareVersion has set. Can not set CoreVersion.");
			}
			if (this.m_Param.CoreVersion != null)
			{
				CollectionElement coreVersion = this.m_Param.CoreVersion;
				if (coreVersion.GetType() == typeof(ScalarString))
				{
					string @string = coreVersion.GetString();
					uint num = 0u;
					int num2 = 0;
					while (num2 < @string.Length && num2 < 4)
					{
						num |= (uint)((uint)@string[num2] << (24 - 8 * num2 & 31));
						num2++;
					}
					return num;
				}
				if (coreVersion.GetType() == typeof(ScalarInteger))
				{
					return (uint)this.m_Param.CoreVersion.GetInteger();
				}
				throw new MakeromException("Invalid CoreVersion");
			}
			else
			{
				if (this.m_Param.FirmwareVersion != null)
				{
					return (uint)this.m_Param.FirmwareVersion.GetInteger();
				}
				throw new ParameterNotFoundException(NyamlOptionStrings.FIRMWARE_VERSION_STRING);
			}
		}
		public uint GetTitleUniqueId()
		{
			this.CheckParameter(this.m_Param.TitleUniqueId, NyamlOptionStrings.TITLE_INFO_UNIQUE_ID);
			if (this.m_Param.TitleUniqueId.GetType() == typeof(ScalarInteger))
			{
				return (uint)this.m_Param.TitleUniqueId.GetInteger();
			}
			throw new MakeromException("Invalid Title UniqueId");
		}
		internal MakeCxiOptions.TitlePlatformName GetTitlePlatformName()
		{
			this.CheckParameter(this.m_Param.TitlePlatform, NyamlOptionStrings.TITLE_PLATFORM);
			Dictionary<string, MakeCxiOptions.TitlePlatformName> dictionary = new Dictionary<string, MakeCxiOptions.TitlePlatformName>
			{

				{
					"ctr",
					MakeCxiOptions.TitlePlatformName.CTR
				}
			};
			if (this.m_Param.TitlePlatform.GetType() == typeof(ScalarString))
			{
				string key = this.m_Param.TitlePlatform.GetString().ToLower();
				if (dictionary.ContainsKey(key))
				{
					return dictionary[key];
				}
			}
			throw new MakeromException("Invalid Title Platform ");
		}
		public string GetProductCode()
		{
			this.CheckParameter(this.m_Param.ProductCode, NyamlOptionStrings.PRODUCT_CODE_STRING);
			return this.m_Param.ProductCode.GetString();
		}
		public byte GetTitleVersion()
		{
			this.CheckParameter(this.m_Param.TitleVersion, NyamlOptionStrings.TITLE_INFO_VERSION);
			CollectionElement titleVersion = this.m_Param.TitleVersion;
			if (titleVersion.GetType() == typeof(ScalarInteger))
			{
				return (byte)titleVersion.GetInteger();
			}
			throw new MakeromException("Invalid Title Variation");
		}
		public byte GetTitleVariation()
		{
			uint categoryFlags = this.GetCategoryFlags();
			CollectionElement collectionElement;
			if (this.GetCategoryName() == "AddOnContents")
			{
				this.CheckParameter(this.m_Param.TitleDataTitleIndex, NyamlOptionStrings.TITLE_INFO_DATA_TITLE_INDEX);
				collectionElement = this.m_Param.TitleDataTitleIndex;
			}
			else
			{
				if (TitleIdUtil.IsAdditionContents(categoryFlags))
				{
					this.CheckParameter(this.m_Param.TitleContentsIndex, NyamlOptionStrings.TITLE_INFO_CONTENTS_INDEX);
					collectionElement = this.m_Param.TitleContentsIndex;
				}
				else
				{
					if (TitleIdUtil.IsDlpChild(categoryFlags))
					{
						this.CheckParameter(this.m_Param.TitleChildIndex, NyamlOptionStrings.TITLE_INFO_CHILD_INDEX);
						collectionElement = this.m_Param.TitleChildIndex;
					}
					else
					{
						if (TitleIdUtil.IsDemo(categoryFlags))
						{
							this.CheckParameter(this.m_Param.TitleDemoIndex, NyamlOptionStrings.TITLE_INFO_DEMO_INDEX);
							collectionElement = this.m_Param.TitleDemoIndex;
						}
						else
						{
							this.CheckParameter(this.m_Param.TitleVersion, NyamlOptionStrings.TITLE_INFO_VERSION);
							collectionElement = this.m_Param.TitleVersion;
						}
					}
				}
			}
			if (collectionElement.GetType() != typeof(ScalarInteger))
			{
				throw new MakeromException("Invalid Title Variation");
			}
			byte b = (byte)collectionElement.GetInteger();
			if (TitleIdUtil.IsDemo(categoryFlags) && b == 0)
			{
				throw new MakeromException("Invalid demo index");
			}
			return b;
		}
		public ulong GetWritableAddress()
		{
			this.CheckParameter(this.m_Param.CardWritableAddress, NyamlOptionStrings.CARD_WRITABLE_ADDRESS_STRING);
			return (ulong)this.m_Param.CardWritableAddress.GetLong();
		}
		public int GetCardCryptoType()
		{
			this.CheckParameter(this.m_Param.CardCryptoType, NyamlOptionStrings.CARD_CRYPTO_TYPE_STRING);
			int integer = this.m_Param.CardCryptoType.GetInteger();
			if (integer < 0 || integer > 3)
			{
				throw new MakeromException("Invalid card crypto type: " + integer);
			}
			if (integer != 3)
			{
				Util.PrintWarning(string.Format("Warning: Card crypto type = \"{0}\"", integer));
			}
			return integer;
		}
		public ushort GetCompanyCode()
		{
			this.CheckParameter(this.m_Param.CompanyCode, NyamlOptionStrings.COMPANY_CODE_STRING);
			if (this.m_Param.CompanyCode.GetType() == typeof(ScalarString))
			{
				string @string = this.m_Param.CompanyCode.GetString();
				if (@string.Length != 2)
				{
					throw new MakeromException("Company code length must be 2");
				}
				this.m_Param.CompanyCode.GetString().ToUInt16ASCII();
				return this.m_Param.CompanyCode.GetString().ToUInt16ASCII();
			}
			else
			{
				if (this.m_Param.CompanyCode.GetType() == typeof(ScalarNull))
				{
					throw new MakeromException("Cannot set null to CompanyCode");
				}
				throw new MakeromException("Invalid CompanyCode");
			}
		}
		public ulong GetSaveDataSize()
		{
			this.CheckParameter(this.m_Param.SaveDataSize, NyamlOptionStrings.SAVE_DATA_SIZE_STRING);
			if (this.m_Param.SaveDataSize.GetType() != typeof(ScalarString))
			{
				throw new MakeromException("Invalid save data size format.");
			}
			string text = this.m_Param.SaveDataSize.GetString();
			string pattern = "^\\d+[GMK]B?$";
			Match match = Regex.Match(text, pattern);
			if (!match.Success)
			{
				throw new MakeromException("Invalid save data size format.");
			}
			text = text.ToUpper();
			ulong num = (text.IndexOf("G") != -1) ? 1073741824uL : ((text.IndexOf("M") != -1) ? 1048576uL : 1024uL);
			text = text.TrimEnd(new char[]
			{
				'G',
				'M',
				'K',
				'B'
			});
			num *= (ulong)uint.Parse(text);
			if (num % 65536uL != 0uL)
			{
				throw new MakeromException("Save data size must be 64K align.");
			}
			return num;
		}
		internal NcchCommonHeaderFlag.ContentType GetContentType()
		{
			this.CheckParameter(this.m_Param.ContentType, NyamlOptionStrings.CONTENT_TYPE_STRING);
			Dictionary<string, NcchCommonHeaderFlag.ContentType> dictionary = new Dictionary<string, NcchCommonHeaderFlag.ContentType>
			{

				{
					"application",
					NcchCommonHeaderFlag.ContentType.Application
				},

				{
					"systemupdate",
					NcchCommonHeaderFlag.ContentType.SystemUpdate
				},

				{
					"manual",
					NcchCommonHeaderFlag.ContentType.Manual
				},

				{
					"child",
					NcchCommonHeaderFlag.ContentType.Child
				},

				{
					"trial",
					NcchCommonHeaderFlag.ContentType.Trial
				}
			};
			if (this.m_Param.ContentType.GetType() == typeof(ScalarString))
			{
				string key = this.m_Param.ContentType.GetString().ToLower();
				if (dictionary.ContainsKey(key))
				{
					return dictionary[key];
				}
			}
			throw new MakeromException("Invalid ContentType");
		}
		internal MakeCxiOptions.TitleUse GetTitleUse()
		{
			this.CheckParameter(this.m_Param.TitleUse, NyamlOptionStrings.TITLE_INFO_USE);
			Dictionary<string, MakeCxiOptions.TitleUse> dictionary = new Dictionary<string, MakeCxiOptions.TitleUse>
			{

				{
					"application",
					MakeCxiOptions.TitleUse.APPLICATION
				},

				{
					"system",
					MakeCxiOptions.TitleUse.SYSTEM
				},

				{
					"evaluation",
					MakeCxiOptions.TitleUse.EVALUATION
				}
			};
			if (this.m_Param.TitleUse.GetType() == typeof(ScalarString))
			{
				string key = this.m_Param.TitleUse.GetString().ToLower();
				if (dictionary.ContainsKey(key))
				{
					return dictionary[key];
				}
			}
			throw new MakeromException("Invalid TitleUse");
		}
		internal bool GetFreeProductCode()
		{
			this.CheckParameter(this.m_Param.FreeProductCode, NyamlOptionStrings.FREE_PRODUCT_CODE_STRING);
			if (this.m_Param.FreeProductCode.GetType() == typeof(ScalarBool))
			{
				return this.m_Param.FreeProductCode.GetBoolean();
			}
			throw new MakeromException("Invalid FreeProductCode");
		}
		internal bool GetEnableCompress()
		{
			this.CheckParameter(this.m_Param.EnableCompress, NyamlOptionStrings.ENABLE_COMPRESS_STRING);
			if (this.m_Param.FreeProductCode.GetType() == typeof(ScalarBool))
			{
				return this.m_Param.EnableCompress.GetBoolean();
			}
			throw new MakeromException("Invalid compress flag");
		}
		internal UInt64Data GetJumpId()
		{
			if (this.m_Param.JumpId == null || this.m_Param.JumpId.GetType() == typeof(ScalarNull))
			{
				return null;
			}
			if (this.m_Param.JumpId.GetType() == typeof(ScalarLong))
			{
				return new UInt64Data((ulong)this.m_Param.JumpId.GetLong());
			}
			throw new MakeromException("Invalid jump id");
		}
		public string GetCardType()
		{
			this.CheckParameter(this.m_Param.CardType, NyamlOptionStrings.CARD_TYPE_STRING);
			if (this.m_Param.CardType.GetType() == typeof(ScalarString))
			{
				string text = this.m_Param.CardType.GetString().ToLower();
				if (text == "s1" || text == "s2")
				{
					return text;
				}
			}
			throw new MakeromException("Invalid card type: " + this.m_Param.CardType.GetString());
		}
		internal MakeCxiOptions.LogoName GetLogoName()
		{
			this.CheckParameter(this.m_Param.Logo, NyamlOptionStrings.LOGO_STRING);
			if (this.m_Param.Logo.GetType() == typeof(ScalarNull))
			{
				return MakeCxiOptions.LogoName.NONE;
			}
			Dictionary<string, MakeCxiOptions.LogoName> dictionary = new Dictionary<string, MakeCxiOptions.LogoName>
			{

				{
					"nintendo",
					MakeCxiOptions.LogoName.NINTENDO
				},

				{
					"licensed",
					MakeCxiOptions.LogoName.LICENSED
				},

				{
					"distributed",
					MakeCxiOptions.LogoName.DISTRIBUTED
				},

				{
					"ique",
					MakeCxiOptions.LogoName.IQUE
				},

				{
					"iqueforsystem",
					MakeCxiOptions.LogoName.IQUEFORSYSTEM
				},

				{
					"none",
					MakeCxiOptions.LogoName.NONE
				}
			};
			string key = this.m_Param.Logo.GetString().ToLower();
			if (dictionary.ContainsKey(key))
			{
				return dictionary[key];
			}
			throw new MakeromException(string.Format("Invalid logo name \"{0}\"", this.m_Param.Logo.GetString()));
		}
		internal bool GetUseExtSaveData()
		{
			this.CheckParameter(this.m_Param.UseExtSaveData, NyamlOptionStrings.USE_EXT_SAVE_DATA_STRING);
			if (this.m_Param.UseExtSaveData.GetType() == typeof(ScalarBool))
			{
				return this.m_Param.UseExtSaveData.GetBoolean();
			}
			throw new MakeromException(string.Format("Invalid parameter \"UseExtSaveData: {0}\"", this.m_Param.UseExtSaveData.GetString()));
		}
		internal UInt64Data GetExtSaveDataId()
		{
			this.CheckParameter(this.m_Param.ExtSaveDataId, NyamlOptionStrings.EXT_SAVE_DATA_ID_STRING);
			if (this.m_Param.ExtSaveDataId.GetType() == typeof(ScalarInteger))
			{
				return new UInt64Data((ulong)((long)this.m_Param.ExtSaveDataId.GetInteger()));
			}
			if (this.m_Param.ExtSaveDataId.GetType() == typeof(ScalarLong))
			{
				return new UInt64Data((ulong)this.m_Param.ExtSaveDataId.GetLong());
			}
			if (this.m_Param.ExtSaveDataId.GetType() == typeof(ScalarNull))
			{
				return null;
			}
			throw new MakeromException(string.Format("Invalid ExtSaveDataId: {0}", this.m_Param.ExtSaveDataId.GetString()));
		}
		internal UInt64Data GetExtSaveDataNumber()
		{
			this.CheckParameter(this.m_Param.ExtSaveDataNumber, NyamlOptionStrings.EXT_SAVE_DATA_NUMBER_STRING);
			ulong num;
			if (this.m_Param.ExtSaveDataNumber.GetType() == typeof(ScalarInteger))
			{
				num = (ulong)((long)this.m_Param.ExtSaveDataNumber.GetInteger());
			}
			else
			{
				if (this.m_Param.ExtSaveDataNumber.GetType() == typeof(ScalarLong))
				{
					num = (ulong)this.m_Param.ExtSaveDataNumber.GetLong();
				}
				else
				{
					if (this.m_Param.ExtSaveDataNumber.GetType() == typeof(ScalarNull))
					{
						return null;
					}
					throw new MakeromException(string.Format("Invalid ExtSaveDataNumber: {0}", this.m_Param.ExtSaveDataNumber.GetString()));
				}
			}
			if (num > 1048575uL)
			{
				throw new MakeromException(string.Format("Unexpected ExtSaveDataNumber: {0:x}\nExpected range: 0x0 - 0xfffff", num));
			}
			return new UInt64Data(num);
		}
		internal IEnumerable<StorageInfo.FileSystemAccess> GetFileSystemAccess()
		{
			this.CheckParameter(this.m_Param.FileSystemAccess, NyamlOptionStrings.FILE_SYSTEM_ACCESS_STRING);
			if (this.m_Param.FileSystemAccess.GetType() == typeof(Sequence) || this.m_Param.FileSystemAccess.GetType() == typeof(ScalarNull))
			{
				List<StorageInfo.FileSystemAccess> list = new List<StorageInfo.FileSystemAccess>();
				string[] stringArray = Nyaml.GetStringArray(this.m_Param.FileSystemAccess);
				for (int i = 0; i < stringArray.Length; i++)
				{
					string s = stringArray[i];
					list.Add(NyamlOption.GetFileSystemAccessNumber(s));
				}
				return list;
			}
			throw new MakeromException("Inavlid FileSystemAccess list");
		}
		private static StorageInfo.FileSystemAccess GetFileSystemAccessNumber(string s)
		{
			switch (s)
			{
			case "CategorySystemApplication":
				return StorageInfo.FileSystemAccess.CATEGORY_SYSTEM_APPLICATION;
			case "CategoryHardwareCheck":
				return StorageInfo.FileSystemAccess.CATEGORY_HARDWARE_CHECK;
			case "CategoryFileSystemTool":
				return StorageInfo.FileSystemAccess.CATEGORY_FILE_SYSTEM_TOOL;
			case "Debug":
				return StorageInfo.FileSystemAccess.DEBUG;
			case "TwlCardBackup":
				return StorageInfo.FileSystemAccess.TWL_CARD_BACKUP;
			case "TwlNandData":
				return StorageInfo.FileSystemAccess.TWL_NAND_DATA;
			case "Boss":
				return StorageInfo.FileSystemAccess.BOSS;
			case "DirectSdmc":
				return StorageInfo.FileSystemAccess.DIRECT_SDMC;
			case "Core":
				return StorageInfo.FileSystemAccess.CORE;
			case "CtrNandRo":
				return StorageInfo.FileSystemAccess.CTR_NAND_RO;
			case "CtrNandRw":
				return StorageInfo.FileSystemAccess.CTR_NAND_RW;
			case "CtrNandRoWrite":
				return StorageInfo.FileSystemAccess.CTR_NAND_RO_WRITE;
			case "CategorySystemSettings":
				return StorageInfo.FileSystemAccess.CATEGORY_SYSTEM_SETTINGS;
			case "CardBoard":
				return StorageInfo.FileSystemAccess.CARD_BOARD;
			case "ExportImportIvs":
				return StorageInfo.FileSystemAccess.EXPORT_IMPORT_IVS;
			case "DirectSdmcWrite":
				return StorageInfo.FileSystemAccess.DIRECT_SDMC_WRITE;
			case "SwitchCleanup":
				return StorageInfo.FileSystemAccess.SWITCH_CLEANUP;
			}
			throw new MakeromException("Inavlid FileSystemAccess list value");
		}
		internal IEnumerable<ARM9AccessControlInfo.Arm9Capability> GetArm9AccessControl()
		{
			this.CheckParameter(this.m_Param.Arm9AccessControl, NyamlOptionStrings.IO_ACCESS_CONTROL_STRING);
			if (this.m_Param.Arm9AccessControl.GetType() == typeof(Sequence) || this.m_Param.Arm9AccessControl.GetType() == typeof(ScalarNull))
			{
				List<ARM9AccessControlInfo.Arm9Capability> list = new List<ARM9AccessControlInfo.Arm9Capability>();
				string[] stringArray = Nyaml.GetStringArray(this.m_Param.Arm9AccessControl);
				for (int i = 0; i < stringArray.Length; i++)
				{
					string s = stringArray[i];
					list.Add(NyamlOption.GetArm9AccessControlNumber(s));
				}
				return list;
			}
			throw new MakeromException("Inavlid Arm9AccessControl list");
		}
		private string MakeInvalidCombinationString(NyamlOption.CardMediaType media, NyamlOption.CardDeviceName device, ulong saveDataSize)
		{
			return string.Format("Invalid Combination. \nMediaType: {0}\nCardDevice: {1}\nSaveDataSize: {2}\n", media, device, saveDataSize);
		}
		public NyamlOption.CardDeviceName GetCardDevice()
		{
			ulong saveDataSize = this.GetSaveDataSize();
			NyamlOption.CardMediaType cardMediaType = this.GetCardMediaType();
			if (this.m_Param.CardDevice == null)
			{
				ScalarString cardDevice = new ScalarString("None");
				ScalarString cardDevice2 = new ScalarString("NorFlash");
				if (cardMediaType == NyamlOption.CardMediaType.CARD2)
				{
					this.m_Param.CardDevice = cardDevice;
				}
				else
				{
					if (saveDataSize == 0uL)
					{
						this.m_Param.CardDevice = cardDevice;
					}
					else
					{
						this.m_Param.CardDevice = cardDevice2;
					}
				}
			}
			string @string = this.m_Param.CardDevice.GetString();
			Dictionary<string, NyamlOption.CardDeviceName> dictionary = new Dictionary<string, NyamlOption.CardDeviceName>
			{

				{
					"NorFlash",
					NyamlOption.CardDeviceName.CARD_DEVICE_NOR_FLASH
				},

				{
					"None",
					NyamlOption.CardDeviceName.CARD_DEVICE_NONE
				},

				{
					"BT",
					NyamlOption.CardDeviceName.CARD_DEVICE_BT
				}
			};
			if (!dictionary.ContainsKey(@string))
			{
				throw new MakeromException(string.Format("Invalid CardDevice \"{0}\"", @string));
			}
			NyamlOption.CardDeviceName cardDeviceName = dictionary[@string];
			string message = this.MakeInvalidCombinationString(cardMediaType, cardDeviceName, saveDataSize);
			if (cardMediaType == NyamlOption.CardMediaType.CARD2 && cardDeviceName == NyamlOption.CardDeviceName.CARD_DEVICE_NOR_FLASH)
			{
				cardDeviceName = NyamlOption.CardDeviceName.CARD_DEVICE_NONE;
				Util.PrintWarning("\"CardDevice: NorFlash\" is invalid on Card2\n");
			}
			if (cardMediaType == NyamlOption.CardMediaType.CARD1)
			{
				if (cardDeviceName == NyamlOption.CardDeviceName.CARD_DEVICE_NONE || cardDeviceName == NyamlOption.CardDeviceName.CARD_DEVICE_BT)
				{
					if (saveDataSize > 0uL)
					{
						Util.PrintWarning(message);
					}
				}
				else
				{
					if (cardDeviceName == NyamlOption.CardDeviceName.CARD_DEVICE_NOR_FLASH && saveDataSize == 0uL)
					{
						Util.PrintWarning(message);
					}
				}
			}
			return cardDeviceName;
		}
		public NyamlOption.CardMediaType GetCardMediaType()
		{
			this.CheckParameter(this.m_Param.CardMediaType, NyamlOptionStrings.CARD_MEDIA_TYPE_STRING);
			Dictionary<string, NyamlOption.CardMediaType> dictionary = new Dictionary<string, NyamlOption.CardMediaType>
			{

				{
					"card1",
					NyamlOption.CardMediaType.CARD1
				},

				{
					"card2",
					NyamlOption.CardMediaType.CARD2
				}
			};
			string key = this.m_Param.CardMediaType.GetString().ToLower();
			if (dictionary.ContainsKey(key))
			{
				return dictionary[key];
			}
			throw new MakeromException(string.Format("Invalid Card MediaType \"{0}\"", this.m_Param.CardMediaType.GetString()));
		}
		private static ARM9AccessControlInfo.Arm9Capability GetArm9AccessControlNumber(string s)
		{
			switch (s)
			{
			case "FsMountNand":
				return ARM9AccessControlInfo.Arm9Capability.FS_MOUNT_NAND;
			case "FsMountNandRoWrite":
				return ARM9AccessControlInfo.Arm9Capability.FS_MOUNT_NAND_RO_WRITE;
			case "FsMountTwln":
				return ARM9AccessControlInfo.Arm9Capability.FS_MOUNT_TWLN;
			case "FsMountWnand":
				return ARM9AccessControlInfo.Arm9Capability.FS_MOUNT_WNAND;
			case "FsMountCardSpi":
				return ARM9AccessControlInfo.Arm9Capability.FS_MOUNT_CARD_SPI;
			case "UseSdif3":
				return ARM9AccessControlInfo.Arm9Capability.USE_SDIF3;
			case "CreateSeed":
				return ARM9AccessControlInfo.Arm9Capability.CREATE_SEED;
			case "UseCardSpi":
				return ARM9AccessControlInfo.Arm9Capability.USE_CARD_SPI;
			}
			throw new MakeromException("Inavlid Arm9AccessControl list value");
		}
		internal byte GetDescVersion()
		{
			return this.m_DescVersion;
		}
		internal byte GetSystemMode()
		{
			this.CheckParameter(this.m_Param.SystemMode, NyamlOptionStrings.SYSTEM_MODE_STRING);
			if (this.m_Param.SystemMode.GetType() != typeof(ScalarInteger))
			{
				throw new MakeromException(string.Format("Invalid SystemMode: {0}", this.m_Param.SystemMode.GetString()));
			}
			byte b = (byte)this.m_Param.SystemMode.GetInteger();
			if (b > 15)
			{
				throw new MakeromException(string.Format("Unexpected SystemMode: {0:x}\nExpected range: 0x0 - 0xf", b));
			}
			return b;
		}
		internal MakeCxiOptions.ResourceLimitCategoryName GetResourceLimitCategory()
		{
			if (this.m_Param.ResourceLimitCategory == null)
			{
				return MakeCxiOptions.ResourceLimitCategoryName.APPLICATION;
			}
			if (this.m_Param.ResourceLimitCategory.GetType() == typeof(ScalarString))
			{
				Dictionary<string, MakeCxiOptions.ResourceLimitCategoryName> dictionary = new Dictionary<string, MakeCxiOptions.ResourceLimitCategoryName>
				{

					{
						"application",
						MakeCxiOptions.ResourceLimitCategoryName.APPLICATION
					},

					{
						"sysapplet",
						MakeCxiOptions.ResourceLimitCategoryName.SYS_APPLET
					},

					{
						"libapplet",
						MakeCxiOptions.ResourceLimitCategoryName.LIB_APPLET
					},

					{
						"other",
						MakeCxiOptions.ResourceLimitCategoryName.OTHER
					}
				};
				string key = this.m_Param.ResourceLimitCategory.GetString().ToLower();
				if (dictionary.ContainsKey(key))
				{
					return dictionary[key];
				}
			}
			throw new MakeromException(string.Format("Invalid ResourceLimitCategory: {0}", this.m_Param.ResourceLimitCategory.GetString()));
		}
		internal byte GetMaxCpu()
		{
			return (byte)this.m_Param.MaxCpu.GetInteger();
		}
		internal uint GetReleaseKernelMajor()
		{
			if (this.m_Param.ReleaseKernelMajor != null)
			{
				return uint.Parse(this.m_Param.ReleaseKernelMajor.GetString());
			}
			return 4294967295u;
		}
		internal uint GetReleaseKernelMinor()
		{
			if (this.m_Param.ReleaseKernelMinor != null)
			{
				return uint.Parse(this.m_Param.ReleaseKernelMinor.GetString());
			}
			return 4294967295u;
		}
		internal string GetCategoryName()
		{
			string text = null;
			if (this.m_Param.TitleCategory.GetType() == typeof(ScalarString))
			{
				text = this.m_Param.TitleCategory.GetString();
				if (text == "")
				{
					text = null;
				}
			}
			return text;
		}
		internal string GetTargetCategoryName()
		{
			string text = null;
			if (this.m_Param.TitleTargetCategory.GetType() == typeof(ScalarString))
			{
				text = this.m_Param.TitleTargetCategory.GetString();
				if (text == "")
				{
					text = null;
				}
			}
			return text;
		}
		private uint GetCategoryFlagsFromCategory(string categoryName)
		{
			List<TitleIdUtil.CategoryFlagsName> list = new List<TitleIdUtil.CategoryFlagsName>();
			Dictionary<string, TitleIdUtil.CategoryMainName> dictionary = new Dictionary<string, TitleIdUtil.CategoryMainName>
			{

				{
					"Application",
					TitleIdUtil.CategoryMainName.Normal
				},

				{
					"SystemApplication",
					TitleIdUtil.CategoryMainName.Normal
				},

				{
					"Applet",
					TitleIdUtil.CategoryMainName.Normal
				},

				{
					"Firmware",
					TitleIdUtil.CategoryMainName.Normal
				},

				{
					"Base",
					TitleIdUtil.CategoryMainName.Normal
				},

				{
					"DlpChild",
					TitleIdUtil.CategoryMainName.DlpChild
				},

				{
					"Demo",
					TitleIdUtil.CategoryMainName.Trial
				},

				{
					"Contents",
					TitleIdUtil.CategoryMainName.Contents
				},

				{
					"SystemContents",
					TitleIdUtil.CategoryMainName.Contents
				},

				{
					"SharedContents",
					TitleIdUtil.CategoryMainName.Contents
				},

				{
					"AddOnContents",
					TitleIdUtil.CategoryMainName.AddOnContents
				},

				{
					"Patch",
					TitleIdUtil.CategoryMainName.Patch
				}
			};
			Dictionary<string, List<TitleIdUtil.CategoryFlagsName>> dictionary2 = new Dictionary<string, List<TitleIdUtil.CategoryFlagsName>>
			{

				{
					"Application",
					new List<TitleIdUtil.CategoryFlagsName>()
				},

				{
					"SystemApplication",
					new List<TitleIdUtil.CategoryFlagsName>
					{
						TitleIdUtil.CategoryFlagsName.System
					}
				},

				{
					"Applet",
					new List<TitleIdUtil.CategoryFlagsName>
					{
						TitleIdUtil.CategoryFlagsName.System,
						TitleIdUtil.CategoryFlagsName.RequireBatchUpdate
					}
				},

				{
					"Firmware",
					new List<TitleIdUtil.CategoryFlagsName>
					{
						TitleIdUtil.CategoryFlagsName.System,
						TitleIdUtil.CategoryFlagsName.RequireBatchUpdate,
						TitleIdUtil.CategoryFlagsName.CannotExecution,
						TitleIdUtil.CategoryFlagsName.CanSkipConvertJumpId
					}
				},

				{
					"Base",
					new List<TitleIdUtil.CategoryFlagsName>
					{
						TitleIdUtil.CategoryFlagsName.System,
						TitleIdUtil.CategoryFlagsName.RequireBatchUpdate,
						TitleIdUtil.CategoryFlagsName.CanSkipConvertJumpId
					}
				},

				{
					"DlpChild",
					new List<TitleIdUtil.CategoryFlagsName>()
				},

				{
					"Demo",
					new List<TitleIdUtil.CategoryFlagsName>()
				},

				{
					"Contents",
					new List<TitleIdUtil.CategoryFlagsName>()
				},

				{
					"SystemContents",
					new List<TitleIdUtil.CategoryFlagsName>
					{
						TitleIdUtil.CategoryFlagsName.CannotExecution,
						TitleIdUtil.CategoryFlagsName.System
					}
				},

				{
					"SharedContents",
					new List<TitleIdUtil.CategoryFlagsName>
					{
						TitleIdUtil.CategoryFlagsName.CannotExecution,
						TitleIdUtil.CategoryFlagsName.System,
						TitleIdUtil.CategoryFlagsName.NotRequireRightForMount
					}
				},

				{
					"AddOnContents",
					new List<TitleIdUtil.CategoryFlagsName>
					{
						TitleIdUtil.CategoryFlagsName.CannotExecution,
						TitleIdUtil.CategoryFlagsName.NotRequireRightForMount
					}
				},

				{
					"Patch",
					new List<TitleIdUtil.CategoryFlagsName>
					{
						TitleIdUtil.CategoryFlagsName.CannotExecution
					}
				}
			};
			if (dictionary.ContainsKey(categoryName) && dictionary2.ContainsKey(categoryName))
			{
				TitleIdUtil.CategoryMainName mainCategory = dictionary[categoryName];
				list = dictionary2[categoryName];
				return TitleIdUtil.MakeCategory(mainCategory, list.ToArray());
			}
			throw new MakeromException(string.Format("Invalid Category name: {0}", categoryName));
		}
		private uint GetCategoryFlagsImpl(Sequence categoryFlags)
		{
			Dictionary<string, TitleIdUtil.CategoryMainName> dictionary = new Dictionary<string, TitleIdUtil.CategoryMainName>
			{

				{
					"Normal",
					TitleIdUtil.CategoryMainName.Normal
				},

				{
					"DlpChild",
					TitleIdUtil.CategoryMainName.DlpChild
				},

				{
					"Demo",
					TitleIdUtil.CategoryMainName.Trial
				},

				{
					"Contents",
					TitleIdUtil.CategoryMainName.Contents
				},

				{
					"AddOnContents",
					TitleIdUtil.CategoryMainName.AddOnContents
				},

				{
					"Patch",
					TitleIdUtil.CategoryMainName.Patch
				}
			};
			Dictionary<string, TitleIdUtil.CategoryFlagsName> dictionary2 = new Dictionary<string, TitleIdUtil.CategoryFlagsName>
			{

				{
					"CannotExecution",
					TitleIdUtil.CategoryFlagsName.CannotExecution
				},

				{
					"System",
					TitleIdUtil.CategoryFlagsName.System
				},

				{
					"RequireBatchUpdate",
					TitleIdUtil.CategoryFlagsName.RequireBatchUpdate
				},

				{
					"NotRequireUserApproval",
					TitleIdUtil.CategoryFlagsName.NotRequireUserApproval
				},

				{
					"NotRequireRightForMount",
					TitleIdUtil.CategoryFlagsName.NotRequireRightForMount
				},

				{
					"CanSkipConvertJumpId",
					TitleIdUtil.CategoryFlagsName.CanSkipConvertJumpId
				}
			};
			TitleIdUtil.CategoryMainName categoryMainName = TitleIdUtil.CategoryMainName.None;
			List<TitleIdUtil.CategoryFlagsName> list = new List<TitleIdUtil.CategoryFlagsName>();
			foreach (CollectionElement current in categoryFlags)
			{
				if (current.GetType() != typeof(ScalarString))
				{
					throw new MakeromException(string.Format("Invalid CategoryFlags: {0}", current.ToString()));
				}
				string @string = current.GetString();
				if (dictionary.ContainsKey(@string))
				{
					if (categoryMainName != TitleIdUtil.CategoryMainName.None)
					{
						throw new MakeromException(string.Format("Failed to set \"{0}\" for categroy. category is already set.", @string));
					}
					categoryMainName = dictionary[@string];
				}
				else
				{
					if (!dictionary2.ContainsKey(@string))
					{
						throw new MakeromException(string.Format("Invalid CategoryFlags: {0}", @string));
					}
					list.Add(dictionary2[@string]);
				}
			}
			if (categoryMainName == TitleIdUtil.CategoryMainName.None)
			{
				categoryMainName = TitleIdUtil.CategoryMainName.Normal;
			}
			return TitleIdUtil.MakeCategory(categoryMainName, list.ToArray());
		}
		internal uint GetTargetCategoryFlags()
		{
			string targetCategoryName = this.GetTargetCategoryName();
			if (targetCategoryName == null)
			{
				return this.GetCategoryFlags();
			}
			return this.GetCategoryFlagsFromCategory(targetCategoryName);
		}
		internal uint GetCategoryFlags()
		{
			Sequence sequence = null;
			string categoryName = this.GetCategoryName();
			if (this.m_Param.CategoryFlags.GetType() == typeof(Sequence))
			{
				sequence = (Sequence)this.m_Param.CategoryFlags;
			}
			if (sequence == null && categoryName == null)
			{
				return 0u;
			}
			if (sequence != null && categoryName != null)
			{
				throw new MakeromException("Can not set Cateory and CategoryFlags at the same time.");
			}
			if (sequence != null)
			{
				return this.GetCategoryFlagsImpl(sequence);
			}
			return this.GetCategoryFlagsFromCategory(categoryName);
		}
	}
}
