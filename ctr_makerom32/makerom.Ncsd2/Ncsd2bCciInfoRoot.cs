using Nintendo.MakeRom.Ncch.FastBuildRomfs;
using System;
using System.Collections.Generic;
namespace makerom.Ncsd2
{
	public class Ncsd2bCciInfoRoot
	{
		public Ncsd2bCciInfo CciInfo
		{
			get;
			set;
		}
		public void LoadRomfsInfo(FastBuildRomFsInfo romfsInfo)
		{
			this.CciInfo = new Ncsd2bCciInfo();
			this.CciInfo.Contents = new List<Ncsd2bContentsInfo>();
			Ncsd2bContentsInfo ncsd2bContentsInfo = new Ncsd2bContentsInfo();
			ncsd2bContentsInfo.RomFsInfo = romfsInfo;
			this.CciInfo.Contents.Add(ncsd2bContentsInfo);
		}
	}
}
