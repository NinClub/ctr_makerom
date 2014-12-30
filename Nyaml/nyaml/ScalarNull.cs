using System;
namespace nyaml
{
	public class ScalarNull : Scalar
	{
		public override bool IsNullScalar
		{
			get
			{
				return true;
			}
		}
		public override string ToString()
		{
			return null;
		}
		public override string GetString()
		{
			return null;
		}
	}
}
