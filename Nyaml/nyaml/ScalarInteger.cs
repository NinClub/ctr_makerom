using System;
namespace nyaml
{
	public class ScalarInteger : Scalar
	{
		private int m_Value;
		public override bool IsNullScalar
		{
			get
			{
				return false;
			}
		}
		public ScalarInteger(int value)
		{
			this.m_Value = value;
		}
		public override string ToString()
		{
			return string.Concat(this.m_Value);
		}
		public override int GetInteger()
		{
			return this.m_Value;
		}
	}
}
