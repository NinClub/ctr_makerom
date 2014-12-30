using makerom.Properties;
using Nintendo.MakeRom;
using System;
namespace makerom
{
	internal class NcsdCommonHeader : StructData<NcsdCommonHeaderStruct>
	{
		private const string SIGNATURE = "NCSD";
		public uint MediaSize
		{
			get
			{
				return this.Struct.MediaSize;
			}
			set
			{
				this.Struct.MediaSize = value;
			}
		}
		public NcsdCommonHeader()
		{
			this.Struct.Signature = "NCSD".ToUInt32ASCII();
		}
		public ByteArrayData GetRsaSignature()
		{
			Rsa rsa = new Rsa(Resources.DevNcsdCfa);
			this.Update();
			return rsa.GetSign(base.GetByteArray(), 0, base.GetByteArray().Length);
		}
	}
}
