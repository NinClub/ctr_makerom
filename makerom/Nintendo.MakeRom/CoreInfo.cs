using System;
namespace Nintendo.MakeRom
{
	internal class CoreInfo : WritableBinaryRegistory
	{
		private readonly CodeSetInfo m_CodeSetInfo;
		private readonly DependencyList m_DepedencyList;
		public CoreInfo(CodeSetInfo codeSetInfo, MakeCxiOptions options)
		{
			this.m_CodeSetInfo = codeSetInfo;
			this.m_DepedencyList = options.DependencyList;
			if (options.DependencyVariation != 0)
			{
				this.m_DepedencyList.UpdateVariation(options.DependencyVariation);
			}
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_CodeSetInfo,
				this.m_DepedencyList
			});
		}
	}
}
