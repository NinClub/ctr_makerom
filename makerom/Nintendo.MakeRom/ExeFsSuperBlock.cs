using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	internal class ExeFsSuperBlock : WritableBinaryRegistory
	{
		private const int NUM_EXEFS = 8;
		private ExeSectionHeader[] m_Header = new ExeSectionHeader[8];
		private ByteArrayData[] m_Hash = new ByteArrayData[8];
		private ByteArrayData m_Reserved = new byte[128];
		public ExeFsSuperBlock()
		{
			for (int i = 0; i < this.m_Hash.Length; i++)
			{
				this.m_Hash[i] = new byte[32];
				this.m_Header[i] = new ExeSectionHeader();
			}
			this.CheckSize();
		}
		public unsafe void AddData(string name, uint offset, byte[] data, int index)
		{
			byte[] array = new byte[8];
			for (int i = 0; i < name.Length; i++)
			{
				array[i] = (byte)name[i];
			}
			fixed (byte* ptr = this.m_Header[index].Struct.name)
			{
				IntPtr destination = new IntPtr((void*)ptr);
				Marshal.Copy(array, 0, destination, 8);
			}
			this.m_Header[index].Struct.offset = offset;
			this.m_Header[index].Struct.size = (uint)data.Length;
			Array.Copy(new SHA256Managed().ComputeHash(data), this.m_Hash[index].Data, 32);
		}
		protected override void Update()
		{
			base.ClearBinaries();
			for (int i = 0; i < 8; i++)
			{
				base.AddBinary(this.m_Header[i]);
			}
			base.AddBinary(this.m_Reserved);
			for (int j = 7; j >= 0; j--)
			{
				base.AddBinary(this.m_Hash[j]);
			}
		}
		private void CheckSize()
		{
		}
		public byte[] GetSha256Hash()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream);
			this.WriteBinary(writer);
			SHA256 sHA = new SHA256Managed();
			return sHA.ComputeHash(memoryStream.ToArray());
		}
	}
}
