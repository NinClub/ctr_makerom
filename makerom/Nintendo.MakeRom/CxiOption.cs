using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	public class CxiOption
	{
		public string TopExefsSectionName
		{
			get;
			set;
		}
		public string DescPath
		{
			get;
			set;
		}
		public string RsfPath
		{
			get;
			set;
		}
		public string ElfPath
		{
			get;
			set;
		}
		public string BannerPath
		{
			get;
			set;
		}
		public string IconPath
		{
			get;
			set;
		}
		public string DotRomfsPath
		{
			get;
			set;
		}
		public string RomfsLayoutPath
		{
			get;
			set;
		}
		public byte[] AesKey
		{
			get;
			set;
		}
		public long ExefsReserveSize
		{
			get;
			set;
		}
		public bool Cip
		{
			get;
			set;
		}
		public int Align
		{
			get;
			set;
		}
		public Dictionary<string, string> UserVariables
		{
			get;
			set;
		}
		public bool OutputCoreInfo
		{
			get;
			set;
		}
		public bool Cdi
		{
			get;
			set;
		}
		public bool Cfa
		{
			get;
			set;
		}
		public bool Caa
		{
			get;
			set;
		}
		public bool CipCompress
		{
			get;
			set;
		}
		public bool ForCard
		{
			get;
			set;
		}
		public bool ForCci2
		{
			get;
			set;
		}
		public byte MediaUnitSize
		{
			get;
			set;
		}
		public byte DependencyVariation
		{
			get;
			set;
		}
		public CxiOption()
		{
			this.Cip = false;
			this.Cdi = false;
			this.Cfa = false;
			this.Caa = false;
			this.Align = 512;
			this.OutputCoreInfo = false;
			this.TopExefsSectionName = ".code";
			this.DependencyVariation = 0;
			this.ExefsReserveSize = 0L;
			this.CipCompress = false;
		}
	}
}
