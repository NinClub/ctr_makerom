using System;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	internal class Compress
	{
		private const int COMPRESS_LARGER_ORIGINAL = -1;
		private const int COMPRESS_FATAL_ERROR = -2;
		private const int COMPRESSED_SIZE_OVER_LIMIT = -3;
		private const uint MAX_COMPRESSED_SIZE = 16777215u;
		private const int LOADER_SIZE_ARM9 = 16384;
		private const int LOADER_SIZE_ARM7 = 1024;
		private const int LZ_BIT_INDEX = 12;
		private const int LZ_BIT_LENGTH = 4;
		private const int LZ_MAX_INDEX = 4096;
		private const int LZ_MAX_LENGTH = 16;
		private const int LZ_MIN_COPY = 3;
		private const int LZ_MAX_COPY = 18;
		private const int LZ_MAX_DIC_LENGTH = 4098;
		public unsafe static int CompressData(byte[] data)
		{
			fixed (byte* ptr = data)
			{
				return Compress.DoCompress(ptr, data.Length);
			}
		}
		private static int RoundUp(int value, int b)
		{
			return value + b - 1 & ~(b - 1);
		}
		private unsafe static int DoCompress(byte* buffer_original, int buffer_original_size)
		{
			if ((uint)buffer_original % 4u != 0u)
			{
				throw new Exception("Top of buffer is not aligned by 4.\n");
			}
			byte[] array = new byte[buffer_original_size];
			fixed (byte* ptr = &array[0])
			{
				byte* ptr2 = buffer_original;
				byte* ptr3 = ptr;
				int num = Compress.LZCompressRV(ptr2, buffer_original_size, ptr3, buffer_original_size);
				int result;
				if (num < 0)
				{
					result = -1;
				}
				else
				{
					int num2 = buffer_original_size - num;
					ptr3 += num;
					int num3;
					int num4;
					if (!Compress.CheckOverwrite(buffer_original_size, ptr3, num2, out num3, out num4))
					{
						ptr2 += num3;
						int num5 = buffer_original_size - num3;
						ptr3 += num4;
						num2 -= num4;
					}
					int num6 = num3 + num2;
					int num7 = Compress.RoundUp(num6, 4);
					int num8 = num7 + sizeof(CompFooter);
					if (buffer_original_size <= num8)
					{
						result = -1;
					}
					else
					{
						Marshal.Copy(array, (int)(ptr3 - ptr), new IntPtr((void*)ptr2), num2);
						for (int i = num6; i < num7; i++)
						{
							buffer_original[i] = 255;
						}
						CompFooter* ptr4 = (CompFooter*)(buffer_original + num7);
						uint num9 = (uint)(num8 - num3);
						if (num9 <= 16777215u)
						{
							ptr4->bufferTopAndBottom = ((num9 & 16777215u) | (uint)((uint)(255 & num8 - num6) << 24));
							ptr4->originalBottom = (uint)(buffer_original_size - num8);
							//ptr = null;
							return num8;
						}
						result = -3;
					}
				}
				return result;
			}
		}
		private unsafe static int LZCompressRV(byte* src_buffer, int src_size, byte* dst_buffer, int dst_size)
		{
			int i = src_size;
			int num = dst_size;
			while (i > 0)
			{
				if (num < 1)
				{
					return -1;
				}
				int num2 = 0;
				int num3;
				num = (num3 = num - 1);
				for (int j = 0; j < 8; j++)
				{
					num2 <<= 1;
					if (i > 0)
					{
						byte* ptr = src_buffer + i;
						int val = src_size - i;
						int num4 = Math.Min(i, 18);
						byte* src_buffer2 = ptr - num4;
						int num6;
						int num5 = Compress.FindMatched(src_buffer2, num4, ptr, Math.Min(val, 4098), out num6);
						if (num5 >= 3)
						{
							if (num < 2)
							{
								return -1;
							}
							i -= num5;
							num6 -= 2;
							num5 -= 3;
							ushort num7 = (ushort)((num6 & 4095) | num5 << 12);
							dst_buffer[--num] = (byte)(num7 >> 8 & 255);
							dst_buffer[--num] = (byte)(num7 & 255);
							num2 |= 1;
						}
						else
						{
							if (num < 1)
							{
								return -1;
							}
							dst_buffer[--num] = src_buffer[--i];
						}
					}
				}
				dst_buffer[num3] = (byte)num2;
			}
			return num;
		}
		private unsafe static int FindMatched(byte* src_buffer, int src_size, byte* dic_buffer, int dic_size, out int index)
		{
			byte* ptr = src_buffer + src_size - 1;
			byte b = *ptr;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < dic_size; i++)
			{
				if (b == dic_buffer[i])
				{
					int num3 = Compress.HowManyMatched(ptr, dic_buffer + i, Math.Min(i + 1, src_size));
					if (num < num3)
					{
						num = num3;
						num2 = i;
					}
				}
			}
			index = num2;
			return num;
		}
		private unsafe static int HowManyMatched(byte* src_buffer, byte* dic_buffer, int max_len)
		{
			int num = 0;
			while (num < max_len && *src_buffer == *dic_buffer)
			{
				src_buffer -= 1;
				dic_buffer -= 1;
				num++;
			}
			return num;
		}
		private unsafe static bool CheckOverwrite(int orig_size, byte* cmprs_buffer, int cmprs_buffer_size, out int orig_safe, out int cmprs_safe)
		{
			int num = cmprs_buffer_size;
			int i = orig_size;
			while (i > 0)
			{
				int num2 = (int)cmprs_buffer[--num];
				for (int j = 0; j < 8; j++)
				{
					if (i > 0)
					{
						if ((num2 & 128) != 0)
						{
							num -= 2;
							ushort num3 = (ushort)((int)cmprs_buffer[num] | (int)cmprs_buffer[num + 1] << 8);
							int num4 = (num3 >> 12 & 15) + 3;
							i -= num4;
							if (i < 0)
							{
								throw new Exception("System error in CheckOverwrite???\n");
							}
							if (i < num)
							{
								orig_safe = i;
								cmprs_safe = num;
								return false;
							}
						}
						else
						{
							num--;
							i--;
						}
						num2 <<= 1;
					}
				}
			}
			orig_safe = 0;
			cmprs_safe = 0;
			return true;
		}
	}
}
