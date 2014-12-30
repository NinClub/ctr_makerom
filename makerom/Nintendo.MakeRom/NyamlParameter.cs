using Nintendo.MakeRom.Properties;
using nyaml;
using System;
using System.Reflection;
namespace Nintendo.MakeRom
{
	public class NyamlParameter : ICloneable
	{
		private Nyaml m_Nyaml;
		public CollectionElement HostRoot
		{
			get;
			private set;
		}
		public CollectionElement CompanyCode
		{
			get;
			private set;
		}
		public CollectionElement CanUseNonAlphabetAndNumber
		{
			get;
			private set;
		}
		public CollectionElement CanUsePrivilegePriority
		{
			get;
			private set;
		}
		public CollectionElement CanWriteSharedPage
		{
			get;
			private set;
		}
		public CollectionElement EnableForceDebug
		{
			get;
			private set;
		}
		public CollectionElement DisableDebug
		{
			get;
			private set;
		}
		public CollectionElement PermitMainFunctionArgument
		{
			get;
			private set;
		}
		public CollectionElement CanShareDeviceMemory
		{
			get;
			private set;
		}
		public CollectionElement StorageId
		{
			get;
			private set;
		}
		public CollectionElement SystemSaveDataId1
		{
			get;
			private set;
		}
		public CollectionElement SystemSaveDataId2
		{
			get;
			private set;
		}
		public CollectionElement OtherUserSaveDataId1
		{
			get;
			private set;
		}
		public CollectionElement OtherUserSaveDataId2
		{
			get;
			private set;
		}
		public CollectionElement OtherUserSaveDataId3
		{
			get;
			private set;
		}
		public CollectionElement UseOtherVariationSaveData
		{
			get;
			private set;
		}
		public CollectionElement IORegisterMapping
		{
			get;
			private set;
		}
		public CollectionElement MemoryMapping
		{
			get;
			private set;
		}
		public CollectionElement ProgramId
		{
			get;
			private set;
		}
		public CollectionElement DependencyList
		{
			get;
			private set;
		}
		public CollectionElement AffinityMask
		{
			get;
			private set;
		}
		public CollectionElement IdealProcessor
		{
			get;
			private set;
		}
		public CollectionElement MainThreadPriority
		{
			get;
			private set;
		}
		public CollectionElement MainThreadStackSize
		{
			get;
			private set;
		}
		public CollectionElement ProcessType
		{
			get;
			private set;
		}
		public CollectionElement AppendSystemCalls
		{
			get;
			private set;
		}
		public CollectionElement InterruptNumbers
		{
			get;
			private set;
		}
		public CollectionElement Title
		{
			get;
			private set;
		}
		public CollectionElement PageSize
		{
			get;
			private set;
		}
		public CollectionElement AllowUnalignedSection
		{
			get;
			private set;
		}
		public CollectionElement RemasterVersion
		{
			get;
			private set;
		}
		public CollectionElement NoPadding
		{
			get;
			private set;
		}
		public CollectionElement UseAes
		{
			get;
			private set;
		}
		public CollectionElement MediaType
		{
			get;
			private set;
		}
		public CollectionElement MediaSize
		{
			get;
			private set;
		}
		public CollectionElement MediaFootPadding
		{
			get;
			private set;
		}
		public CollectionElement Padding
		{
			get;
			private set;
		}
		public CollectionElement Logo
		{
			get;
			private set;
		}
		public CollectionElement RomFsRoot
		{
			get;
			private set;
		}
		public CollectionElement Reject
		{
			get;
			private set;
		}
		public CollectionElement Include
		{
			get;
			private set;
		}
		public CollectionElement DefaultReject
		{
			get;
			private set;
		}
		public CollectionElement Files
		{
			get;
			private set;
		}
		public CollectionElement ExeFs
		{
			get;
			private set;
		}
		public CollectionElement PlainRegion
		{
			get;
			private set;
		}
		public CollectionElement FirmwareVersion
		{
			get;
			private set;
		}
		public CollectionElement CoreVersion
		{
			get;
			private set;
		}
		public CollectionElement TitleInfo
		{
			get;
			private set;
		}
		public CollectionElement TitleUniqueId
		{
			get;
			private set;
		}
		public CollectionElement TitleVersion
		{
			get;
			private set;
		}
		public CollectionElement TitleContentsIndex
		{
			get;
			private set;
		}
		public CollectionElement TitleChildIndex
		{
			get;
			private set;
		}
		public CollectionElement TitleDataTitleIndex
		{
			get;
			private set;
		}
		public CollectionElement TitleDemoIndex
		{
			get;
			private set;
		}
		public CollectionElement TitlePlatform
		{
			get;
			private set;
		}
		public CollectionElement TitleTargetCategory
		{
			get;
			private set;
		}
		public CollectionElement ProductCode
		{
			get;
			private set;
		}
		public CollectionElement CardType
		{
			get;
			private set;
		}
		public CollectionElement CardCryptoType
		{
			get;
			private set;
		}
		public CollectionElement CardWritableAddress
		{
			get;
			private set;
		}
		public CollectionElement ContentType
		{
			get;
			private set;
		}
		public CollectionElement TitleUse
		{
			get;
			private set;
		}
		public CollectionElement FreeProductCode
		{
			get;
			private set;
		}
		public CollectionElement SaveDataSize
		{
			get;
			private set;
		}
		public CollectionElement EnableCompress
		{
			get;
			private set;
		}
		public CollectionElement JumpId
		{
			get;
			private set;
		}
		public CollectionElement UseExtSaveData
		{
			get;
			private set;
		}
		public CollectionElement ExtSaveDataId
		{
			get;
			private set;
		}
		public CollectionElement ExtSaveDataNumber
		{
			get;
			private set;
		}
		public CollectionElement FileSystemAccess
		{
			get;
			private set;
		}
		public CollectionElement BackupMemoryType
		{
			get;
			private set;
		}
		public CollectionElement Arm9AccessControl
		{
			get;
			private set;
		}
		public CollectionElement CardDevice
		{
			get;
			set;
		}
		public CollectionElement CardMediaType
		{
			get;
			private set;
		}
		public CollectionElement DefaultSystemCalls
		{
			get;
			set;
		}
		public CollectionElement ServiceAccessControl
		{
			get;
			set;
		}
		public CollectionElement HandleTableSize
		{
			get;
			set;
		}
		public CollectionElement MemoryType
		{
			get;
			set;
		}
		public CollectionElement SystemMode
		{
			get;
			set;
		}
		public CollectionElement ResourceLimitCategory
		{
			get;
			set;
		}
		public CollectionElement RunnableOnSleep
		{
			get;
			set;
		}
		public CollectionElement SpecialMemoryArrange
		{
			get;
			set;
		}
		public CollectionElement MaxCpu
		{
			get;
			set;
		}
		public CollectionElement CategoryFlags
		{
			get;
			set;
		}
		public CollectionElement TitleCategory
		{
			get;
			set;
		}
		public CollectionElement ReleaseKernelMajor
		{
			get;
			set;
		}
		public CollectionElement ReleaseKernelMinor
		{
			get;
			set;
		}
		public CollectionElement AccessControlInfo
		{
			get;
			private set;
		}
		public CollectionElement SystemControlInfo
		{
			get;
			private set;
		}
		public CollectionElement BasicInfo
		{
			get;
			private set;
		}
		public CollectionElement Rom
		{
			get;
			private set;
		}
		public CollectionElement Option
		{
			get;
			private set;
		}
		public CollectionElement CardInfo
		{
			get;
			private set;
		}
		public NyamlParameter(Nyaml nyaml)
		{
			this.m_Nyaml = nyaml;
			this.CheckValidParameters();
			this.SetAccessControlInfo(nyaml);
			this.SetSystemControlInfo(nyaml);
			this.SetBasicInfo(nyaml);
			this.SetRom(nyaml);
			this.SetOption(nyaml);
			this.SetExeFs(nyaml);
			this.SetPlainRegion(nyaml);
			this.SetTitleInfo(nyaml);
			this.SetCardInfo(nyaml);
		}
		private void CheckValidParameters()
		{
			Nyaml nyaml = Nyaml.LoadFromText(Resources.Rsf_Template);
			string notIncludedKey = Nyaml.GetNotIncludedKey(nyaml.Collection, this.m_Nyaml.Collection);
			if (notIncludedKey != null)
			{
				throw new UnknownParameterException(notIncludedKey);
			}
		}
		public void Merge(NyamlParameter other)
		{
			if (other == null)
			{
				return;
			}
			Type type = base.GetType();
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if ((CollectionElement)getMethod.Invoke(this, null) == null)
				{
					MethodInfo setMethod = propertyInfo.GetSetMethod(true);
					setMethod.Invoke(this, new object[]
					{
						getMethod.Invoke(other, null)
					});
				}
			}
		}
		private void SetOption(Nyaml nyaml)
		{
			CollectionElement collection = nyaml.Collection;
			this.Option = collection.GetCollectionElement(NyamlOptionStrings.OPTION_STRING);
			this.AppendSystemCalls = collection.GetCollectionElement(NyamlOptionStrings.APPEND_SYSTEM_CALL_STRING);
			this.PageSize = collection.GetCollectionElement(NyamlOptionStrings.PAGE_SIZE_STRING);
			this.AllowUnalignedSection = collection.GetCollectionElement(NyamlOptionStrings.ALLOW_UNALIGNED_SECTION_STRING);
			this.NoPadding = collection.GetCollectionElement(NyamlOptionStrings.NO_PADDING_STRING);
			this.UseAes = collection.GetCollectionElement(NyamlOptionStrings.USE_AES_STRING);
			this.FreeProductCode = collection.GetCollectionElement(NyamlOptionStrings.FREE_PRODUCT_CODE_STRING);
			this.EnableCompress = collection.GetCollectionElement(NyamlOptionStrings.ENABLE_COMPRESS_STRING);
		}
		private void SetTitleInfo(Nyaml nyaml)
		{
			CollectionElement collection = nyaml.Collection;
			this.TitleInfo = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_STRING);
			this.TitleCategory = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_CATEGORY);
			this.TitleUniqueId = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_UNIQUE_ID);
			this.TitleVersion = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_VERSION);
			this.TitleContentsIndex = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_CONTENTS_INDEX);
			this.TitlePlatform = collection.GetCollectionElement(NyamlOptionStrings.TITLE_PLATFORM);
			this.TitleUse = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_USE);
			this.TitleChildIndex = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_CHILD_INDEX);
			this.TitleDataTitleIndex = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_DATA_TITLE_INDEX);
			this.TitleDemoIndex = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_DEMO_INDEX);
			this.CategoryFlags = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_CATEGORY_FLAGS);
			this.TitleTargetCategory = collection.GetCollectionElement(NyamlOptionStrings.TITLE_INFO_TARGET_CATEGORY);
		}
		private void SetAccessControlInfo(Nyaml nyaml)
		{
			CollectionElement collection = nyaml.Collection;
			this.AccessControlInfo = collection.GetCollectionElement(NyamlOptionStrings.ACCESS_CONTROL_INFO_STRING);
			this.MainThreadPriority = collection.GetCollectionElement(NyamlOptionStrings.PRIORITY_STRING);
			this.AffinityMask = collection.GetCollectionElement(NyamlOptionStrings.AFFINITY_MASK_STRING);
			this.IdealProcessor = collection.GetCollectionElement(NyamlOptionStrings.IDEAL_PROCESSOR_STRING);
			this.ServiceAccessControl = collection.GetCollectionElement(NyamlOptionStrings.SERVICE_ACCESS_CONTROL_STRING);
			this.ProgramId = collection.GetCollectionElement(NyamlOptionStrings.PROGRAM_ID_STRING);
			this.StorageId = collection.GetCollectionElement(NyamlOptionStrings.STORAGE_ID_STRING);
			this.SystemSaveDataId1 = collection.GetCollectionElement(NyamlOptionStrings.SYSTEM_SAVEDATA_ID_STRING1);
			this.SystemSaveDataId2 = collection.GetCollectionElement(NyamlOptionStrings.SYSTEM_SAVEDATA_ID_STRING2);
			this.OtherUserSaveDataId1 = collection.GetCollectionElement(NyamlOptionStrings.OTHER_USER_SAVEDATA_ID_STRING1);
			this.OtherUserSaveDataId2 = collection.GetCollectionElement(NyamlOptionStrings.OTHER_USER_SAVEDATA_ID_STRING2);
			this.OtherUserSaveDataId3 = collection.GetCollectionElement(NyamlOptionStrings.OTHER_USER_SAVEDATA_ID_STRING3);
			this.UseOtherVariationSaveData = collection.GetCollectionElement(NyamlOptionStrings.USE_OTHER_VARIATION_SAVE_DATA_STRING);
			this.MemoryMapping = collection.GetCollectionElement(NyamlOptionStrings.MEMORY_MAPPING_STRING);
			this.IORegisterMapping = collection.GetCollectionElement(NyamlOptionStrings.IO_REGISTER_MAPPING_STRING);
			this.EnableForceDebug = collection.GetCollectionElement(NyamlOptionStrings.ENABLE_FORCE_DEBUG_STRING);
			this.DisableDebug = collection.GetCollectionElement(NyamlOptionStrings.DISABLE_DEBUG_STRING);
			this.CanUseNonAlphabetAndNumber = collection.GetCollectionElement(NyamlOptionStrings.CAN_USE_NON_ALPHABET_STRING);
			this.CanUsePrivilegePriority = collection.GetCollectionElement(NyamlOptionStrings.CAN_USE_PRIVILEGE_PRIORITY_STRING);
			this.CanWriteSharedPage = collection.GetCollectionElement(NyamlOptionStrings.CAN_WRITE_SHARED_PAGE_STRING);
			this.PermitMainFunctionArgument = collection.GetCollectionElement(NyamlOptionStrings.PERMIT_MAIN_FUNCTION_ARGUMENT);
			this.CanShareDeviceMemory = collection.GetCollectionElement(NyamlOptionStrings.CAN_SHARE_DEVICE_MEMORY_STRING);
			this.MemoryType = collection.GetCollectionElement(NyamlOptionStrings.MEMORY_TYPE_STRING);
			this.DefaultSystemCalls = collection.GetCollectionElement(NyamlOptionStrings.SYSTEM_CALL_ACCESS_STRING);
			this.InterruptNumbers = collection.GetCollectionElement(NyamlOptionStrings.INTERRUPT_NUMBERS_STRING);
			this.HandleTableSize = collection.GetCollectionElement(NyamlOptionStrings.HANDLE_TABLE_SIZE_STRING);
			this.FirmwareVersion = collection.GetCollectionElement(NyamlOptionStrings.FIRMWARE_VERSION_STRING);
			this.CoreVersion = collection.GetCollectionElement(NyamlOptionStrings.CORE_VERSION_STRING);
			this.UseExtSaveData = collection.GetCollectionElement(NyamlOptionStrings.USE_EXT_SAVE_DATA_STRING);
			this.ExtSaveDataId = collection.GetCollectionElement(NyamlOptionStrings.EXT_SAVE_DATA_ID_STRING);
			this.ExtSaveDataNumber = collection.GetCollectionElement(NyamlOptionStrings.EXT_SAVE_DATA_NUMBER_STRING);
			this.SystemMode = collection.GetCollectionElement(NyamlOptionStrings.SYSTEM_MODE_STRING);
			this.FileSystemAccess = collection.GetCollectionElement(NyamlOptionStrings.FILE_SYSTEM_ACCESS_STRING);
			this.Arm9AccessControl = collection.GetCollectionElement(NyamlOptionStrings.IO_ACCESS_CONTROL_STRING);
			this.RunnableOnSleep = collection.GetCollectionElement(NyamlOptionStrings.RUNNABLE_ON_SLEEP_STRING);
			this.SpecialMemoryArrange = collection.GetCollectionElement(NyamlOptionStrings.SPECIAL_MEMORY_ARRANGE_STRING);
		}
		private void SetSystemControlInfo(Nyaml nyaml)
		{
			CollectionElement collection = nyaml.Collection;
			this.SystemControlInfo = collection.GetCollectionElement(NyamlOptionStrings.SYSTEM_CONTROL_INFO_STRING);
			this.ProcessType = collection.GetCollectionElement(NyamlOptionStrings.APP_TYPE_STRING);
			this.MainThreadStackSize = collection.GetCollectionElement(NyamlOptionStrings.STACK_SIZE_STRING);
			this.DependencyList = collection.GetCollectionElement(NyamlOptionStrings.DEPENDENCY_LIST_STRING);
			this.RemasterVersion = collection.GetCollectionElement(NyamlOptionStrings.REMASTER_VERSION_STRING);
			this.JumpId = collection.GetCollectionElement(NyamlOptionStrings.JUMPID_STRING);
		}
		private void SetBasicInfo(Nyaml nyaml)
		{
			CollectionElement collection = nyaml.Collection;
			this.BasicInfo = collection.GetCollectionElement(NyamlOptionStrings.BASIC_INFO_STRING);
			this.Title = collection.GetCollectionElement(NyamlOptionStrings.TITLE_STRING);
			this.CompanyCode = collection.GetCollectionElement(NyamlOptionStrings.COMPANY_CODE_STRING);
			this.MediaSize = collection.GetCollectionElement(NyamlOptionStrings.MEDIA_SIZE_STRING);
			this.MediaFootPadding = collection.GetCollectionElement(NyamlOptionStrings.MEDIA_FOOT_PADDING_STRING);
			this.ProductCode = collection.GetCollectionElement(NyamlOptionStrings.PRODUCT_CODE_STRING);
			this.ContentType = collection.GetCollectionElement(NyamlOptionStrings.CONTENT_TYPE_STRING);
			this.Logo = collection.GetCollectionElement(NyamlOptionStrings.LOGO_STRING);
			this.BackupMemoryType = collection.GetCollectionElement(NyamlOptionStrings.BACKUP_MEMORY_TYPE_STRING);
		}
		private void SetRom(Nyaml nyaml)
		{
			CollectionElement collection = nyaml.Collection;
			this.Rom = collection.GetCollectionElement(NyamlOptionStrings.ROM_STRING);
			this.HostRoot = collection.GetCollectionElement(NyamlOptionStrings.HOST_ROOT_STRING);
			this.Reject = collection.GetCollectionElement(NyamlOptionStrings.REJECT_STRING);
			this.Include = collection.GetCollectionElement(NyamlOptionStrings.INCLUDE_STRING);
			this.DefaultReject = collection.GetCollectionElement(NyamlOptionStrings.DEFAULT_REJECT_STRING);
			this.Files = collection.GetCollectionElement(NyamlOptionStrings.FILE_STRING);
			this.Padding = collection.GetCollectionElement(NyamlOptionStrings.PADDING_STRING);
			this.SaveDataSize = collection.GetCollectionElement(NyamlOptionStrings.SAVE_DATA_SIZE_STRING);
		}
		private void SetExeFs(Nyaml nyaml)
		{
			CollectionElement collection = nyaml.Collection;
			this.ExeFs = collection.GetCollectionElement(NyamlOptionStrings.EXEFS_STRING);
		}
		private void SetPlainRegion(Nyaml nyaml)
		{
			CollectionElement collection = nyaml.Collection;
			this.PlainRegion = collection.GetCollectionElement(NyamlOptionStrings.PLAIN_REGION_STRING);
		}
		private void SetCardInfo(Nyaml nyaml)
		{
			CollectionElement collection = nyaml.Collection;
			this.CardInfo = collection.GetCollectionElement(NyamlOptionStrings.CARD_INFO_STRING);
			this.CardType = collection.GetCollectionElement(NyamlOptionStrings.CARD_TYPE_STRING);
			this.CardCryptoType = collection.GetCollectionElement(NyamlOptionStrings.CARD_CRYPTO_TYPE_STRING);
			this.CardWritableAddress = collection.GetCollectionElement(NyamlOptionStrings.CARD_WRITABLE_ADDRESS_STRING);
			this.CardDevice = collection.GetCollectionElement(NyamlOptionStrings.CARD_CARD_DEVICE_STRING);
			this.CardMediaType = collection.GetCollectionElement(NyamlOptionStrings.CARD_MEDIA_TYPE_STRING);
		}
		public object Clone()
		{
			return new NyamlParameter(this.m_Nyaml);
		}
	}
}
