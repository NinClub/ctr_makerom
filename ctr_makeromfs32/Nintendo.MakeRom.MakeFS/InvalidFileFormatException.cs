using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class InvalidFileFormatException : MakeFSException
	{
		public InvalidFileFormatException() : base("InvalidFileFormatException")
		{
		}
	}
}
