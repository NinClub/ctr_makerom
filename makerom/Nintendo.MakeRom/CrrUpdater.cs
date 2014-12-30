using Nintendo.MakeRom.Properties;
using Nintendo.RelocatableObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	internal class CrrUpdater
	{
		private const int SIGN_TARGET_BEGIN_ADDR = 832;
		private const int SIGN_TARGET_HEADER_SIZE = 32;
		private readonly string CRR_EXTENSION = ".crr";
		private readonly string CRR_DIR = ".crr";
		private string m_CrrRoot;
		private FileInfo[] m_CrrFiles = new FileInfo[0];
		public CrrUpdater(string romfsRoot)
		{
			this.m_CrrRoot = Path.Combine(romfsRoot, this.CRR_DIR);
			this.ReadCrrDir();
		}
		public void ReadCrrDir()
		{
			string name = ".crr directory includes invalid file or directory";
			DirectoryInfo directoryInfo = new DirectoryInfo(this.m_CrrRoot);
			if (!directoryInfo.Exists)
			{
				return;
			}
			if (directoryInfo.GetDirectories().Count<DirectoryInfo>() != 0)
			{
				throw new MakeromException(name);
			}
			this.m_CrrFiles = directoryInfo.GetFiles();
			FileInfo[] crrFiles = this.m_CrrFiles;
			for (int i = 0; i < crrFiles.Length; i++)
			{
				FileInfo fileInfo = crrFiles[i];
				if (fileInfo.Extension != this.CRR_EXTENSION)
				{
					throw new MakeromException(name);
				}
			}
		}
		private byte[] CrrHeaderToBytes(CrrHeader header)
		{
			byte[] array = new byte[Marshal.SizeOf(typeof(CrrHeader))];
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			try
			{
				Marshal.StructureToPtr(header, intPtr, true);
				Marshal.Copy(intPtr, array, 0, array.Length);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return array;
		}
		public void Update(uint uniqueId)
		{
			Rsa rsa = new Rsa(Resources.CrrDevKey);
			FileInfo[] crrFiles = this.m_CrrFiles;
			for (int i = 0; i < crrFiles.Length; i++)
			{
				FileInfo fileInfo = crrFiles[i];
				byte[] array = null;
				CrrHeader crrHeader = null;
				using (CrrReader crrReader = CrrReader.Load(fileInfo.FullName))
				{
					crrHeader = crrReader.GetHeader();
					array = crrReader.GetHashRegion();
				}
				crrHeader.uniqueId = uniqueId << 8;
				byte[] array2 = this.CrrHeaderToBytes(crrHeader);
				byte[] array3 = new byte[array.Length + 32];
				Array.Copy(array2, 832, array3, 0, 32);
				Array.Copy(array, 0, array3, 32, array.Length);
				crrHeader.sign.value = rsa.GetSign(array3, 0, array3.Length);
				array2 = this.CrrHeaderToBytes(crrHeader);
				using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Write))
				{
					fileStream.Seek(0L, SeekOrigin.Begin);
					fileStream.Write(array2, 0, array2.Length);
				}
			}
		}
		public List<string> GetModuleIdList()
		{
			List<string> list = new List<string>();
			FileInfo[] crrFiles = this.m_CrrFiles;
			for (int i = 0; i < crrFiles.Length; i++)
			{
				FileInfo fileInfo = crrFiles[i];
				using (CrrReader crrReader = CrrReader.Load(fileInfo.FullName))
				{
					list.AddRange(crrReader.GetModuleIdList());
				}
			}
			list.RemoveAll((string item) => item == "");
			return list;
		}
	}
}
