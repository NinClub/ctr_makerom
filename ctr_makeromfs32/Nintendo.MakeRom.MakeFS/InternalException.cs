using System;
namespace Nintendo.MakeRom.MakeFS
{
	public class InternalException : MakeFSException
	{
		private uint m_Value;
		public override string Message
		{
			get
			{
				return string.Format("InternalException\ncode={0,8:X8}\n", this.m_Value);
			}
		}
		public InternalException(uint val) : base("InternalException")
		{
			this.m_Value = val;
		}
	}
}
