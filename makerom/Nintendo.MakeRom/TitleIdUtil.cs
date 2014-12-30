using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	public class TitleIdUtil
	{
		public enum CategoryMainName
		{
			Normal,
			DlpChild,
			Trial,
			Contents,
			AddOnContents,
			Patch,
			None
		}
		public enum CategoryFlagsName
		{
			CannotExecution,
			System,
			RequireBatchUpdate,
			NotRequireUserApproval,
			NotRequireRightForMount,
			CanSkipConvertJumpId
		}
		private const int CATEGORY_TYPE_MASK = 7;
		public static Dictionary<TitleIdUtil.CategoryMainName, uint> CategoryTypeToUIntTable = new Dictionary<TitleIdUtil.CategoryMainName, uint>
		{

			{
				TitleIdUtil.CategoryMainName.Normal,
				0u
			},

			{
				TitleIdUtil.CategoryMainName.DlpChild,
				1u
			},

			{
				TitleIdUtil.CategoryMainName.Trial,
				2u
			},

			{
				TitleIdUtil.CategoryMainName.Contents,
				3u
			},

			{
				TitleIdUtil.CategoryMainName.AddOnContents,
				4u
			},

			{
				TitleIdUtil.CategoryMainName.Patch,
				6u
			}
		};
		public static Dictionary<TitleIdUtil.CategoryFlagsName, uint> CategoryFlagsToUIntTable = new Dictionary<TitleIdUtil.CategoryFlagsName, uint>
		{

			{
				TitleIdUtil.CategoryFlagsName.CannotExecution,
				8u
			},

			{
				TitleIdUtil.CategoryFlagsName.System,
				16u
			},

			{
				TitleIdUtil.CategoryFlagsName.RequireBatchUpdate,
				32u
			},

			{
				TitleIdUtil.CategoryFlagsName.NotRequireUserApproval,
				64u
			},

			{
				TitleIdUtil.CategoryFlagsName.NotRequireRightForMount,
				128u
			},

			{
				TitleIdUtil.CategoryFlagsName.CanSkipConvertJumpId,
				256u
			}
		};
		public static uint MakeCategory(TitleIdUtil.CategoryMainName mainCategory, params TitleIdUtil.CategoryFlagsName[] flags)
		{
			uint num = TitleIdUtil.CategoryTypeToUIntTable[mainCategory];
			for (int i = 0; i < flags.Length; i++)
			{
				TitleIdUtil.CategoryFlagsName key = flags[i];
				num |= TitleIdUtil.CategoryFlagsToUIntTable[key];
			}
			return num;
		}
		public static bool IsAddOnContents(uint category)
		{
			return category == TitleIdUtil.MakeCategory(TitleIdUtil.CategoryMainName.AddOnContents, new TitleIdUtil.CategoryFlagsName[]
			{
				TitleIdUtil.CategoryFlagsName.CannotExecution,
				TitleIdUtil.CategoryFlagsName.NotRequireRightForMount
			});
		}
		public static bool IsAdditionContents(uint category)
		{
			return (category & 7u) == TitleIdUtil.CategoryTypeToUIntTable[TitleIdUtil.CategoryMainName.Contents];
		}
		public static bool IsDlpChild(uint category)
		{
			return (category & 7u) == TitleIdUtil.CategoryTypeToUIntTable[TitleIdUtil.CategoryMainName.DlpChild];
		}
		public static bool IsDemo(uint category)
		{
			return (category & 7u) == TitleIdUtil.CategoryTypeToUIntTable[TitleIdUtil.CategoryMainName.Trial];
		}
		public static bool IsPatch(uint category)
		{
			return (category & 7u) == TitleIdUtil.CategoryTypeToUIntTable[TitleIdUtil.CategoryMainName.Patch];
		}
		internal static ulong MakeTitleCodeImpl(MakeCxiOptions options, uint category)
		{
			ulong num = 0uL;
			num |= (ulong)category << 32;
			num |= (ulong)((ulong)((long)options.TitlePlatform) << 28);
			num |= (ulong)options.TitleUniqueId << 8;
			return num | (ulong)options.TitleVariation;
		}
		internal static ulong MakeTargetTitleCode(MakeCxiOptions options)
		{
			return TitleIdUtil.MakeTitleCodeImpl(options, options.TargetCategoryFlags);
		}
		internal static ulong MakeTitleCode(MakeCxiOptions options)
		{
			return TitleIdUtil.MakeTitleCodeImpl(options, options.CategoryFlags);
		}
		public static uint GetCategory(ulong programId)
		{
			return (uint)(programId >> 32 & 511uL);
		}
		internal static bool IsSystemCategory(uint category)
		{
			return (category & 16u) != 0u;
		}
		internal static UInt64Data MakeProgramId(MakeCxiOptions options)
		{
			UInt64Data data = 1125899906842624uL;
			return data | TitleIdUtil.MakeTitleCode(options);
		}
		internal static UInt64Data MakeTargetProgramId(MakeCxiOptions options)
		{
			UInt64Data data = 1125899906842624uL;
			return data | TitleIdUtil.MakeTargetTitleCode(options);
		}
	}
}
