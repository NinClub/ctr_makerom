using System;
namespace nyaml
{
	internal class Program
	{
		private void Test()
		{
		}
		private static void Main(string[] args)
		{
			string[] inputs = new string[]
			{
				"File: $(FILE), Version: $(VERSION)"
			};
			Nyaml.PreProcessImpl(inputs, null);
		}
	}
}
