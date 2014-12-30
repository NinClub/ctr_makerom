using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace makerom.Ncsd2
{
	[XmlRoot(ElementName = "CciInfo")]
	public class Ncsd2bCciInfo
	{
		[XmlArrayItem(ElementName = "ContentsInfo")]
		public List<Ncsd2bContentsInfo> Contents
		{
			get;
			set;
		}
	}
}
