using Nintendo.MakeRom.Extensions;
using Nintendo.MakeRom.Ncch.FastBuildRomfs;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
namespace Nintendo.MakeRom.Ncch.RomFs
{
	internal class DotRomFsBinary : WritableBinaryRegistory, IRomFsBinary, IWritableBinary
	{
		private FastBuildRomfsHeader m_fastBuildRomfsHeader;
		private byte[] m_hash;
		private long m_size;
		private string m_romfsFilename;
		public ICryptoTransform CryptoTransform
		{
			get;
			set;
		}
		public RomFsInfo RomFsInfo
		{
			get;
			private set;
		}
		public override long Size
		{
			get
			{
				return this.m_size;
			}
		}
		internal DotRomFsBinary(MakeCxiOptions options, string dotRomfsFileName)
		{
			this.RomFsInfo = new RomFsInfo();
			this.m_fastBuildRomfsHeader = new FastBuildRomfsHeader();
			this.m_romfsFilename = dotRomfsFileName;
			using (FileStream fileStream = new FileStream(this.m_romfsFilename, FileMode.Open, FileAccess.Read))
			{
				this.m_fastBuildRomfsHeader.Read(fileStream);
				AesCtr transform = new AesCtr(options.AesKey, Util.MakePartitionId(options), 216172782113783808uL);
				byte[] array = new byte[this.m_fastBuildRomfsHeader.ProtectionArea];
				using (CryptoStream cryptoStream = new CryptoStream(fileStream, transform, CryptoStreamMode.Read))
				{
					cryptoStream.Read(array, 0, array.Length);
				}
				if (Encoding.ASCII.GetString(array, 0, 4) != "IVFC")
				{
					throw new ArgumentException("invalid .romfs file, please recreate it");
				}
				this.m_hash = new SHA256Managed().ComputeHash(array);
				if (!this.m_hash.SequenceEqual(this.m_fastBuildRomfsHeader.ProtectionHash))
				{
					throw new ArgumentException("invalid .romfs file, please recreate it");
				}
			}
			this.m_size = this.m_fastBuildRomfsHeader.RomfsSize;
		}
		protected override void Update()
		{
		}
		public uint GetHashRegionSize()
		{
			return this.m_fastBuildRomfsHeader.ProtectionArea;
		}
		public byte[] GetSuperBlockHash()
		{
			return this.m_hash;
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			Profiler.Entry("Romfs");
			using (PipeStream p = new PipeStream())
			{
				Thread thread = new Thread((ThreadStart)delegate
				{
					byte[] array = new byte[4194304];
					using (FileStream fileStream = new FileStream(this.m_romfsFilename, FileMode.Open, FileAccess.Read))
					{
						fileStream.Seek((long)FastBuildRomfsHeader.MakeromfsInfoSize, SeekOrigin.Begin);
						long num = 0L;
						while (num < this.m_size)
						{
							int num2 = (int)((this.m_size - num < (long)array.Length) ? (this.m_size - num) : ((long)array.Length));
							int num3 = fileStream.Read(array, 0, num2);
							if (num3 != num2)
							{
								throw new FormatException("Invalid .romfs file");
							}
							p.Write(array, 0, num3);
							num += (long)num3;
							if (num3 < array.Length)
							{
								break;
							}
						}
					}
					p.EndWriting();
				});
				Thread thread2 = new Thread((ThreadStart)delegate
				{
					byte[] array = new byte[4194304];
					int num;
					do
					{
						num = p.Read(array, 0, array.Length);
						writer.Write(array, 0, num);
					}
					while (num >= array.Length);
				});
				thread.Start();
				thread2.Start();
				thread2.Join();
				thread.Join();
			}
			Profiler.Exit("Romfs");
		}
	}
}
