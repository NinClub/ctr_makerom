using System;
namespace nyaml
{
	public class StackOverflowException : Exception
	{
		public StackOverflowException() : base("Stack Overflow\n")
		{
		}
	}
}
