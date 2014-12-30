using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Nintendo.MakeRom
{
	public class RomFsInfo
	{
		public string Root
		{
			get;
			set;
		}
		[XmlArrayItem(ElementName = "Name")]
		public List<string> Files
		{
			get;
			set;
		}
		public RomFsInfo()
		{
			this.Files = new List<string>();
		}
		public void AddFile(string fullpath)
		{
			string item = fullpath.Replace(this.Root, "");
			this.Files.Add(item);
		}
	}
}
