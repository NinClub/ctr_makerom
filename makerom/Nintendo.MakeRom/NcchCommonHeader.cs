using System;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	internal class NcchCommonHeader : StructData<NcchCommonHeaderStruct>
	{
		private const string SIGNATURE = "NCCH";
		public NcchCommonHeader()
		{
			this.Struct.Signature = "NCCH".ToUInt32ASCII();
		}
		public byte[] GetRsaSignature(RSAParameters param)
		{
			this.Update();
			byte[] byteArray = base.GetByteArray();
			Rsa rsa = new Rsa(param);
			return rsa.GetSign(byteArray, 0, byteArray.Length);
		}
	}
}
