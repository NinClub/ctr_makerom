using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class InvalidLayoutException : MakeFSException
	{
		private string m_Name;
		public override string Message
		{
			get
			{
				return string.Format("InvalidLayout\n{0}\n", this.m_Name);
			}
		}
		public InvalidLayoutException(string name) : base("InvalidLayout")
		{
			this.m_Name = name;
		}
	}
}
