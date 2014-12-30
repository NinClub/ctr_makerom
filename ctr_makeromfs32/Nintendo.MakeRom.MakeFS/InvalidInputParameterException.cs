using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class InvalidInputParameterException : MakeFSException
	{
		private string m_Name;
		public override string Message
		{
			get
			{
				return string.Format("InvalidInputParameterException\n{0}\n", this.m_Name);
			}
		}
		public InvalidInputParameterException(string name) : base("InvalidInputParameterException")
		{
			this.m_Name = name;
		}
	}
}
