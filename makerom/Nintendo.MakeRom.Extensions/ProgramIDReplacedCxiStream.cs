using Nintendo.MakeRom.Properties;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
namespace Nintendo.MakeRom.Extensions
{
	public class ProgramIDReplacedCxiStream : Stream, IDisposable
	{
		private const int RsaSize = 256;
		private const int HeaderSize = 256;
		private string m_FileName;
		private ulong m_ProgramID;
		private ulong m_OldProgramID;
		private bool m_isCFA;
		private bool m_isNcch;
		private bool m_isReplaced;
		private byte[] m_ReplacedBuffer;
		private bool m_isChangedCategory;
		private bool m_isReplacedPartitionID;
		private ulong m_basePartitionID;
		private ulong m_newPartitionID;
		private Stream m_BaseRecryptingStream;
		private bool m_isRecrypted;
		private long m_RecryptingStart;
		private long m_RecryptingSize;
		private Stream m_BaseStream;
		public ulong ProgramID
		{
			get
			{
				return this.m_ProgramID;
			}
		}
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public override long Position
		{
			get
			{
				return this.m_BaseStream.Position;
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		public unsafe ProgramIDReplacedCxiStream(string filename)
		{
			this.m_isReplaced = false;
			this.m_FileName = filename;
			NcchCommonHeader ncchHeader = this.GetNcchHeader();
			fixed (byte* ptr = ncchHeader.Struct.Flags)
			{
				this.m_isCFA = this.IsCfa(ptr);
			}
			this.m_ProgramID = ncchHeader.Struct.ProgramId;
			this.OpenBaseStream();
		}
		private unsafe void Initialize(string filename, ulong programID, NcchCommonHeader header)
		{
			this.m_FileName = filename;
			this.m_ProgramID = programID;
			fixed (byte* ptr = header.Struct.Flags)
			{
				this.m_isCFA = this.IsCfa(ptr);
			}
			if (this.m_isNcch && header.Struct.ProgramId != programID)
			{
				this.m_isReplaced = true;
				this.m_OldProgramID = header.Struct.ProgramId;
				header.Struct.ProgramId = programID;
				this.MakeReplacedBuffer(header);
				if (TitleIdUtil.IsSystemCategory(TitleIdUtil.GetCategory(this.m_OldProgramID)) != TitleIdUtil.IsSystemCategory(TitleIdUtil.GetCategory(programID)))
				{
					byte b;
					fixed (byte* ptr2 = header.Struct.Flags)
					{
						b = ptr2[7];
					}
					if ((b & 1) != 1)
					{
						throw new NotSupportedException("Unknown aes key");
					}
					this.m_isChangedCategory = true;
					this.prepareRecrypt(header);
					return;
				}
			}
			else
			{
				this.m_isReplaced = false;
			}
		}
		public ProgramIDReplacedCxiStream(string contentName, Stream baseStream, Stream baseRecryptStream, ulong programID)
		{
			NcchCommonHeader ncchHeaderFromStream = this.GetNcchHeaderFromStream(baseStream);
			this.Initialize(contentName, programID, ncchHeaderFromStream);
			baseStream.Seek(0L, SeekOrigin.Begin);
			this.OpenBaseStreamCore(baseStream, baseRecryptStream);
		}
		public ProgramIDReplacedCxiStream(string filename, ulong programID)
		{
			NcchCommonHeader ncchHeaderFromFile = this.GetNcchHeaderFromFile(filename);
			this.Initialize(filename, programID, ncchHeaderFromFile);
			this.OpenBaseStream();
		}
		public unsafe ProgramIDReplacedCxiStream(string filename, ulong programID, ulong partitionID)
		{
			this.m_FileName = filename;
			this.m_ProgramID = programID;
			NcchCommonHeader ncchHeader = this.GetNcchHeader();
			fixed (byte* ptr = ncchHeader.Struct.Flags)
			{
				this.m_isCFA = this.IsCfa(ptr);
			}
			this.m_isReplaced = false;
			this.m_OldProgramID = ncchHeader.Struct.ProgramId;
			if (this.m_isNcch && ncchHeader.Struct.ProgramId != programID)
			{
				this.m_isReplaced = true;
				ncchHeader.Struct.ProgramId = programID;
				this.MakeReplacedBuffer(ncchHeader);
				if (TitleIdUtil.IsSystemCategory(TitleIdUtil.GetCategory(this.m_OldProgramID)) != TitleIdUtil.IsSystemCategory(TitleIdUtil.GetCategory(programID)))
				{
					byte b;
					fixed (byte* ptr2 = ncchHeader.Struct.Flags)
					{
						b = ptr2[7];
					}
					if ((b & 1) != 1)
					{
						throw new NotSupportedException("Unknown aes key");
					}
					this.m_isChangedCategory = true;
					this.prepareRecrypt(ncchHeader);
				}
			}
			if (this.m_isNcch && ncchHeader.Struct.PartitionId != partitionID)
			{
				byte b2;
				fixed (byte* ptr3 = ncchHeader.Struct.Flags)
				{
					b2 = ptr3[7];
				}
				if ((b2 & 1) != 1)
				{
					throw new NotSupportedException("Unknown aes key");
				}
				this.m_isReplacedPartitionID = true;
				this.m_isReplaced = true;
				ulong partitionId = ncchHeader.Struct.PartitionId;
				ncchHeader.Struct.PartitionId = partitionID;
				this.prepareRecrypt(ncchHeader, partitionId, partitionID);
				this.MakeReplacedBuffer(ncchHeader);
			}
			this.OpenBaseStream();
		}
		private unsafe void prepareRecrypt(NcchCommonHeader header, ulong basePartitionID, ulong newPartitionID)
		{
			this.m_basePartitionID = basePartitionID;
			this.m_newPartitionID = newPartitionID;
			byte b;
			fixed (byte* ptr = header.Struct.Flags)
			{
				b = ptr[6];
			}
			this.m_RecryptingStart = (long)((ulong)((ulong)header.Struct.RomFsOffset << (int)(b + 9)));
			this.m_RecryptingSize = (long)((ulong)((ulong)header.Struct.RomFsSize << (int)(b + 9)));
		}
		private void prepareRecrypt(NcchCommonHeader header)
		{
			ulong partitionId = header.Struct.PartitionId;
			this.prepareRecrypt(header, partitionId, partitionId);
		}
		private byte[] GetKeyFromProgramId(ulong programId)
		{
			if (!TitleIdUtil.IsSystemCategory(TitleIdUtil.GetCategory(programId)))
			{
				return Resources.key;
			}
			if (MakeCxi.SystemFixedKey == null)
			{
				throw new MakeromException("System fixed key is not found.");
			}
			return MakeCxi.SystemFixedKey;
		}
		private unsafe NcchCommonHeader GetNcchHeaderFromStream(Stream fStream)
		{
			NcchCommonHeader ncchCommonHeader = new NcchCommonHeader();
			byte[] array = new byte[Marshal.SizeOf(ncchCommonHeader.Struct)];
			fStream.Seek(256L, SeekOrigin.Begin);
			fStream.Read(array, 0, Marshal.SizeOf(ncchCommonHeader.Struct));
			this.m_isNcch = true;
			byte[] bytes = Encoding.UTF8.GetBytes("NCCH");
			for (int i = 0; i < bytes.Length; i++)
			{
				if (array[i] != bytes[i])
				{
					this.m_isNcch = false;
				}
			}
			NcchCommonHeaderStruct @struct;
			byte* ptr = (byte*)(&@struct);
			for (int j = 0; j < array.Length; j++)
			{
				ptr[j] = array[j];
			}
			ncchCommonHeader.Struct = @struct;
			return ncchCommonHeader;
		}
		private NcchCommonHeader GetNcchHeaderFromFile(string filename)
		{
			NcchCommonHeader ncchHeaderFromStream;
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				ncchHeaderFromStream = this.GetNcchHeaderFromStream(fileStream);
			}
			return ncchHeaderFromStream;
		}
		private NcchCommonHeader GetNcchHeader()
		{
			return this.GetNcchHeaderFromFile(this.m_FileName);
		}
		private unsafe bool IsCfa(byte* flags)
		{
			return (flags[5] & 3) == 1;
		}
		private void OpenBaseStreamCore(Stream baseStream, Stream recryptTargetStream)
		{
			if (this.m_isReplacedPartitionID || this.m_isChangedCategory)
			{
				recryptTargetStream.Seek(this.m_RecryptingStart, SeekOrigin.Begin);
				byte[] keyFromProgramId = this.GetKeyFromProgramId(this.m_OldProgramID);
				byte[] keyFromProgramId2 = this.GetKeyFromProgramId(this.m_ProgramID);
				AesCtr crypto = new AesCtr(keyFromProgramId, this.m_basePartitionID, 216172782113783808uL);
				AesCtr crypto2 = new AesCtr(keyFromProgramId2, this.m_newPartitionID, 216172782113783808uL);
				this.m_BaseRecryptingStream = new MulticoreCryptoStreamReader(new MulticoreCryptoStreamReader(recryptTargetStream, crypto, CryptoStreamMode.Read), crypto2, CryptoStreamMode.Read);
				this.m_isRecrypted = true;
			}
			this.m_BaseStream = baseStream;
		}
		private void OpenBaseStream()
		{
			this.OpenBaseStreamCore(new FileStream(this.m_FileName, FileMode.Open, FileAccess.Read), new FileStream(this.m_FileName, FileMode.Open, FileAccess.Read));
		}
		private void MakeReplacedBuffer(NcchCommonHeader header)
		{
			if (!this.m_isCFA)
			{
				throw new NotSupportedException("CXI's ID cannot be modified");
			}
			this.m_ReplacedBuffer = new byte[512];
			using (MemoryStream memoryStream = new MemoryStream(this.m_ReplacedBuffer))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
					rSACryptoServiceProvider.FromXmlString(Resources.DevNcsdCfa);
					binaryWriter.Write(header.GetRsaSignature(rSACryptoServiceProvider.ExportParameters(true)));
					header.WriteBinary(binaryWriter);
				}
			}
		}
		public override void Flush()
		{
			throw new NotImplementedException();
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			if (!this.m_isReplaced)
			{
				return this.m_BaseStream.Read(buffer, offset, count);
			}
			if (this.m_BaseStream.Position < 512L)
			{
				int num2 = (int)((this.m_BaseStream.Position + (long)count > 512L) ? (512L - this.m_BaseStream.Position) : ((long)count));
				Array.Copy(this.m_ReplacedBuffer, this.m_BaseStream.Position, buffer, (long)offset, (long)num2);
				this.m_BaseStream.Seek((long)num2, SeekOrigin.Current);
				num += num2;
				offset += num2;
				count -= num2;
			}
			if (!this.m_isRecrypted)
			{
				return this.m_BaseStream.Read(buffer, offset, count) + num;
			}
			if (this.m_isRecrypted && this.m_BaseStream.Position < this.m_RecryptingStart)
			{
				int num3 = (int)((this.m_BaseStream.Position + (long)count > this.m_RecryptingStart) ? (this.m_RecryptingStart - this.m_BaseStream.Position) : ((long)count));
				this.m_BaseStream.Read(buffer, offset, num3);
				num += num3;
				offset += num3;
				count -= num3;
			}
			if (this.m_isRecrypted && this.m_BaseStream.Position < this.m_RecryptingStart + this.m_RecryptingSize)
			{
				int num4 = (int)((this.m_BaseStream.Position + (long)count > this.m_RecryptingStart + this.m_RecryptingSize) ? (this.m_RecryptingStart + this.m_RecryptingSize - this.m_BaseStream.Position) : ((long)count));
				this.m_BaseStream.Seek((long)num4, SeekOrigin.Current);
				this.m_BaseRecryptingStream.Read(buffer, offset, num4);
				num += num4;
				offset += num4;
				count -= num4;
			}
			return this.m_BaseStream.Read(buffer, offset, count) + num;
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this.m_isRecrypted && this.m_BaseStream.Position > this.m_RecryptingStart)
			{
				throw new NotSupportedException();
			}
			return this.m_BaseStream.Seek(offset, origin);
		}
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}
		void IDisposable.Dispose()
		{
			this.m_BaseStream.Dispose();
		}
	}
}
