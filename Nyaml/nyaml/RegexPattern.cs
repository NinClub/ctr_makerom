using System;
namespace nyaml
{
	public class RegexPattern
	{
		private static readonly string CHAR_LETTERS = "\\d\\w-:<>\"'\\\\_\\.~|/+=$#\\{\\}\\(\\)\\*\\?@";
		private static readonly string REGEX_LETTERS = "\\[\\]\\^\\\\";
		public static readonly string REGEX_CHAR = "[" + RegexPattern.CHAR_LETTERS + "]";
		public static readonly string REGEX_MACRO = "\\$\\(([A-Za-z0-9_]+)\\)";
		public static readonly string REGEX_TOKEN = RegexPattern.REGEX_CHAR + "+([\\x20\\t]|\\z)";
		public static readonly string REGEX_TOKEN_NUMBER = "((^\\d+\\z)|(^0x[\\da-fA-F]+\\z))";
		public static readonly string REGEX_TOKEN_LONG_NUMBER = "((^\\d+(l|L)\\z)|(^0x[\\da-fA-F]+(l|L)\\z))";
		public static readonly string REGEX_TOKEN_TEXT = RegexPattern.REGEX_CHAR + "+\\z";
		public static readonly string REGEX_REGEX_CHAR = "[" + RegexPattern.CHAR_LETTERS + RegexPattern.REGEX_LETTERS + "]";
	}
}
