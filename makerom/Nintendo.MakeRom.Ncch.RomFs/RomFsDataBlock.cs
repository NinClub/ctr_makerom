using Nintendo.MakeRom.Extensions;
using Nintendo.MakeRom.MakeFS;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
namespace Nintendo.MakeRom.Ncch.RomFs
{
	internal class RomFsDataBlock : WritableBinaryRegistory
	{
		private long m_size;
		private Hash.Input m_inData;
		private long m_protectionAreaSize;
		private string m_tempFile;
		public override long Size
		{
			get
			{
				return this.m_size;
			}
		}
		public RomFsDataBlock(Creator.Input[] files, Stream entryBlockStream)
		{
			this.m_inData = new Hash.Input();
			this.m_inData.sizeOptionalInfo = 0u;
			this.m_inData.sizeBlockLevel1 = 4096u;
			this.m_inData.sizeBlockLevel2 = 4096u;
			this.m_inData.sizeBlockLevel3 = 4096u;
			Hash.QueryTotalSize(ref this.m_size, this.m_inData, Creator.GetDatablockLength(files, entryBlockStream.Length));
			this.m_tempFile = Path.GetTempFileName();
			using (PipeStream pipe = new PipeStream())
			{
				using (FileStream outFileStream = new FileStream(this.m_tempFile, FileMode.Create, FileAccess.Write))
				{
					using (MemoryStream masterHashStream = new MemoryStream())
					{
						long masterHashOffset = 0L;
						long masterHashSize = 0L;
						long protectionAreaOffset = 0L;
						long protectionAreaSize = 0L;
						Thread thread = new Thread((ThreadStart)delegate
						{
							Creator.Create(pipe, files, entryBlockStream, (ulong)entryBlockStream.Length);
							pipe.EndWriting();
						});
						Thread thread2 = new Thread((ThreadStart)delegate
						{
							Hash.Create(masterHashStream, outFileStream, ref masterHashOffset, ref masterHashSize, ref protectionAreaOffset, ref protectionAreaSize, this.m_inData, pipe, Creator.GetDatablockLength(files, entryBlockStream.Length), -1);
						});
						thread.Start();
						thread2.Start();
						thread2.Join();
						thread.Join();
						outFileStream.Seek(masterHashOffset, SeekOrigin.Begin);
						outFileStream.Write(masterHashStream.GetBuffer(), 0, (int)masterHashStream.Length);
						this.m_protectionAreaSize = protectionAreaSize;
					}
				}
			}
		}
		protected override void Update()
		{
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			using (FileStream fileStream = new FileStream(this.m_tempFile, FileMode.Open, FileAccess.Read))
			{
				byte[] array = new byte[1048576];
				while (true)
				{
					int num = fileStream.Read(array, 0, array.Length);
					if (num == 0)
					{
						break;
					}
					writer.Write(array, 0, num);
				}
			}
			writer.Flush();
		}
		internal byte[] GetSuperBlockHash()
		{
			byte[] result;
			using (FileStream fileStream = new FileStream(this.m_tempFile, FileMode.Open, FileAccess.Read))
			{
				byte[] array = new byte[this.GetHashRegionSize()];
				fileStream.Read(array, 0, array.Length);
				result = new SHA256Managed().ComputeHash(array);
			}
			return result;
		}
		internal uint GetHashRegionSize()
		{
			return (uint)((this.m_protectionAreaSize % 512L == 0L) ? this.m_protectionAreaSize : (this.m_protectionAreaSize + 512L & -512L));
		}
		~RomFsDataBlock()
		{
			File.Delete(this.m_tempFile);
		}
	}
}
