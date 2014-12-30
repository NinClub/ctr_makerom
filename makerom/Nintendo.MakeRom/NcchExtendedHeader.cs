using System;
using System.IO;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	internal class NcchExtendedHeader : WritableCryptoBinaryRegistory
	{
		protected readonly SystemControlInfo m_SystemControlInfo;
		protected readonly AccessControlInfo m_AccessControlInfo;
		public NcchExtendedHeader(AccessControlInfo accContInfo, SystemControlInfo sysContInfo, MakeCxiOptions options)
		{
			this.m_SystemControlInfo = sysContInfo;
			this.m_AccessControlInfo = accContInfo;
			base.CryptoTransform = null;
			this.CheckSize();
		}
		protected virtual void CheckSize()
		{
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_SystemControlInfo,
				this.m_AccessControlInfo
			});
		}
		public byte[] GetExtendedHeaderHash()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream);
			this.m_SystemControlInfo.WriteBinary(writer);
			this.m_AccessControlInfo.WriteBinary(writer);
			SHA256 sHA = new SHA256Managed();
			return sHA.ComputeHash(memoryStream.ToArray());
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			base.WriteBinary(writer);
		}
	}
}
