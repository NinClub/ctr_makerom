using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
namespace Nintendo.MakeRom
{
	public class Util
	{
		private class Range
		{
			public uint Min
			{
				get;
				set;
			}
			public uint Max
			{
				get;
				set;
			}
			public Range(uint min, uint max)
			{
				this.Min = min;
				this.Max = max;
			}
			public bool IsIncluded(uint value)
			{
				return this.Min <= value && value <= this.Max;
			}
		}
		public static byte[] MakePaddingData(uint paddingSize, byte paddingChar)
		{
			if (paddingSize == 0u)
			{
				return new byte[0];
			}
			byte[] first = new byte[0];
			if (paddingSize > 2147483647u)
			{
				first = first.Concat(Enumerable.Repeat<byte>(paddingChar, 2147483647).ToArray<byte>()).ToArray<byte>();
				paddingSize -= 2147483647u;
			}
			return first.Concat(Enumerable.Repeat<byte>(paddingChar, (int)paddingSize).ToArray<byte>()).ToArray<byte>();
		}
		public static uint CalculatePaddingSize(int size, int align)
		{
			return Util.CalculatePaddingSize((long)size, align);
		}
		public static uint CalculatePaddingSize(long size, int align)
		{
			if (size % (long)align == 0L)
			{
				return 0u;
			}
			return (uint)((long)align - size % (long)align);
		}
		internal static void CheckTitleIdRange(MakeCxiOptions options)
		{
			Dictionary<MakeCxiOptions.TitleUse, Util.Range[]> source = new Dictionary<MakeCxiOptions.TitleUse, Util.Range[]>
			{

				{
					MakeCxiOptions.TitleUse.APPLICATION,
					new Util.Range[]
					{
						new Util.Range(768u, 1015807u),
						new Util.Range(1044480u, 1045503u)
					}
				},

				{
					MakeCxiOptions.TitleUse.SYSTEM,
					new Util.Range[]
					{
						new Util.Range(0u, 767u)
					}
				},

				{
					MakeCxiOptions.TitleUse.EVALUATION,
					new Util.Range[]
					{
						new Util.Range(1015808u, 1048575u)
					}
				}
			};
			Util.Range[] value = source.Single((KeyValuePair<MakeCxiOptions.TitleUse, Util.Range[]> pair) => pair.Key == options.Use).Value;
			if (value.FirstOrDefault((Util.Range range) => range.IsIncluded(options.TitleUniqueId)) == null)
			{
				Util.PrintWarning(string.Format("Unexpected UniqueId: {0:x}", options.TitleUniqueId));
				Util.Range[] array = value;
				for (int i = 0; i < array.Length; i++)
				{
					Util.Range range2 = array[i];
					Console.WriteLine(" Expected range : {0:x} - {1:x}", range2.Min, range2.Max);
				}
			}
		}
		public static uint Int64ToMediaUnitSize(long size, byte unitSizeLog)
		{
			uint num = 1u << (int)(unitSizeLog + 9);
			uint num2 = (uint)(size / (long)((ulong)num));
			if (size % (long)((ulong)num) != 0L)
			{
				num2 += 1u;
			}
			return num2;
		}
		public static long MediaUnitSizeToInt64(uint mediaUnitSize, byte unitSize)
		{
			return Util.GetMediaUnitByteSize(unitSize) * (long)((ulong)mediaUnitSize);
		}
		public static long GetMediaUnitByteSize(byte mediaUnitSize)
		{
			return 1L << (int)(mediaUnitSize + 9);
		}
		public static void CheckSize(ulong size, ulong limit)
		{
			if (size > limit)
			{
				throw new MakeromException(string.Format("Too large size\n Limit: {0}\n Current: {1}", 9223372036854775807L, size));
			}
		}
		internal static ulong MakePartitionId(MakeCxiOptions option)
		{
			if (option.PartitionId == 18446744073709551615uL)
			{
				return TitleIdUtil.MakeProgramId(option);
			}
			return option.PartitionId;
		}
		internal static bool IsOldProductCode(MakeCxiOptions option)
		{
			Match match = Regex.Match(option.ProductCode, "CTR-([A-Z0-9]){1}-([A-Z0-9]){4}[ ]\\(([A-Z]){3}\\)");
			return match.Success;
		}
		internal static bool IsValidProductCode(MakeCxiOptions option)
		{
			if (option.ProductCode.Length > 16)
			{
				return false;
			}
			if (option.FreeProductCode)
			{
				return true;
			}
			Match match = Regex.Match(option.ProductCode, "CTR-([A-Z0-9]){1}-([A-Z0-9]){4}");
			return match.Success;
		}
		public static void PrintWarning(string message)
		{
			Warning.PrintWarning(message, 255);
		}
		public static byte[] GetKeyData(string keyString)
		{
			if (keyString.Length != 32)
			{
				throw new MakeromException("Key data length must be 32");
			}
			byte[] array = new byte[16];
			for (int i = 0; i < 16; i++)
			{
				string text = keyString.Substring(i * 2, 2);
				byte b = 0;
				if (!byte.TryParse(text, NumberStyles.AllowHexSpecifier, null, out b))
				{
					throw new MakeromException("Aes key string has invalid char: " + text);
				}
				array[i] = b;
			}
			return array;
		}
		public static ulong SizeStringToUInt64(string sizeString)
		{
			Dictionary<string, ulong> dictionary = new Dictionary<string, ulong>
			{

				{
					"kb",
					1024uL
				},

				{
					"mb",
					1048576uL
				},

				{
					"gb",
					1073741824uL
				}
			};
			Match match = Regex.Match(sizeString, "^(\\d+)([a-zA-Z]+)\\z");
			if (!match.Success)
			{
				throw new MakeromException(string.Format("Invalid string format: {0}", sizeString));
			}
			string key = match.Groups[2].Value.ToLower();
			if (!dictionary.ContainsKey(key))
			{
				throw new MakeromException(string.Format("Invalid string format: {0}", sizeString));
			}
			return ulong.Parse(match.Groups[1].Value) * dictionary[key];
		}
	}
}
