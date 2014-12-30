using System;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class ElfSectionHeader
	{
		public const uint TYPE_NULL = 0u;
		public const uint TYPE_PROGBITS = 1u;
		public const uint TYPE_SYMTAB = 2u;
		public const uint TYPE_STRTAB = 3u;
		public const uint TYPE_RELA = 4u;
		public const uint TYPE_HASH = 5u;
		public const uint TYPE_DYNAMIC = 6u;
		public const uint TYPE_NOTE = 7u;
		public const uint TYPE_NOBITS = 8u;
		public const uint TYPE_REL = 9u;
		public const uint TYPE_SHLIB = 10u;
		public const uint TYPE_DYNSYM = 11u;
		public const uint TYPE_LOPROC = 1879048192u;
		public const uint TYPE_HIPROC = 2147483647u;
		public const uint TYPE_LOUSER = 2147483648u;
		public const uint TYPE_HIUSER = 4294967295u;
		public const uint FLAG_WRITE = 1u;
		public const uint FLAG_ALLOC = 2u;
		public const uint FLAG_EXECINSTR = 4u;
		private const int BYTE_INDEX_NAME = 0;
		private const int BYTE_INDEX_TYPE = 4;
		private const int BYTE_INDEX_FLAGS = 8;
		private const int BYTE_INDEX_ADDRESS = 12;
		private const int BYTE_INDEX_OFFSET = 16;
		private const int BYTE_INDEX_SIZE = 20;
		private const int BYTE_INDEX_LINK = 24;
		private const int BYTE_INDEX_INFO = 28;
		private const int BYTE_INDEX_ADDRESS_ALIGN = 32;
		private const int BYTE_INDEX_ENTRY_SIZE = 36;
		private readonly byte[] m_Data;
		public uint Type
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 4);
			}
		}
		public uint Flags
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 8);
			}
		}
		public uint Offset
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 16);
			}
		}
		public uint Size
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 20);
			}
		}
		public uint Address
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 12);
			}
		}
		public int NameIndex
		{
			get
			{
				return BitConverter.ToInt32(this.m_Data, 0);
			}
		}
		public uint Align
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 32);
			}
		}
		public ElfSectionHeader(Stream stream, int offset, ushort headerSize)
		{
			this.m_Data = new byte[(int)headerSize];
			stream.Seek((long)offset, SeekOrigin.Begin);
			stream.Read(this.m_Data, 0, (int)headerSize);
		}
		public bool IsText()
		{
			return this.Type == 1u && this.Flags == 6u;
		}
		public bool IsReadOnly()
		{
			return this.Type == 1u && this.Flags == 2u;
		}
		public bool IsData()
		{
			return this.Type == 1u && this.Flags == 3u;
		}
		public bool IsBss()
		{
			return this.Type == 8u && this.Flags == 3u;
		}
	}
}
