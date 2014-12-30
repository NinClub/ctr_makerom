using System;
namespace Nintendo.MakeRom
{
	internal class CodeSegmentInfo : WritableBinaryRegistory
	{
		public UInt32Data Address
		{
			get;
			private set;
		}
		public UInt32Data NumMaxPages
		{
			get;
			private set;
		}
		public UInt32Data CodeSize
		{
			get;
			private set;
		}
		public CodeSegmentInfo(uint address, uint numMaxPages, uint codeSize)
		{
			this.Address = address;
			this.NumMaxPages = numMaxPages;
			this.CodeSize = codeSize;
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.Address,
				this.NumMaxPages,
				this.CodeSize
			});
		}
		public override string ToString()
		{
			return string.Format("CodeSegmentInfo: Address 0x{0:x8}, NumMaxPages {1}, Size {2}", this.Address.Data, this.NumMaxPages.Data, this.CodeSize.Data);
		}
	}
}
