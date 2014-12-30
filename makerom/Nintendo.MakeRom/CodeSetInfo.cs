using System;
namespace Nintendo.MakeRom
{
	internal class CodeSetInfo : WritableBinaryRegistory
	{
		private readonly UInt64Name m_Name = new UInt64Name("");
		private readonly SystemInfoFlags m_Flags;
		private readonly CodeSegmentInfo m_TextSectionInfo;
		private readonly UInt32Data m_StackSize = 0u;
		private readonly CodeSegmentInfo m_ReadOnlySectionInfo;
		private readonly ReservedBlock m_Reserved1 = new ReservedBlock(4u);
		private readonly CodeSegmentInfo m_DataSectionInfo;
		private readonly UInt32Data m_BssSize;
		public CodeSetInfo(CodeSegmentInfo textInfo, CodeSegmentInfo readOnlyInfo, CodeSegmentInfo dataInfo, uint bssSize, MakeCxiOptions options) : this(options)
		{
			this.m_TextSectionInfo = textInfo;
			this.m_ReadOnlySectionInfo = readOnlyInfo;
			this.m_DataSectionInfo = dataInfo;
			this.m_BssSize = bssSize;
		}
		public CodeSetInfo(MakeCxiOptions options)
		{
			this.m_Name = options.Name;
			this.m_StackSize = options.MainThreadStackSize;
			this.m_Flags = new SystemInfoFlags(options);
			this.m_TextSectionInfo = new CodeSegmentInfo(0u, 0u, 0u);
			this.m_ReadOnlySectionInfo = new CodeSegmentInfo(0u, 0u, 0u);
			this.m_DataSectionInfo = new CodeSegmentInfo(0u, 0u, 0u);
			this.m_BssSize = 0u;
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_Name,
				this.m_Flags,
				this.m_TextSectionInfo,
				this.m_StackSize,
				this.m_ReadOnlySectionInfo,
				this.m_Reserved1,
				this.m_DataSectionInfo,
				this.m_BssSize
			});
		}
	}
}
