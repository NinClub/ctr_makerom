using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	internal class ARM11SystemLocalCapabilities : WritableBinaryRegistory
	{
		private readonly UInt64Data m_ProgramId;
		private readonly ARM11SystemLocalCapabilityFlags m_Flags;
		private readonly byte m_MaxCpu;
		private readonly ReservedBlock m_Reserved0 = new ReservedBlock(1u);
		private readonly ResourceLimitDescriptor[] m_ResourceLimits = new ResourceLimitDescriptor[15];
		private readonly StorageInfo m_StorageInfo;
		private readonly ServiceAccessControl m_ServiceAccessControl;
		private readonly ReservedBlock m_Reserved = new ReservedBlock(31u);
		private readonly ByteData m_ResourceLimitCategory;
		private byte GetResourceLimitCategoryValue(MakeCxiOptions options)
		{
			Dictionary<MakeCxiOptions.ResourceLimitCategoryName, byte> dictionary = new Dictionary<MakeCxiOptions.ResourceLimitCategoryName, byte>
			{

				{
					MakeCxiOptions.ResourceLimitCategoryName.APPLICATION,
					0
				},

				{
					MakeCxiOptions.ResourceLimitCategoryName.SYS_APPLET,
					1
				},

				{
					MakeCxiOptions.ResourceLimitCategoryName.LIB_APPLET,
					2
				},

				{
					MakeCxiOptions.ResourceLimitCategoryName.OTHER,
					3
				}
			};
			return dictionary[options.ResourceLimitCategory];
		}
		public ARM11SystemLocalCapabilities(MakeCxiOptions options)
		{
			this.m_ProgramId = TitleIdUtil.MakeTargetProgramId(options);
			this.m_Flags = new ARM11SystemLocalCapabilityFlags(options);
			this.m_ServiceAccessControl = options.ServiceAccessControl;
			this.m_StorageInfo = new StorageInfo(options);
			this.m_MaxCpu = options.MaxCpu;
			for (int i = 0; i < this.m_ResourceLimits.Length; i++)
			{
				this.m_ResourceLimits[i] = new ResourceLimitDescriptor();
			}
			this.m_ResourceLimitCategory = this.GetResourceLimitCategoryValue(options);
		}
		protected override void Update()
		{
			base.ClearBinaries();
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_ProgramId,
				this.m_Flags
			});
			base.AddBinaries(new IWritableBinary[]
			{
				new ByteData(this.m_MaxCpu)
			});
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_Reserved0
			});
			base.AddBinaries(this.m_ResourceLimits);
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_StorageInfo,
				this.m_ServiceAccessControl,
				this.m_Reserved,
				this.m_ResourceLimitCategory
			});
		}
		public byte[] GetFlags()
		{
			return this.m_Flags.Data;
		}
	}
}
