using System;
using System.Security.Cryptography;
namespace Nintendo.MakeRom.Ncch.RomFs
{
	internal interface IRomFsBinary : IWritableBinary
	{
		RomFsInfo RomFsInfo
		{
			get;
		}
		ICryptoTransform CryptoTransform
		{
			get;
			set;
		}
		byte[] GetSuperBlockHash();
		uint GetHashRegionSize();
	}
}
