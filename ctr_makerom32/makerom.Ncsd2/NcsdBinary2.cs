using Nintendo.MakeRom;
using Nintendo.MakeRom.Ncch.FastBuildRomfs;
using System;
using System.IO;
using System.Runtime.InteropServices;
namespace makerom.Ncsd2
{
	internal class NcsdBinary2
	{
		internal Ccl Cci2Image;
		private unsafe long GetNcchOffsetFrom2a(Stream ncsd, int partitionIndex)
		{
			byte[] array = new byte[new NcsdHeader().Size];
			ncsd.Read(array, 0, array.Length);
			fixed (byte* ptr = &array[256])
			{
				NcsdCommonHeaderStruct ncsdCommonHeaderStruct = (NcsdCommonHeaderStruct)Marshal.PtrToStructure((IntPtr)((void*)ptr), typeof(NcsdCommonHeaderStruct));
				return makerom.Util.MediaUnitToByte(ncsdCommonHeaderStruct.ParitionOffsetAndSize[partitionIndex * 2], ncsdCommonHeaderStruct.Flags[6]);
			}
		}
		private unsafe long GetNcchOffsetFrom2aAnd2bWithoutRomfs(Stream ebin, Stream rbin)
		{
			if (rbin.Length == rbin.Position)
			{
				return 0L;
			}
			NcchBinary ncchBinary = MakeCxi.LoadFromStream(rbin);
			ulong partitionId = ncchBinary.GetPartitionId();
			byte[] array = new byte[new NcsdHeader().Size];
			ebin.Read(array, 0, array.Length);
			fixed (byte* ptr = &array[256])
			{
				NcsdCommonHeaderStruct ncsdCommonHeaderStruct = (NcsdCommonHeaderStruct)Marshal.PtrToStructure((IntPtr)((void*)ptr), typeof(NcsdCommonHeaderStruct));
				for (int i = 0; i < 8; i++)
				{
					if ((ncsdCommonHeaderStruct.PartitionId)[i] == partitionId)
					{
						return makerom.Util.MediaUnitToByte(ncsdCommonHeaderStruct.ParitionOffsetAndSize[i * 2], ncsdCommonHeaderStruct.Flags[6]);
					}
				}
			}
			throw new MakeromException("Invalid r.bin file");
		}
		private long GetRomfsOffsetFromNcch(Stream ncch)
		{
			NcchFileBinary ncchFileBinary = MakeCxi.LoadFromStream(ncch);
			return ncchFileBinary.GetRomfsOffset();
		}
		internal NcsdBinary2(string path2a, string path2b, string outputPath)
		{
			this.Cci2Image = new Ccl();
			FileInfo fileInfo = new FileInfo(path2a);
			this.Cci2Image.Images.Add(new Ccl.Image(0L, fileInfo.Length, 0L, fileInfo.LastWriteTime.ToFileTime())
			{
				Path = makerom.Util.GetRelativePath(Path.GetFullPath(outputPath), Path.GetFullPath(path2a))
			});
			FileInfo fileInfo2 = new FileInfo(path2b);
			long addr = 0L;
			using (FileStream fileStream = fileInfo.OpenRead())
			{
				long ncchOffsetFrom2a = this.GetNcchOffsetFrom2a(fileStream, 0);
				fileStream.Seek(ncchOffsetFrom2a, SeekOrigin.Begin);
				long romfsOffsetFromNcch = this.GetRomfsOffsetFromNcch(fileStream);
				if (romfsOffsetFromNcch != 0L)
				{
					addr = romfsOffsetFromNcch + ncchOffsetFrom2a;
				}
				else
				{
					fileStream.Seek(0L, SeekOrigin.Begin);
					using (FileStream fileStream2 = fileInfo2.OpenRead())
					{
						fileStream2.Seek((long)FastBuildRomfsHeader.MakeromfsInfoSize, SeekOrigin.Begin);
						addr = this.GetNcchOffsetFrom2aAnd2bWithoutRomfs(fileStream, fileStream2);
					}
				}
			}
			this.Cci2Image.Images.Add(new Ccl.Image(addr, fileInfo2.Length - (long)FastBuildRomfsHeader.MakeromfsInfoSize, (long)FastBuildRomfsHeader.MakeromfsInfoSize, fileInfo2.LastWriteTime.ToFileTime())
			{
				Path = makerom.Util.GetRelativePath(Path.GetFullPath(outputPath), Path.GetFullPath(path2b))
			});
		}
		private void CopyStreamToStream(Stream input, Stream output, long size)
		{
			byte[] array = new byte[1048576];
			int num2;
			for (long num = 0L; num < size; num += (long)num2)
			{
				num2 = input.Read(array, 0, array.Length);
				output.Write(array, 0, num2);
			}
		}
		internal void Dump(string output)
		{
			using (FileStream fileStream = new FileStream(output, FileMode.Create, FileAccess.Write))
			{
				foreach (Ccl.Image current in this.Cci2Image.Images)
				{
					fileStream.Seek(current.m_loadAddress.GetInt64(), SeekOrigin.Begin);
					string path = Path.Combine(Path.GetDirectoryName(output), current.Path);
					using (FileStream fileStream2 = new FileStream(path, FileMode.Open, FileAccess.Read))
					{
						fileStream2.Seek(current.m_offset.GetInt64(), SeekOrigin.Begin);
						this.CopyStreamToStream(fileStream2, fileStream, current.m_size.GetInt64());
					}
				}
			}
		}
	}
}
