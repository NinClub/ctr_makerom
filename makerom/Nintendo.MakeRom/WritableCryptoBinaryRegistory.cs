using System;
using System.IO;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	public abstract class WritableCryptoBinaryRegistory : WritableBinaryRegistory
	{
		public ICryptoTransform CryptoTransform
		{
			get;
			set;
		}
		public WritableCryptoBinaryRegistory(ICryptoTransform cryptoTransform)
		{
			this.CryptoTransform = cryptoTransform;
		}
		public WritableCryptoBinaryRegistory() : this(null)
		{
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			if (this.CryptoTransform == null)
			{
				base.WriteBinary(writer);
				return;
			}
			using (MulticoreCryptoStream multicoreCryptoStream = new MulticoreCryptoStream(writer.BaseStream, this.CryptoTransform, CryptoStreamMode.Write))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(multicoreCryptoStream))
				{
					base.WriteBinary(binaryWriter);
				}
			}
			GC.Collect();
		}
	}
}
