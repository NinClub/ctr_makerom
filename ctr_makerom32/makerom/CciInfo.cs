using Nintendo.MakeRom;
using System;
using System.Collections.Generic;
namespace makerom
{
	public class CciInfo
	{
		public List<ContentsInfo> Contents
		{
			get;
			set;
		}
		public CciInfo()
		{
			this.Contents = new List<ContentsInfo>();
		}
	}
}
