using System;
namespace Nintendo.MakeRom
{
	public class InvalidParameterException : MakeromException
	{
		private string m_ParameterName;
		private string m_Value;
		public override string Message
		{
			get
			{
				return string.Format("InvalidParameter\n Name: {0}\n Value: {1}\n", this.m_ParameterName, this.m_Value);
			}
		}
		public InvalidParameterException(string parameterName, string value) : base("InvalidParameter")
		{
			this.m_ParameterName = parameterName;
			this.m_Value = value;
		}
	}
}
