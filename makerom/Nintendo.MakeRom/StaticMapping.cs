using System;
using System.Globalization;
namespace Nintendo.MakeRom
{
	internal class StaticMapping : MappingDescriptor
	{
		public StaticMapping(string address, bool isReadOnly) : this(uint.Parse(address, NumberStyles.AllowHexSpecifier), isReadOnly)
		{
		}
		public StaticMapping(uint address, bool isReadOnly) : base(address, 2044u, 11, isReadOnly)
		{
		}
	}
}
