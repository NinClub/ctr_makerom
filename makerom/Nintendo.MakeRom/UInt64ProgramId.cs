using System;
namespace Nintendo.MakeRom
{
	internal class UInt64ProgramId : UInt64Data
	{
		private const ulong INVALID_ID = 0uL;
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public UInt64ProgramId() : base(0uL)
		{
		}
		public UInt64ProgramId(ulong programId) : base(programId)
		{
		}
		public bool IsInvalidId()
		{
			return base.Data.Equals(0uL);
		}
		public string GetName()
		{
			uint num = (uint)(base.Data & 65535uL);
			char[] value = new char[]
			{
				(char)(num >> 24 & 255u),
				(char)(num >> 16 & 255u),
				(char)(num >> 8 & 255u),
				(char)(num & 255u)
			};
			return new string(value);
		}
		public override bool Equals(object obj)
		{
			if (obj.GetType() == typeof(UInt64ProgramId))
			{
				return base.Data == ((UInt64ProgramId)obj).Data;
			}
			return base.Equals(obj);
		}
	}
}
