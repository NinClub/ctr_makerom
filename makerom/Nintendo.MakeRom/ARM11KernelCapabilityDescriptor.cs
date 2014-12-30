using System;
namespace Nintendo.MakeRom
{
	internal class ARM11KernelCapabilityDescriptor : WritableBinaryRegistory
	{
		private const uint NULL_DESC = 4294967295u;
		private readonly int m_NumPrefixBits;
		private readonly uint m_PrefixVal;
		protected uint PrefixMask
		{
			get;
			private set;
		}
		protected uint PrefixBits
		{
			get
			{
				return this.m_PrefixVal << 32 - this.m_NumPrefixBits;
			}
		}
		public UInt32Data Data
		{
			get;
			protected set;
		}
		public ARM11KernelCapabilityDescriptor() : this(0, 0u)
		{
			this.Data = 4294967295u;
		}
		public ARM11KernelCapabilityDescriptor(int numPrefixBits, uint prefixVal)
		{
			this.m_NumPrefixBits = numPrefixBits;
			this.m_PrefixVal = prefixVal;
			this.PrefixMask = ~((1u << 32 - numPrefixBits) - 1u);
			this.Data = 0u;
			this.Update();
		}
		protected override void Update()
		{
			this.Data |= this.PrefixBits;
			base.SetBinaries(new IWritableBinary[]
			{
				this.Data
			});
		}
		public bool IsNull()
		{
			return this.Data == 4294967295u;
		}
		public bool Equals(ARM11KernelCapabilityDescriptor desc)
		{
			return this.Data.Data == desc.Data.Data;
		}
	}
}
