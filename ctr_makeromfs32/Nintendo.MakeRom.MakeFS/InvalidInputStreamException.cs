using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class InvalidInputStreamException : MakeFSException
	{
		private string m_Name;
		public override string Message
		{
			get
			{
				return string.Format("InvalidInputStreamException\n{0}\n", this.m_Name);
			}
		}
		public InvalidInputStreamException(string name) : base("InvalidInputStreamException")
		{
			this.m_Name = name;
		}
	}
}
