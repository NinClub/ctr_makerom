using System;
using System.Collections.Generic;
using System.Text;
namespace nyaml
{
	internal class BlockScalar : Scalar
	{
		public List<string> Element
		{
			get;
			private set;
		}
		public override bool IsNullScalar
		{
			get
			{
				return false;
			}
		}
		public BlockScalar(string text)
		{
			this.Element = new List<string>();
			this.Element.Add(text);
		}
		public BlockScalar(int number)
		{
			this.Element = new List<string>();
			this.Element.Add(number.ToString());
		}
		public BlockScalar(long number)
		{
			this.Element = new List<string>();
			this.Element.Add(number.ToString());
		}
		public override void Dump(int indent)
		{
			foreach (string current in this.Element)
			{
				Console.WriteLine(new string(' ', indent) + current);
			}
		}
		public override bool GetBoolean()
		{
			throw new NotImplementedException();
		}
		public override string GetString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in this.Element)
			{
				stringBuilder.Append(current);
			}
			return stringBuilder.ToString();
		}
		public override int GetInteger()
		{
			throw new NotImplementedException();
		}
	}
}
