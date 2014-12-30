using System;
namespace Nintendo.MakeRom
{
	public abstract class NcchBinary : WritableBinaryRegistory
	{
		public ContentsInfo CxiInfo
		{
			get;
			protected set;
		}
		public ContentsCoreInfo CxiCoreInfo
		{
			get;
			protected set;
		}
		public abstract ulong GetPartitionId();
		public abstract ulong GetProgramId();
		public abstract void SetPartitionId(ulong partitionId);
		public abstract void SetProgramId(ulong programId);
		public abstract byte GetCryptoType();
		public abstract byte[] GetCommonHeader();
		public NcchBinary()
		{
			this.CxiInfo = new ContentsInfo();
			this.CxiCoreInfo = new ContentsCoreInfo();
			this.CxiInfo.CoreInfo = this.CxiCoreInfo;
		}
	}
}
