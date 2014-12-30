using System;
using System.IO;
namespace Nintendo.MakeRom
{
	public interface IWritableBinary
	{
		long Size
		{
			get;
		}
		void WriteBinary(BinaryWriter writer);
	}
}
