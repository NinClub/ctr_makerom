using Nintendo.MakeRom.Extensions;
using System;
using System.IO;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	public class NcchFileBinary : NcchBinary
	{
		private ulong m_PartitionId;
		private byte m_CryptoType;
		private NcchCommonHeader m_Header;
		private string m_CxiPath;
		private int BUFFER_SIZE = 1048576;
		private ulong m_Size;
		private bool m_isReplaced;
		public override long Size
		{
			get
			{
				Util.CheckSize(this.m_Size, 9223372036854775807uL);
				return (long)this.m_Size;
			}
		}
		private unsafe void ReadFromStream(Stream reader)
		{
			this.m_Header = new NcchCommonHeader();
			int num = sizeof(NcchCommonHeaderStruct);
			byte[] array = new byte[num];
			reader.Seek(256L, SeekOrigin.Current);
			reader.Read(array, 0, num);
			fixed (NcchCommonHeaderStruct* ptr = &this.m_Header.Struct)
			{
				IntPtr destination = new IntPtr((void*)ptr);
				Marshal.Copy(array, 0, destination, num);
			}
			this.m_PartitionId = this.m_Header.Struct.PartitionId;
			fixed (byte* ptr2 = this.m_Header.Struct.Flags)
			{
				this.m_CryptoType = ptr2[7];
				this.m_Size = (ulong)Util.MediaUnitSizeToInt64(this.m_Header.Struct.ContentSize, ptr2[6]);
			}
		}
		internal NcchFileBinary(string path)
		{
			this.m_CxiPath = path;
			using (FileStream fileStream = new FileStream(this.m_CxiPath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				this.ReadFromStream(fileStream);
			}
		}
		internal NcchFileBinary(Stream reader)
		{
			this.ReadFromStream(reader);
		}
		public override byte GetCryptoType()
		{
			return this.m_CryptoType;
		}
		public override ulong GetPartitionId()
		{
			return this.m_PartitionId;
		}
		public override ulong GetProgramId()
		{
			return this.m_Header.Struct.ProgramId;
		}
		protected override void Update()
		{
		}
		public override byte[] GetCommonHeader()
		{
			return this.m_Header.GetByteArray();
		}
		public override void SetProgramId(ulong programID)
		{
			this.m_Header.Struct.ProgramId = programID;
			this.m_isReplaced = true;
		}
		public override void SetPartitionId(ulong partitionId)
		{
			this.m_PartitionId = partitionId;
			this.m_Header.Struct.PartitionId = partitionId;
			this.m_isReplaced = true;
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			using (Stream stream = this.m_isReplaced ? (Stream)(new ProgramIDReplacedCxiStream(this.m_CxiPath, this.m_Header.Struct.ProgramId, this.m_Header.Struct.PartitionId)) : (Stream)(new FileStream(this.m_CxiPath, FileMode.Open, FileAccess.Read)))
			{
				byte[] buffer = new byte[this.BUFFER_SIZE];
				int count;
				while ((count = stream.Read(buffer, 0, this.BUFFER_SIZE)) != 0)
				{
					writer.Write(buffer, 0, count);
				}
			}
		}
		public unsafe long GetRomfsOffset()
		{
			fixed (byte* ptr = this.m_Header.Struct.Flags)
			{
				return Util.MediaUnitSizeToInt64(this.m_Header.Struct.RomFsOffset, ptr[6]);
			}
		}
	}
}
