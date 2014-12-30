using System;
namespace Nintendo.MakeRom
{
	public class UnknownParameterException : MakeromException
	{
		private string m_NotIncludedKey;
		public override string Message
		{
			get
			{
				return string.Format("UnknownParameter: {0}\n", this.m_NotIncludedKey);
			}
		}
		public UnknownParameterException(string notIncludedKey) : base("UnknownParameter")
		{
			this.m_NotIncludedKey = notIncludedKey;
		}
	}
}
