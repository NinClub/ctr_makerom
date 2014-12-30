using System;
namespace Nintendo.MakeRom
{
	internal class UInt64ProgramIdArray : BinaryArray<UInt64ProgramId>
	{
		public UInt64ProgramIdArray(int size) : base(size)
		{
		}
		public void AddProgramId(UInt64ProgramId programId)
		{
			for (int i = 0; i < base.Array.Length; i++)
			{
				if (base.Array[i].IsInvalidId())
				{
					base.Array[i] = programId;
					return;
				}
			}
			throw new IndexOutOfRangeException();
		}
	}
}
