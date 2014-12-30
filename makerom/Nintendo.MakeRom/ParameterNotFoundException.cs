using System;
namespace Nintendo.MakeRom
{
	public class ParameterNotFoundException : MakeromException
	{
		private string m_Name;
		public override string Message
		{
			get
			{
				return string.Format("ParameterNotFound: {0}\n", this.m_Name);
			}
		}
		public ParameterNotFoundException(string name) : base(name)
		{
			this.m_Name = name;
		}
	}
}
