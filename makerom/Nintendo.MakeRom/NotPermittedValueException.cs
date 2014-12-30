using System;
namespace Nintendo.MakeRom
{
	public class NotPermittedValueException : MakeromException
	{
		private string m_Name;
		private string m_Value;
		public override string Message
		{
			get
			{
				return string.Format("NotPermittedValue\n{0}: {1}\n", this.m_Name, this.m_Value);
			}
		}
		public NotPermittedValueException(string name, string value) : base("NotPermittedValue")
		{
			this.m_Name = name;
			this.m_Value = value;
		}
	}
}
