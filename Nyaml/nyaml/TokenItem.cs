using System;
namespace nyaml
{
	internal class TokenItem
	{
		public Token Token
		{
			get;
			private set;
		}
		public string Value
		{
			get;
			private set;
		}
		public int Line
		{
			get;
			private set;
		}
		public TokenItem(Token token, string value, int line)
		{
			this.Token = token;
			this.Value = value;
			this.Line = line;
		}
	}
}
