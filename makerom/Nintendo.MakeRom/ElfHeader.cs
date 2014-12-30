using System;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class ElfHeader
	{
		private const int ELF_HEADER_SIZE = 52;
		private const int BYTE_INDEX_TYPE = 16;
		private const int BYTE_SIZE_TYPE = 2;
		private const int BYTE_INDEX_MACHINE = 18;
		private const int BYTE_SIZE_MACHINE = 2;
		private const int BYTE_INDEX_VERSION = 20;
		private const int BYTE_SIZE_VERSION = 4;
		private const int BYTE_INDEX_ENTRY = 24;
		private const int BYTE_SIZE_ENTRY = 4;
		private const int BYTE_INDEX_PROGRAM_HEADER_TABLE_OFFSET = 28;
		private const int BYTE_SIZE_PROGRAM_HEADER_TABLE_OFFSET = 4;
		private const int BYTE_INDEX_SECTION_HEADER_TABLE_OFFSET = 32;
		private const int BYTE_SIZE_SECTION_HEADER_TABLE_OFFSET = 4;
		private const int BYTE_INDEX_FLAGS = 36;
		private const int BYTE_SIZE_FLAGS = 4;
		private const int BYTE_INDEX_ELF_HEADER_SIZE = 40;
		private const int BYTE_SIZE_ELF_HEADER_SIZE = 2;
		private const int BYTE_INDEX_PROGRAM_HEADER_ENTRY_SIZE = 42;
		private const int BYTE_SIZE_PROGRAM_HEADER_ENTRY_SIZE = 2;
		private const int BYTE_INDEX_NUM_PROGRAM_HEADER_ENTRY = 44;
		private const int BYTE_SIZE_NUM_PROGRAM_HEADER_ENTRY = 2;
		private const int BYTE_INDEX_SECTION_HEADER_ENTRY_SIZE = 46;
		private const int BYTE_SIZE_SECTION_HEADER_ENTRY_SIZE = 2;
		private const int BYTE_INDEX_NUM_SECTION_HEADER_ENTRY = 48;
		private const int BYTE_SIZE_NUM_SECTION_HEADER_ENTRY = 2;
		private const int BYTE_INDEX_SECTION_HEADER_TABLE_SECTION = 50;
		private const int BYTE_SIZE_SECTION_HEADER_TABLE_SECTION = 2;
		private readonly byte[] EI_NIDENT = new byte[]
		{
			127,
			69,
			76,
			70
		};
		private readonly byte[] m_Data;
		public ElfHeader(Stream stream)
		{
			this.m_Data = new byte[52];
			stream.Read(this.m_Data, 0, 52);
			if (!this.IsValid())
			{
				throw new InvalidDataException(string.Format("Invalid elf header: {0}:{1}:{2}:{3}", new object[]
				{
					this.m_Data[0],
					this.m_Data[1],
					this.m_Data[2],
					this.m_Data[3]
				}));
			}
			stream.Seek(-52L, SeekOrigin.Current);
		}
		public ushort GetNumSections()
		{
			return BitConverter.ToUInt16(this.m_Data, 48);
		}
		public ushort GetSectionHeaderEntrySize()
		{
			return BitConverter.ToUInt16(this.m_Data, 46);
		}
		public uint GetSectionHeaderTableOffset()
		{
			return BitConverter.ToUInt32(this.m_Data, 32);
		}
		public ushort GetSectionNameTableIndex()
		{
			return BitConverter.ToUInt16(this.m_Data, 50);
		}
		public ushort GetNumPrograms()
		{
			return BitConverter.ToUInt16(this.m_Data, 44);
		}
		public ushort GetProgramHeaderEntrySize()
		{
			return BitConverter.ToUInt16(this.m_Data, 42);
		}
		public uint GetProgramHeaderTableOffset()
		{
			return BitConverter.ToUInt32(this.m_Data, 28);
		}
		private bool IsValid()
		{
			return this.m_Data[0] == this.EI_NIDENT[0] && this.m_Data[1] == this.EI_NIDENT[1] && this.m_Data[2] == this.EI_NIDENT[2] && this.m_Data[3] == this.EI_NIDENT[3];
		}
	}
}
