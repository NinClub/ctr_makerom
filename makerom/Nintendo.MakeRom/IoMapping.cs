using System;
using System.Globalization;
namespace Nintendo.MakeRom
{
	internal class IoMapping : MappingDescriptor
	{
		public IoMapping(string address) : this(uint.Parse(address, NumberStyles.AllowHexSpecifier))
		{
		}
		public IoMapping(uint address) : base(address, 4094u, 12, false)
		{
		}
	}
}
