using System;
using System.Text;
namespace nyaml
{
	public class SyntaxErrorException : Exception
	{
		public int Line
		{
			get;
			set;
		}
		public Token Token
		{
			get;
			set;
		}
		public string TokenValue
		{
			get;
			set;
		}
		public string FileName
		{
			get;
			set;
		}
		public override string Message
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SyntaxError\n");
				stringBuilder.Append("File     : " + this.FileName + "\n");
				stringBuilder.Append("Line     : " + this.Line + "\n");
				stringBuilder.Append("Token    : " + this.TokenValue + "\n");
				stringBuilder.Append("TokenType: " + this.Token + "\n");
				return stringBuilder.ToString();
			}
		}
		public SyntaxErrorException() : base("Syntax Error\n")
		{
			this.Line = 0;
			this.TokenValue = "";
			this.Token = Token.token_eof;
		}
	}
}
