using System;
namespace Nintendo.MakeRom
{
	internal class AddressMappingException : Exception
	{
		public AddressMappingException()
		{
		}
		public AddressMappingException(string message) : base(message)
		{
		}
		public AddressMappingException(string message, Exception inner) : base(message)
		{
		}
	}
}
