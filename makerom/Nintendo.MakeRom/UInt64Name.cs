using System;
using System.Globalization;
using System.Text;
namespace Nintendo.MakeRom
{
	internal class UInt64Name : UInt64Data
	{
		private const string NULL_STRING = "";
		public UInt64Name(string programIdDesc)
		{
			if (programIdDesc == null)
			{
				programIdDesc = "";
			}
			if (programIdDesc.StartsWith("0x"))
			{
				ulong.Parse(programIdDesc, NumberStyles.AllowHexSpecifier);
				return;
			}
			base.Data = programIdDesc.PadRight(8, '\0').ToUInt64ASCII();
		}
		public UInt64Name() : this("")
		{
		}
		public bool IsNull()
		{
			return this.Equals(new UInt64Name(""));
		}
		public bool Equals(UInt64Name id)
		{
			return id.Data == base.Data;
		}
		public override string ToString()
		{
			byte[] bytes = BitConverter.GetBytes(base.Data);
			int num = 0;
			while (num < bytes.Length && bytes[num] != 0)
			{
				num++;
			}
			return Encoding.ASCII.GetString(bytes, 0, num);
		}
	}
}
