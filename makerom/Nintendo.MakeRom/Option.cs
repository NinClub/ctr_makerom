using System;
using System.Text.RegularExpressions;
namespace Nintendo.MakeRom
{
	public class Option
	{
		public enum ArgumentType
		{
			Non,
			Optional,
			Required
		}
		public delegate bool OnFoundFunc(string val);
		private readonly Option.OnFoundFunc m_FoundFunc;
		public string ShortName
		{
			get;
			private set;
		}
		public string LongName
		{
			get;
			private set;
		}
		public bool IsFound
		{
			get;
			private set;
		}
		public bool IsOptional
		{
			get;
			private set;
		}
		public bool AllowDuplicated
		{
			get;
			private set;
		}
		public Option.ArgumentType ArgType
		{
			get;
			private set;
		}
		public Option(string shortName, string longName, Option.OnFoundFunc foundFunc, bool isOptional) : this(shortName, longName, foundFunc, isOptional, false)
		{
		}
		public Option(string shortName, string longName, Option.OnFoundFunc foundFunc, bool isOptional, bool allowDuplicated)
		{
			this.ShortName = this.NormalizeName(shortName, "-");
			this.LongName = this.NormalizeName(longName, "--");
			this.m_FoundFunc = foundFunc;
			this.IsOptional = isOptional;
			this.IsFound = false;
			this.AllowDuplicated = allowDuplicated;
			this.ArgType = (Option.ArgumentType)Math.Max((int)this.NameToArgumentType(shortName), (int)this.NameToArgumentType(longName));
		}
		public bool OnFound(string value)
		{
			this.IsFound = true;
			return this.m_FoundFunc(value);
		}
		private string NormalizeName(string name, string prefix)
		{
			string[] array = name.Split(new char[]
			{
				' '
			});
			switch (array.Length)
			{
			case 1:
			case 2:
				if (array[0].StartsWith(prefix))
				{
					return array[0];
				}
				throw new ArgumentException("invalid option prefix.", name);
			default:
				throw new ArgumentException("invalid option prefix.", name);
			}
		}
		private Option.ArgumentType NameToArgumentType(string name)
		{
			string[] array = name.Split(new char[]
			{
				' '
			});
			switch (array.Length)
			{
			case 1:
				return Option.ArgumentType.Non;
			case 2:
			{
				Regex regex = new Regex("^[.*]$");
				if (regex.IsMatch(array[1]))
				{
					return Option.ArgumentType.Optional;
				}
				return Option.ArgumentType.Required;
			}
			default:
				throw new ArgumentException("invalid option argument.", name);
			}
		}
	}
}
