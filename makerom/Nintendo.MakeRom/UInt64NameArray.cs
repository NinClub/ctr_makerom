using System;
namespace Nintendo.MakeRom
{
	internal class UInt64NameArray : BinaryArray<UInt64Name>
	{
		public UInt64NameArray(int size) : base(size)
		{
		}
		public UInt64NameArray(int size, string nameArrayText) : this(size)
		{
			char[] separator = new char[]
			{
				','
			};
			string[] array = nameArrayText.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string programIdDesc = array2[i];
				this.AddName(new UInt64Name(programIdDesc));
			}
		}
		protected void AddName(UInt64Name name)
		{
			for (int i = 0; i < base.Array.Length; i++)
			{
				if (base.Array[i].IsNull())
				{
					base.Array[i] = name;
					return;
				}
			}
			throw new IndexOutOfRangeException();
		}
	}
}
