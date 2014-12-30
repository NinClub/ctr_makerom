using System;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class ElfProgramHeader
	{
		private const int BYTE_INDEX_TYPE = 0;
		private const int BYTE_INDEX_OFFSET = 4;
		private const int BYTE_INDEX_V_ADDR = 8;
		private const int BYTE_INDEX_P_ADDR = 12;
		private const int BYTE_INDEX_FILE_SIZE = 16;
		private const int BYTE_INDEX_MEMORY_SIZE = 20;
		private const int BYTE_INDEX_FLAGS = 24;
		private const int BYTE_INDEX_ALIGN = 28;
		private readonly byte[] m_Data;
		public uint Type
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 0);
			}
		}
		public uint Offset
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 4);
			}
		}
		public uint VAddr
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 8);
			}
		}
		public uint PAddr
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 12);
			}
		}
		public uint FileSize
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 16);
			}
		}
		public uint MemorySize
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 20);
			}
		}
		public uint Flags
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 24);
			}
		}
		public uint Align
		{
			get
			{
				return BitConverter.ToUInt32(this.m_Data, 28);
			}
		}
		public ElfProgramHeader(Stream stream, int offset, ushort headerSize)
		{
			this.m_Data = new byte[(int)headerSize];
			stream.Seek((long)offset, SeekOrigin.Begin);
			stream.Read(this.m_Data, 0, (int)headerSize);
		}
	}
}
