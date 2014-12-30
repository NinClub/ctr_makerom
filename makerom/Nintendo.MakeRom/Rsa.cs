using System;
using System.Linq;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	public class Rsa
	{
		private RSACryptoServiceProvider m_Rsa = new RSACryptoServiceProvider();
		private RSAParameters m_Param;
		public Rsa(string keyXML)
		{
			this.m_Rsa.FromXmlString(keyXML);
			this.m_Param = this.m_Rsa.ExportParameters(true);
		}
		public Rsa(RSAParameters param)
		{
			this.m_Rsa.ImportParameters(param);
			this.m_Param = param;
		}
		public byte[] GetSign(byte[] data, int offset, int size)
		{
			RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(this.m_Rsa);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm("SHA256");
			SHA256 sHA = new SHA256Managed();
			byte[] rgbHash = sHA.ComputeHash(data, offset, size);
			return rSAPKCS1SignatureFormatter.CreateSignature(rgbHash);
		}
		public bool Verify(byte[] data, int offset, int size, byte[] signature)
		{
			SHA256 sHA = new SHA256Managed();
			byte[] first = sHA.ComputeHash(data, offset, size);
			byte[] second = this.m_Rsa.Decrypt(signature, false);
			return first.SequenceEqual(second);
		}
		public byte[] GetPrivateKey()
		{
			return this.m_Param.D;
		}
		public byte[] GetPublicKey()
		{
			return this.m_Param.Modulus;
		}
	}
}
