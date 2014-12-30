using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class InvalidPathFormatException : MakeFSException
	{
		private string m_Name;
		public override string Message
		{
			get
			{
				return string.Format("InvalidPathFormatException\n{0}\n", this.m_Name);
			}
		}
		public InvalidPathFormatException(string name) : base("InvalidPathFormat")
		{
			this.m_Name = name;
		}
	}
}
