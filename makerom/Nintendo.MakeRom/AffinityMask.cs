using System;
using System.Globalization;
namespace Nintendo.MakeRom
{
	internal class AffinityMask : ByteData
	{
		public AffinityMask(string affinityDesc)
		{
			if (affinityDesc.StartsWith("0x"))
			{
				base.Data = byte.Parse(affinityDesc.Substring("0x".Length), NumberStyles.AllowHexSpecifier);
				return;
			}
			base.Data = byte.Parse(affinityDesc);
		}
		public AffinityMask(int affinityDesc)
		{
			base.Data = (byte)affinityDesc;
		}
	}
}
