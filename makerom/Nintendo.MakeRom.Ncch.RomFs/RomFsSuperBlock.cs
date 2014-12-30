using System;
using System.IO;
using System.Security.Cryptography;
namespace Nintendo.MakeRom.Ncch.RomFs
{
	internal class RomFsSuperBlock : WritableBinaryRegistory
	{
		private const uint SIZE_OF_ROMFS_SUPER_BLOCK = 1024u;
		public byte[] GetSuperBlockHash()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream);
			this.WriteBinary(writer);
			return new SHA256Managed().ComputeHash(memoryStream.ToArray());
		}
		protected override void Update()
		{
			ByteArrayData byteArrayData = new ByteArrayData(1024u);
			base.SetBinaries(new IWritableBinary[]
			{
				byteArrayData
			});
		}
	}
}
