using System;
using System.Collections.Generic;
using System.IO;
namespace Nintendo.MakeRom.Ncch.RomFs
{
	public class FileNameTable
	{
		public List<FileSystemInfo> NameEntryTable
		{
			get;
			private set;
		}
		public int NumFiles
		{
			get
			{
				return this.NameEntryTable.Count;
			}
		}
		internal FileNameTable(string rootPath, FileSearcher searcher)
		{
			this.NameEntryTable = new List<FileSystemInfo>();
			this.AddDirectory(new DirectoryInfo(rootPath), searcher);
		}
		internal void AddDirectory(DirectoryInfo dirInfo, FileSearcher searcher)
		{
			FileSystemInfo[] fileSystemInfos = searcher.GetFileSystemInfos(dirInfo);
			for (int i = 0; i < fileSystemInfos.Length; i++)
			{
				FileSystemInfo fileSystemInfo = fileSystemInfos[i];
				if ((fileSystemInfo.Attributes & FileAttributes.Directory) == (FileAttributes)0)
				{
					this.NameEntryTable.Add(fileSystemInfo);
				}
			}
			DirectoryInfo[] directoryInfos = searcher.GetDirectoryInfos(dirInfo);
			for (int j = 0; j < directoryInfos.Length; j++)
			{
				DirectoryInfo dirInfo2 = directoryInfos[j];
				this.AddDirectory(dirInfo2, searcher);
			}
		}
	}
}
