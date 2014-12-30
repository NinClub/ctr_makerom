using System;
namespace nyaml
{
	public class ScalarLong : Scalar
	{
		private long m_Value;
		public override bool IsNullScalar
		{
			get
			{
				return false;
			}
		}
		public ScalarLong(long value)
		{
			this.m_Value = value;
		}
		public override string ToString()
		{
			return string.Concat(this.m_Value);
		}
		public override long GetLong()
		{
			return this.m_Value;
		}
	}
}
