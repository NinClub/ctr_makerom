using System;
using System.IO;
namespace Nintendo.MakeRom.Ncch.RomFs
{
	internal class FileBinaryRegistory : IWritableBinary
	{
		private string m_Path;
		private long m_FileSize;
		public long Size
		{
			get
			{
				return this.m_FileSize;
			}
		}
		public FileBinaryRegistory(string path, uint align)
		{
			this.m_Path = path;
		}
		public FileBinaryRegistory(string path) : this(path, 4u)
		{
			this.m_FileSize = new FileInfo(path).Length;
		}
		public void WriteBinary(BinaryWriter writer)
		{
			FileStream fileStream = new FileStream(this.m_Path, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[16777216];
			for (int count = fileStream.Read(buffer, 0, 16777216); count != 0; count = fileStream.Read(buffer, 0, 16777216))
			{
				writer.Write(buffer, 0, count);
			}
			fileStream.Close();
		}
	}
}
