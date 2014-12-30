using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class InvalidOutputStreamException : MakeFSException
	{
		private string m_Name;
		public override string Message
		{
			get
			{
				return string.Format("InvalidOutputStreamException\n{0}\n", this.m_Name);
			}
		}
		public InvalidOutputStreamException(string name) : base("InvalidOutputStreamException")
		{
			this.m_Name = name;
		}
	}
}
