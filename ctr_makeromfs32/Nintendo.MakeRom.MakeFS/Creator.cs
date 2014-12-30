using System;
using System.IO;
using System.Linq;
namespace Nintendo.MakeRom.MakeFS
{
	public class Creator
	{
		public class Input
		{
			public string m_PCName;
			public ulong m_Offset;
			public ulong m_Size;
		}
		private static void InsertPadding(Stream output, ulong size)
		{
			if (size > 0uL)
			{
				if (size <= 65536uL)
				{
					byte[] buffer = new byte[size];
					output.Write(buffer, 0, (int)size);
					return;
				}
				byte[] buffer2 = new byte[65536];
				while (size > 0uL)
				{
					int num = (int)((size < 65536uL) ? size : 65536uL);
					output.Write(buffer2, 0, num);
					size -= (ulong)((long)num);
				}
			}
		}
		private static void CopyStream(Stream output, Stream input, ulong size)
		{
			if (size > 0uL)
			{
				if (size < 65536uL)
				{
					byte[] buffer = new byte[size];
					input.Read(buffer, 0, (int)size);
					output.Write(buffer, 0, (int)size);
					return;
				}
				byte[] buffer2 = new byte[65536];
				while (size > 0uL)
				{
					int num = (int)((size < 65536uL) ? size : 65536uL);
					input.Read(buffer2, 0, num);
					output.Write(buffer2, 0, num);
					size -= (ulong)((long)num);
				}
			}
		}
		public static void Create(Stream outStream, Creator.Input[] inData, Stream inStream, ulong sizeInStream)
		{
			Creator.CopyStream(outStream, inStream, sizeInStream);
			ulong num = 0uL;
			for (int i = 0; i < inData.Length; i++)
			{
				Creator.Input input = inData[i];
				ulong num2 = input.m_Offset - num;
				if (input.m_Offset < num)
				{
					throw new InvalidLayoutException(input.m_PCName);
				}
				if (num2 > 0uL)
				{
					Creator.InsertPadding(outStream, num2);
					num += num2;
				}
				FileStream fileStream = new FileStream(input.m_PCName, FileMode.Open, FileAccess.Read);
				Creator.CopyStream(outStream, fileStream, input.m_Size);
				fileStream.Close();
				num += input.m_Size;
			}
		}
		public static long GetDatablockLength(Creator.Input[] inData, long entrySize)
		{
			if (inData.Length == 0)
			{
				return entrySize;
			}
			return entrySize + (long)inData.Last<Creator.Input>().m_Offset + (long)inData.Last<Creator.Input>().m_Size;
		}
	}
}
