using Nintendo.MakeRom.Extensions;
using Nintendo.MakeRom.MakeFS;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
namespace Nintendo.MakeRom.Ncch.FastBuildRomfs
{
	internal class FastBuildRomFsDataBlock
	{
		private long m_size;
		private Hash.Input m_inData;
		private long m_protectionAreaSize;
		private byte[] m_protectionAreaHash;
		public long Size
		{
			get
			{
				return this.m_size;
			}
		}
		public FastBuildRomFsDataBlock(Creator.Input[] files, Stream entryBlockStream, Stream outputStream, AesCtr ctr)
		{
			long position = outputStream.Position;
			this.m_inData = new Hash.Input();
			this.m_inData.sizeOptionalInfo = 0u;
			this.m_inData.sizeBlockLevel1 = 4096u;
			this.m_inData.sizeBlockLevel2 = 4096u;
			this.m_inData.sizeBlockLevel3 = 4096u;
			Hash.QueryTotalSize(ref this.m_size, this.m_inData, Creator.GetDatablockLength(files, entryBlockStream.Length));
			AesCtr aesCtr = new AesCtr(ctr.key, ctr.GetCounter());
			AesCtr transform = new AesCtr(ctr.key, ctr.GetCounter());
			long position2;
			using (PipeStream pipe = new PipeStream())
			{
				using (MemoryStream masterHashStream = new MemoryStream())
				{
					long masterHashOffset = 0L;
					long masterHashSize = 0L;
					long protectionAreaOffset = 0L;
					long protectionAreaSize = 0L;
					using (MulticoreCryptoStream cryptoStream = new MulticoreCryptoStream(outputStream, ctr, CryptoStreamMode.Write))
					{
						Thread thread = new Thread((ThreadStart)delegate
						{
							Creator.Create(pipe, files, entryBlockStream, (ulong)entryBlockStream.Length);
							pipe.EndWriting();
						});
						Thread thread2 = new Thread((ThreadStart)delegate
						{
							Hash.Create(masterHashStream, cryptoStream, ref masterHashOffset, ref masterHashSize, ref protectionAreaOffset, ref protectionAreaSize, this.m_inData, pipe, Creator.GetDatablockLength(files, entryBlockStream.Length), -1);
						});
						thread.Start();
						thread2.Start();
						thread2.Join();
						thread.Join();
					}
					position2 = outputStream.Position;
					outputStream.Seek(position + masterHashOffset, SeekOrigin.Begin);
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream3 = new CryptoStream(memoryStream, aesCtr, CryptoStreamMode.Write))
						{
							cryptoStream3.Write(new byte[masterHashOffset], 0, (int)masterHashOffset);
						}
					}
					using (MulticoreCryptoStream multicoreCryptoStream = new MulticoreCryptoStream(outputStream, aesCtr, CryptoStreamMode.Write))
					{
						multicoreCryptoStream.Write(masterHashStream.GetBuffer(), 0, (int)masterHashStream.Length);
						this.m_protectionAreaSize = protectionAreaSize;
					}
				}
			}
			outputStream.Seek(position, SeekOrigin.Begin);
			byte[] array = new byte[this.GetHashRegionSize()];
			outputStream.Read(array, 0, array.Length);
			outputStream.Seek(position2, SeekOrigin.Begin);
			using (MemoryStream memoryStream2 = new MemoryStream(array))
			{
				using (CryptoStream cryptoStream2 = new CryptoStream(memoryStream2, transform, CryptoStreamMode.Read))
				{
					byte[] array2 = new byte[this.GetHashRegionSize()];
					cryptoStream2.Read(array2, 0, array2.Length);
					this.m_protectionAreaHash = new SHA256Managed().ComputeHash(array2);
				}
			}
		}
		internal uint GetHashRegionSize()
		{
			return (uint)((this.m_protectionAreaSize % 512L == 0L) ? this.m_protectionAreaSize : (this.m_protectionAreaSize + 512L & -512L));
		}
		internal byte[] GetActualHash()
		{
			return this.m_protectionAreaHash;
		}
	}
}
