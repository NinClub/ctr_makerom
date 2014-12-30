using System;
using System.Collections.Generic;
using System.IO;
namespace Nintendo.MakeRom.MakeFS
{
	public class Layouter
	{
		public class Input
		{
			public string m_PCName;
			public uint m_SizeAlign;
		}
		public class Output
		{
			public string m_PCName;
			public ulong m_Offset;
			public ulong m_Size;
		}
		public static Layouter.Output[] Create(Layouter.Input[] inData)
		{
			List<Layouter.Output> list = new List<Layouter.Output>();
			ulong num = 0uL;
			for (int i = 0; i < inData.Length; i++)
			{
				Layouter.Input input = inData[i];
				Layouter.Output output = new Layouter.Output();
				FileInfo fileInfo = new FileInfo(input.m_PCName);
				num = (num + (ulong)input.m_SizeAlign - 1uL) / (ulong)input.m_SizeAlign * (ulong)input.m_SizeAlign;
				output.m_PCName = input.m_PCName;
				output.m_Offset = num;
				output.m_Size = (ulong)fileInfo.Length;
				list.Add(output);
				num += (ulong)fileInfo.Length;
			}
			return list.ToArray();
		}
	}
}
