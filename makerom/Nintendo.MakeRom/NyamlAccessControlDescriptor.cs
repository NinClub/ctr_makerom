using nyaml;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	public class NyamlAccessControlDescriptor
	{
		private const int MAX_CPU_INDEX = 16;
		private NyamlDescParameter m_Param;
		public NyamlAccessControlDescriptor(NyamlDescParameter param)
		{
			this.m_Param = param;
		}
		internal void CheckProgramId(UInt64ProgramId programId)
		{
			if (this.m_Param.ProgramIdDesc != null)
			{
				ulong @long = (ulong)this.m_Param.ProgramIdDesc.GetLong();
				for (int i = 0; i < 8; i++)
				{
					byte b = (byte)(@long >> i * 8 & 255uL);
					byte b2 = (byte)(programId >> i * 8 & 255uL);
					if (b != 255 && b != b2)
					{
						throw new NotPermittedValueException("ProgramId", string.Format("{0,0:X16}", programId.Data));
					}
				}
				return;
			}
			throw new NotPermittedValueException("ProgramId", string.Format("{0,0:X16}", programId.Data));
		}
		private void CheckRangeParameter(Range src, CollectionElement whiteList, string name)
		{
			if (whiteList != null)
			{
				string[] stringArray = Nyaml.GetStringArray(whiteList);
				string[] array = stringArray;
				for (int i = 0; i < array.Length; i++)
				{
					string value = array[i];
					Range range = new Range(value);
					if (range.IsInclude(src))
					{
						return;
					}
				}
			}
			throw new NotPermittedValueException(name, src.ToString());
		}
		private void CheckRangeParameter(uint value, CollectionElement whiteList, string name)
		{
			if (whiteList != null)
			{
				string[] stringArray = Nyaml.GetStringArray(whiteList);
				string[] array = stringArray;
				for (int i = 0; i < array.Length; i++)
				{
					string value2 = array[i];
					Range range = new Range(value2);
					if (range.IsInclude(value))
					{
						return;
					}
				}
			}
			throw new NotPermittedValueException(name, value.ToString());
		}
		private void CheckStringParameter(string value, CollectionElement whiteList, string name)
		{
			if (whiteList != null)
			{
				string[] stringArray = Nyaml.GetStringArray(whiteList);
				string[] array = stringArray;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (text.Equals(value))
					{
						return;
					}
				}
			}
			throw new NotPermittedValueException(name, value);
		}
		internal void CheckMemoryType(MakeCxiOptions.MemoryTypeName memoryType)
		{
			if (this.m_Param.MemoryTypeDesc != null)
			{
				MakeCxiOptions.MemoryTypeName memoryTypeName = NyamlOption.StringToMemoryTypeName(this.m_Param.MemoryTypeDesc.GetString());
				if (memoryTypeName == memoryType)
				{
					return;
				}
			}
			throw new NotPermittedValueException("MemoryType", memoryType.ToString());
		}
		public void CheckPriority(sbyte priority)
		{
			if (this.m_Param.PriorityDesc != null)
			{
				int integer = this.m_Param.PriorityDesc.GetInteger();
				if ((int)priority >= integer && priority <= 127)
				{
					return;
				}
			}
			throw new NotPermittedValueException("Priority", priority.ToString());
		}
		internal void CheckAffinityMask(AffinityMask affinityMask)
		{
			if (this.m_Param.AffinityMaskDesc != null)
			{
				byte b = (byte)this.m_Param.AffinityMaskDesc.GetInteger();
				if ((affinityMask.Data & ~b) == 0)
				{
					return;
				}
			}
			throw new NotPermittedValueException("AffinityMask", affinityMask.Data.ToString());
		}
		public void CheckIdealProcessor(sbyte idealProcessor)
		{
			if (this.m_Param.IdealProcessorDesc != null)
			{
				byte b = (byte)this.m_Param.IdealProcessorDesc.GetInteger();
				if ((1 << (int)idealProcessor & (int)b) != 0)
				{
					return;
				}
			}
			throw new NotPermittedValueException("IdealProcessor", idealProcessor.ToString());
		}
		public void CheckFirmwareVersion(byte firmwareVersion)
		{
			if (this.m_Param.FirmwareVersionDesc != null)
			{
				byte b = (byte)this.m_Param.FirmwareVersionDesc.GetInteger();
				if (firmwareVersion <= b)
				{
					return;
				}
			}
			throw new NotPermittedValueException("FirmwareVersion", firmwareVersion.ToString());
		}
		public void CheckStorageId(string storageId)
		{
			this.CheckStringParameter(storageId, this.m_Param.StorageIdDesc, "StorageId");
		}
		public void CheckServiceAccessControl(string[] serviceAccessControl)
		{
			for (int i = 0; i < serviceAccessControl.Length; i++)
			{
				string value = serviceAccessControl[i];
				this.CheckStringParameter(value, this.m_Param.ServiceAccessControlDesc, "ServiceAccessControl");
			}
		}
		public void CheckHandleTableSize(uint handleTableSize)
		{
			if (this.m_Param.HandleTableSizeDesc == null)
			{
				throw new ParameterNotFoundException("Desc: HandleTableSize");
			}
			int integer = this.m_Param.HandleTableSizeDesc.GetInteger();
			if ((ulong)handleTableSize <= (ulong)((long)integer))
			{
				return;
			}
			throw new NotPermittedValueException("HandleTableSize", handleTableSize.ToString());
		}
		internal void CheckSystemCallAccessControl(SystemCallAccessControl access)
		{
		}
		internal void CheckInterruptNumbers(InterruptNumberList list)
		{
			string[] interruptNumberList = list.GetInterruptNumberList();
			string[] array = interruptNumberList;
			for (int i = 0; i < array.Length; i++)
			{
				string value = array[i];
				this.CheckStringParameter(value, this.m_Param.EnableInterruptNumbers, "InterruptNumbers");
			}
		}
		public void CheckMemoryMapping(string[] mappingList)
		{
			AddressChecker addressChecker = new AddressChecker(Nyaml.GetStringArray(this.m_Param.MemoryMappingDesc));
			addressChecker.CheckAddress(mappingList);
		}
		public void CheckIORegisterMapping(string[] mappingList)
		{
			AddressChecker addressChecker = new AddressChecker(Nyaml.GetStringArray(this.m_Param.IORegisterMappingDesc));
			addressChecker.CheckAddress(mappingList);
		}
		public byte[] GetAccControlDescRsaSign()
		{
			string @string = this.m_Param.AccCtlDescSign.GetString();
			return Convert.FromBase64String(@string);
		}
		public byte[] GetAccControlDesc()
		{
			string @string = this.m_Param.AccCtlDescBin.GetString();
			return Convert.FromBase64String(@string);
		}
		public Mapping GetEnableSystemCalls()
		{
			return (Mapping)this.m_Param.EnableSystemCalls;
		}
		public CollectionElement GetDefaultSystemCalls()
		{
			Sequence sequence = new Sequence();
			Mapping mapping = (Mapping)this.m_Param.EnableSystemCalls;
			foreach (KeyValuePair<string, CollectionElement> current in mapping)
			{
				sequence.Elements.Add(new ScalarString(current.Key));
			}
			return sequence;
		}
		public CollectionElement GetHandleTableSize()
		{
			return this.m_Param.HandleTableSizeDesc;
		}
		public CollectionElement GetDefaultServiceAccesses()
		{
			return this.m_Param.ServiceAccessControlDesc;
		}
		public CollectionElement GetDefaultMemoryType()
		{
			return this.m_Param.MemoryTypeDesc;
		}
		public CollectionElement GetRunnableOnSleepFlag()
		{
			return this.m_Param.RunnableOnSleepDesc;
		}
		public CollectionElement GetSpecialMemoryArrangeFlag()
		{
			return this.m_Param.SpecialMemoryArrange;
		}
		public RSAParameters GetCommonHeaderKeyParams()
		{
			if (this.m_Param.CommonHeaderKey == null)
			{
				return this.GetDefaultCommonHeaderKey();
			}
			RSAParameters result = default(RSAParameters);
			string @string = this.m_Param.KeyD.GetString();
			string string2 = this.m_Param.KeyP.GetString();
			string string3 = this.m_Param.KeyQ.GetString();
			string string4 = this.m_Param.KeyDP.GetString();
			string string5 = this.m_Param.KeyDQ.GetString();
			string string6 = this.m_Param.KeyInverseQ.GetString();
			string string7 = this.m_Param.KeyModulus.GetString();
			string string8 = this.m_Param.KeyExponent.GetString();
			result.D = Convert.FromBase64String(@string);
			result.P = Convert.FromBase64String(string2);
			result.Q = Convert.FromBase64String(string3);
			result.DP = Convert.FromBase64String(string4);
			result.DQ = Convert.FromBase64String(string5);
			result.InverseQ = Convert.FromBase64String(string6);
			result.Modulus = Convert.FromBase64String(string7);
			result.Exponent = Convert.FromBase64String(string8);
			return result;
		}
		public string[] GetEnableInterruptNumbers()
		{
			if (this.m_Param.EnableInterruptNumbers != null)
			{
				return Nyaml.GetStringArray(this.m_Param.EnableInterruptNumbers);
			}
			throw new ParameterNotFoundException("AccessControlDescriptor/EnableInterrupts");
		}
		public byte[] GetCryptoKey()
		{
			if (this.m_Param.CryptoKey != null && this.m_Param.CryptoKey.GetType() == typeof(ScalarString))
			{
				return Util.GetKeyData(this.m_Param.CryptoKey.GetString());
			}
			return null;
		}
		public byte GetDescVersion()
		{
			if (this.m_Param.DescVersionDesc != null)
			{
				return (byte)this.m_Param.DescVersionDesc.GetInteger();
			}
			throw new ParameterNotFoundException("AccessControlDescriptor/DescVersion");
		}
		public CollectionElement GetSystemMode()
		{
			return this.m_Param.SystemModeDesc;
		}
		public void CheckIsAutoGen()
		{
			if (this.m_Param.AutoGen != null && this.m_Param.AutoGen.GetBoolean())
			{
				return;
			}
			Console.WriteLine("[Makerom] Warning: .desc is not autogen. Maybe signature is not valid");
		}
		public CollectionElement GetResourceLimitCategory()
		{
			return this.m_Param.ResourceLimitCategory;
		}
		public CollectionElement GetReleaseKernelMajor()
		{
			return this.m_Param.ReleaseKernelMajor;
		}
		public CollectionElement GetReleaseKernelMinor()
		{
			return this.m_Param.ReleaseKernelMinor;
		}
		public CollectionElement GetMaxCpu()
		{
			return new ScalarInteger((int)this.GetAccControlDesc()[16]);
		}
		private RSAParameters GetDefaultCommonHeaderKey()
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			return rSACryptoServiceProvider.ExportParameters(true);
		}
	}
}
