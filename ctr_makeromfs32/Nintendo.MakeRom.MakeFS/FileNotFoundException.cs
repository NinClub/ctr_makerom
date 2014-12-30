using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class FileNotFoundException : MakeFSException
	{
		private string m_Name;
		public override string Message
		{
			get
			{
				return string.Format("FileNotFoundException\n{0}\n", this.m_Name);
			}
		}
		public FileNotFoundException(string name) : base("FileNotFoundException")
		{
			this.m_Name = name;
		}
	}
}
