using System;
using System.IO;
using System.Security.Cryptography;
namespace Nintendo.MakeRom.Ncch.FastBuildRomfs
{
	public class FastBuildRomfsHeader
	{
		public static readonly int MakeromfsInfoSize = 1024;
		private byte[] m_padding = new byte[FastBuildRomfsHeader.MakeromfsInfoSize - 12 - 32];
		private byte[] m_iv = new byte[16];
		public byte[] ProtectionHash
		{
			internal get;
			set;
		}
		public uint ProtectionArea
		{
			internal get;
			set;
		}
		public long RomfsSize
		{
			get;
			set;
		}
		public FastBuildRomfsHeader()
		{
			this.m_iv[8] = 4;
			this.ProtectionHash = new byte[32];
		}
		public void Dump(Stream writer)
		{
			using (MemoryStream memoryStream = new MemoryStream(FastBuildRomfsHeader.MakeromfsInfoSize))
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new AesCtr(new byte[16], this.m_iv), CryptoStreamMode.Write))
				{
					if (this.ProtectionHash.Length != 32)
					{
						throw new MakeromException("Invalid r.bin Hash Length");
					}
					cryptoStream.Write(this.ProtectionHash, 0, this.ProtectionHash.Length);
					byte[] bytes = BitConverter.GetBytes(this.ProtectionArea);
					cryptoStream.Write(bytes, 0, bytes.Length);
					bytes = BitConverter.GetBytes(this.RomfsSize);
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.Write(this.m_padding, 0, this.m_padding.Length);
					writer.Write(memoryStream.GetBuffer(), 0, FastBuildRomfsHeader.MakeromfsInfoSize);
				}
			}
		}
		public void Read(Stream reader)
		{
			byte[] array = new byte[FastBuildRomfsHeader.MakeromfsInfoSize];
			reader.Read(array, 0, array.Length);
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new AesCtr(new byte[16], this.m_iv), CryptoStreamMode.Read))
				{
					cryptoStream.Read(this.ProtectionHash, 0, this.ProtectionHash.Length);
					byte[] array2 = new byte[4];
					cryptoStream.Read(array2, 0, array2.Length);
					this.ProtectionArea = BitConverter.ToUInt32(array2, 0);
					array2 = new byte[8];
					cryptoStream.Read(array2, 0, array2.Length);
					this.RomfsSize = BitConverter.ToInt64(array2, 0);
					cryptoStream.Read(this.m_padding, 0, this.m_padding.Length);
				}
			}
		}
	}
}
