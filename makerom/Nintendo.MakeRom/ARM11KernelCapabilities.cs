using System;
namespace Nintendo.MakeRom
{
	internal class ARM11KernelCapabilities : WritableBinaryRegistory
	{
		private const int FLAGS_MAX_SIZE = 112;
		private SystemCallAccessControl m_SystemCallAccessControl;
		private InterruptNumberList m_InterruptNumberList;
		private AddressMapping m_AddressMapping;
		private OtherCapabilities m_OtherCapabilites;
		private HandleTableSize m_HandleTableSize;
		private ReleaseKernelVersion m_ReleaseKernelVersion;
		private ReservedBlock m_Reserved = new ReservedBlock(16u);
		public ARM11KernelCapabilities(MakeCxiOptions options)
		{
			this.m_AddressMapping = options.Mapping;
			this.m_InterruptNumberList = options.InterruptNumberList;
			this.m_SystemCallAccessControl = options.SystemCallAccessControl;
			this.m_OtherCapabilites = new OtherCapabilities(options);
			this.m_HandleTableSize = new HandleTableSize(options);
			this.m_ReleaseKernelVersion = new ReleaseKernelVersion(options);
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_SystemCallAccessControl,
				this.m_InterruptNumberList,
				this.m_AddressMapping,
				this.m_OtherCapabilites,
				this.m_HandleTableSize,
				this.m_ReleaseKernelVersion
			});
			long num = this.m_SystemCallAccessControl.Size + this.m_InterruptNumberList.Size + this.m_AddressMapping.Size + this.m_OtherCapabilites.Size + this.m_HandleTableSize.Size + this.m_ReleaseKernelVersion.Size;
			if (num > 112L)
			{
				throw new Exception("Too many Kernel Capabilities.");
			}
			while (num < 112L)
			{
				ARM11KernelCapabilityDescriptor aRM11KernelCapabilityDescriptor = new ARM11KernelCapabilityDescriptor();
				base.AddBinaries(new IWritableBinary[]
				{
					aRM11KernelCapabilityDescriptor
				});
				num += aRM11KernelCapabilityDescriptor.Size;
			}
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_Reserved
			});
		}
	}
}
