using Nintendo.RelocatableObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
namespace Nintendo.MakeRom
{
	internal class CrrReader : IDisposable
	{
		private const int SIGNATURE = 810701379;
		private CrrHeader m_Header = new CrrHeader();
		private Stream m_Stream;
		private bool IsValid()
		{
			return this.m_Header.signature == 810701379u;
		}
		public List<string> GetModuleIdList()
		{
			byte[] array = new byte[this.m_Header.moduleIdSize];
			this.m_Stream.Seek((long)((ulong)this.m_Header.moduleIdOffset), SeekOrigin.Begin);
			this.m_Stream.Read(array, 0, array.Length);
			string @string = Encoding.UTF8.GetString(array);
			string arg_51_0 = @string;
			char[] separator = new char[1];
			return arg_51_0.Split(separator).ToList<string>();
		}
		public byte[] GetHashRegion()
		{
			uint num = this.m_Header.numHash * 32u;
			byte[] array = new byte[num];
			this.m_Stream.Seek((long)((ulong)this.m_Header.hashOffset), SeekOrigin.Begin);
			this.m_Stream.Read(array, 0, array.Length);
			return array;
		}
		public CrrHeader GetHeader()
		{
			return this.m_Header;
		}
		public static CrrReader Load(string crrFile)
		{
			CrrReader crrReader = new CrrReader();
			crrReader.m_Stream = new FileStream(crrFile, FileMode.Open, FileAccess.Read);
			byte[] array = new byte[Marshal.SizeOf(typeof(CrrHeader))];
			crrReader.m_Stream.Read(array, 0, array.Length);
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			try
			{
				Marshal.Copy(array, 0, intPtr, array.Length);
				crrReader.m_Header = (CrrHeader)Marshal.PtrToStructure(intPtr, typeof(CrrHeader));
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (!crrReader.IsValid())
			{
				throw new MakeromException(string.Format("Invalid Crr file: {0}", crrFile));
			}
			return crrReader;
		}
		public void Dispose()
		{
			if (this.m_Stream != null)
			{
				this.m_Stream.Dispose();
			}
		}
	}
}
