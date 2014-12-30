using System;
using System.Text;
namespace nyaml
{
	public class WrongCastException : Exception
	{
		private Type m_SrcType;
		private Type m_DstType;
		private string m_Value;
		public override string Message
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("WrongCast\n");
				stringBuilder.Append(string.Format("Cannot cast {0} to {1}\n", this.m_SrcType, this.m_DstType, this.m_Value));
				stringBuilder.Append(string.Format("Value = {0}\n", this.m_Value));
				return stringBuilder.ToString();
			}
		}
		public WrongCastException(object src, Type dstType) : base("Wrong cast\n")
		{
			this.m_DstType = dstType;
			this.m_SrcType = src.GetType();
			Scalar scalar = (Scalar)src;
			if (this.m_SrcType == typeof(ScalarBool))
			{
				this.m_Value = scalar.GetBoolean().ToString();
				return;
			}
			if (this.m_SrcType == typeof(ScalarInteger))
			{
				this.m_Value = scalar.GetInteger().ToString();
				return;
			}
			if (this.m_SrcType == typeof(ScalarString))
			{
				this.m_Value = scalar.GetString().ToString();
				return;
			}
			if (this.m_SrcType == typeof(ScalarNull))
			{
				this.m_Value = "(null)";
			}
		}
	}
}
