using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class DulicatedPathException : MakeFSException
	{
		private string m_Name;
		public override string Message
		{
			get
			{
				return string.Format("DulicatedPathException\n{0}\n", this.m_Name);
			}
		}
		public DulicatedPathException(string name) : base("DulicatedPathException")
		{
			this.m_Name = name;
		}
	}
}
