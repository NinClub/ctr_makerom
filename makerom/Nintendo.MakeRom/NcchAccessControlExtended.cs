using System;
using System.IO;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	internal class NcchAccessControlExtended : WritableCryptoBinaryRegistory
	{
		private readonly ByteArrayData m_RsaSignature;
		private readonly ByteArrayData m_NcchHeaderPublicKey;
		private readonly ByteArrayData m_AccessControlInfoDescriptor;
		public NcchAccessControlExtended(MakeCxiOptions options, RSAParameters keyParams)
		{
			this.m_AccessControlInfoDescriptor = options.AccControlDescBin;
			this.m_NcchHeaderPublicKey = new Rsa(keyParams).GetPublicKey();
			this.m_RsaSignature = options.AccControlDescRsaSign;
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_RsaSignature,
				this.m_NcchHeaderPublicKey,
				this.m_AccessControlInfoDescriptor
			});
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			base.WriteBinary(writer);
		}
	}
}
