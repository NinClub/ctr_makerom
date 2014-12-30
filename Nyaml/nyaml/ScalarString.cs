using System;
namespace nyaml
{
	public class ScalarString : Scalar
	{
		private string m_Value;
		public override bool IsNullScalar
		{
			get
			{
				return false;
			}
		}
		public ScalarString(string value)
		{
			this.m_Value = value;
		}
		public override string ToString()
		{
			return this.m_Value;
		}
		public override string GetString()
		{
			return this.m_Value;
		}
	}
}
