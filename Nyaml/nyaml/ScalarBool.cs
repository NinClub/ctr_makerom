using System;
namespace nyaml
{
	public class ScalarBool : Scalar
	{
		private bool m_Value;
		public override bool IsNullScalar
		{
			get
			{
				return false;
			}
		}
		public ScalarBool(bool value)
		{
			this.m_Value = value;
		}
		public override string ToString()
		{
			return this.m_Value.ToString();
		}
		public override bool GetBoolean()
		{
			return this.m_Value;
		}
	}
}
