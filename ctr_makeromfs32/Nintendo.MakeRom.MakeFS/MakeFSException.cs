using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class MakeFSException : Exception
	{
		public MakeFSException(string name) : base(name)
		{
		}
	}
}
