using Nintendo.MakeRom.Properties;
using nyaml;
using System;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	public class NyamlDescParameter
	{
		public CollectionElement AccessControlDescriptor
		{
			get;
			private set;
		}
		public CollectionElement ProgramIdDesc
		{
			get;
			private set;
		}
		public CollectionElement PriorityDesc
		{
			get;
			private set;
		}
		public CollectionElement AffinityMaskDesc
		{
			get;
			private set;
		}
		public CollectionElement IdealProcessorDesc
		{
			get;
			private set;
		}
		public CollectionElement FirmwareVersionDesc
		{
			get;
			private set;
		}
		public CollectionElement StorageIdDesc
		{
			get;
			private set;
		}
		public CollectionElement ServiceAccessControlDesc
		{
			get;
			private set;
		}
		public CollectionElement SystemCallAccessControlDesc
		{
			get;
			private set;
		}
		public CollectionElement InterruptNumbersDesc
		{
			get;
			private set;
		}
		public CollectionElement MemoryMappingDesc
		{
			get;
			private set;
		}
		public CollectionElement IORegisterMappingDesc
		{
			get;
			private set;
		}
		public CollectionElement Arm9AccessControlDesc
		{
			get;
			private set;
		}
		public CollectionElement HandleTableSizeDesc
		{
			get;
			private set;
		}
		public CollectionElement MemoryTypeDesc
		{
			get;
			private set;
		}
		public CollectionElement DescVersionDesc
		{
			get;
			private set;
		}
		public CollectionElement SystemModeDesc
		{
			get;
			private set;
		}
		public CollectionElement RunnableOnSleepDesc
		{
			get;
			private set;
		}
		public CollectionElement SpecialMemoryArrange
		{
			get;
			private set;
		}
		public CollectionElement EnableInterruptNumbers
		{
			get;
			private set;
		}
		public CollectionElement EnableSystemCalls
		{
			get;
			private set;
		}
		public CollectionElement AccCtlDescSign
		{
			get;
			private set;
		}
		public CollectionElement AccCtlDescBin
		{
			get;
			private set;
		}
		public CollectionElement CommonHeaderKey
		{
			get;
			private set;
		}
		public CollectionElement KeyP
		{
			get;
			private set;
		}
		public CollectionElement KeyQ
		{
			get;
			private set;
		}
		public CollectionElement KeyD
		{
			get;
			private set;
		}
		public CollectionElement KeyDP
		{
			get;
			private set;
		}
		public CollectionElement KeyDQ
		{
			get;
			private set;
		}
		public CollectionElement KeyInverseQ
		{
			get;
			private set;
		}
		public CollectionElement KeyModulus
		{
			get;
			private set;
		}
		public CollectionElement KeyExponent
		{
			get;
			private set;
		}
		public CollectionElement AutoGen
		{
			get;
			private set;
		}
		public CollectionElement CryptoKey
		{
			get;
			private set;
		}
		public CollectionElement ResourceLimitCategory
		{
			get;
			private set;
		}
		public CollectionElement ReleaseKernelMajor
		{
			get;
			private set;
		}
		public CollectionElement ReleaseKernelMinor
		{
			get;
			private set;
		}
		public NyamlDescParameter(Nyaml nyaml)
		{
			this.CheckValidDesc(nyaml);
			this.SetAccessControlDescriptor(nyaml);
			this.SetCommonHeaderKey(nyaml);
		}
		private void CheckValidDesc(Nyaml nyaml)
		{
			Nyaml nyaml2 = Nyaml.LoadFromText(Resources.Desc_Template);
			string notIncludedKey = Nyaml.GetNotIncludedKey(nyaml2.Collection, nyaml.Collection);
			if (notIncludedKey != null)
			{
				throw new UnknownParameterException(notIncludedKey);
			}
		}
		private void SetCommonHeaderKey(Nyaml nyaml)
		{
			this.CommonHeaderKey = nyaml.Collection.GetCollectionElement("CommonHeaderKey");
			if (this.CommonHeaderKey == null || this.CommonHeaderKey.IsNullScalar)
			{
				throw new ParameterNotFoundException("CommonHeaderKey");
			}
			this.KeyD = this.CommonHeaderKey.GetCollectionElement("D");
			this.KeyP = this.CommonHeaderKey.GetCollectionElement("P");
			this.KeyQ = this.CommonHeaderKey.GetCollectionElement("Q");
			this.KeyDP = this.CommonHeaderKey.GetCollectionElement("DP");
			this.KeyDQ = this.CommonHeaderKey.GetCollectionElement("DQ");
			this.KeyInverseQ = this.CommonHeaderKey.GetCollectionElement("InverseQ");
			this.KeyModulus = this.CommonHeaderKey.GetCollectionElement("Modulus");
			this.KeyExponent = this.CommonHeaderKey.GetCollectionElement("Exponent");
		}
		private void SetDefaultCommonHeaderKey()
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.ExportParameters(true);
		}
		private void SetAccessControlDescriptor(Nyaml nyaml)
		{
			this.AccessControlDescriptor = nyaml.Collection.GetCollectionElement("AccessControlDescriptor");
			if (this.AccessControlDescriptor == null || this.AccessControlDescriptor.IsNullScalar)
			{
				throw new ParameterNotFoundException("AccessControlDescriptor");
			}
			this.ProgramIdDesc = this.AccessControlDescriptor.GetCollectionElement("ProgramId");
			this.PriorityDesc = this.AccessControlDescriptor.GetCollectionElement("Priority");
			this.AffinityMaskDesc = this.AccessControlDescriptor.GetCollectionElement("AffinityMask");
			this.IdealProcessorDesc = this.AccessControlDescriptor.GetCollectionElement("IdealProcessor");
			this.FirmwareVersionDesc = this.AccessControlDescriptor.GetCollectionElement("FirmwareVersion");
			this.StorageIdDesc = this.AccessControlDescriptor.GetCollectionElement("StorageId");
			this.ServiceAccessControlDesc = this.AccessControlDescriptor.GetCollectionElement("ServiceAccessControl");
			this.MemoryMappingDesc = this.AccessControlDescriptor.GetCollectionElement("MemoryMapping");
			this.IORegisterMappingDesc = this.AccessControlDescriptor.GetCollectionElement("IORegisterMapping");
			this.Arm9AccessControlDesc = this.AccessControlDescriptor.GetCollectionElement("Arm9AccessControl");
			this.EnableInterruptNumbers = this.AccessControlDescriptor.GetCollectionElement("EnableInterruptNumbers");
			this.EnableSystemCalls = this.AccessControlDescriptor.GetCollectionElement("EnableSystemCalls");
			this.HandleTableSizeDesc = this.AccessControlDescriptor.GetCollectionElement("HandleTableSize");
			this.AutoGen = this.AccessControlDescriptor.GetCollectionElement("AutoGen");
			this.MemoryTypeDesc = this.AccessControlDescriptor.GetCollectionElement("MemoryType");
			this.DescVersionDesc = this.AccessControlDescriptor.GetCollectionElement("DescVersion");
			this.SystemModeDesc = this.AccessControlDescriptor.GetCollectionElement("SystemMode");
			this.RunnableOnSleepDesc = this.AccessControlDescriptor.GetCollectionElement("RunnableOnSleep");
			this.SpecialMemoryArrange = this.AccessControlDescriptor.GetCollectionElement("SpecialMemoryArrange");
			this.AccCtlDescSign = this.AccessControlDescriptor.GetCollectionElement("Signature");
			this.AccCtlDescBin = this.AccessControlDescriptor.GetCollectionElement("Descriptor");
			this.CryptoKey = this.AccessControlDescriptor.GetCollectionElement("CryptoKey");
			this.ResourceLimitCategory = this.AccessControlDescriptor.GetCollectionElement("ResourceLimitCategory");
			this.ReleaseKernelMajor = this.AccessControlDescriptor.GetCollectionElement("ReleaseKernelMajor");
			this.ReleaseKernelMinor = this.AccessControlDescriptor.GetCollectionElement("ReleaseKernelMinor");
			if (this.EnableSystemCalls.GetType() == typeof(ScalarNull))
			{
				this.EnableSystemCalls = new Mapping();
			}
		}
	}
}
