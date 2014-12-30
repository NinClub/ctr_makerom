using System;
namespace Nintendo.MakeRom
{
	internal class OtherCapabilityDescriptor : ARM11KernelCapabilityDescriptor
	{
		private enum CapabilityNames
		{
			PERMIT_DEBUG,
			FORCE_DEBUG,
			CAN_USE_NON_ALPHABET_AND_NUMBER,
			CAN_WRITE_SHARED_PAGE,
			CAN_USE_PRIVILEGE_PRIORITY,
			PERMIT_MAIN_FUNCTION_ARGUMENT,
			CAN_SHARE_DEVICE_MEMORY,
			RUNNABLE_ON_SLEEP,
			SPECIAL_MEMORY_ARRANGE = 12
		}
		public OtherCapabilityDescriptor(MakeCxiOptions options) : base(9, 510u)
		{
			base.Data = 0u;
			if (options.PermitDebug)
			{
				this.SetPermitDebugFlag();
			}
			if (options.ForceDebug)
			{
				this.SetForceDebugFlag();
			}
			if (options.CanWriteSharedPage)
			{
				this.SetCanWriteSharedPageFlag();
			}
			if (options.CanUseNonAlphabetAndNumber)
			{
				this.SetCanUseNonAlphabetAndNumberFlag();
			}
			if (options.CanUsePrivilegePriority)
			{
				this.SetCanUsePrivilegePriorityFlag();
			}
			if (options.PermitMainFunctionArgument)
			{
				this.SetPermitMainFunctionArgumentFlag();
			}
			if (options.CanShareDeviceMemory)
			{
				this.SetCanShareDeviceMemoryFlag();
			}
			if (options.RunnableOnSleep)
			{
				this.SetRunnableOnSleepFlag();
			}
			if (options.SpecialMemoryArrange)
			{
				this.SetSpecialMemoryArrangeFlag();
			}
			this.SetMemoryType(options.MemoryType);
		}
		public void SetPermitDebugFlag()
		{
			base.Data |= 1u;
		}
		public void SetForceDebugFlag()
		{
			base.Data |= 2u;
		}
		public void SetCanUseNonAlphabetAndNumberFlag()
		{
			base.Data |= 4u;
		}
		public void SetCanWriteSharedPageFlag()
		{
			base.Data |= 8u;
		}
		public void SetCanUsePrivilegePriorityFlag()
		{
			base.Data |= 16u;
		}
		public void SetPermitMainFunctionArgumentFlag()
		{
			base.Data |= 32u;
		}
		public void SetCanShareDeviceMemoryFlag()
		{
			base.Data |= 64u;
		}
		public void SetRunnableOnSleepFlag()
		{
			base.Data |= 128u;
		}
		public void SetSpecialMemoryArrangeFlag()
		{
			base.Data |= 4096u;
		}
		public void SetMemoryType(MakeCxiOptions.MemoryTypeName memoryType)
		{
			byte b = (byte)memoryType;
			base.Data = ((base.Data & 4294963455u) | (uint)((uint)b << 8));
		}
	}
}
