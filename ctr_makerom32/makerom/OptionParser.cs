using System;
using System.Collections.Generic;
namespace makerom
{
	public class OptionParser
	{
		private List<Option> m_Options;
		public OptionParser()
		{
			this.m_Options = new List<Option>();
		}
		public void AddOption(Option option)
		{
			this.m_Options.Add(option);
		}
		public string[] Parse(string[] args)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < args.Length; i++)
			{
				bool flag = false;
				foreach (Option current in this.m_Options)
				{
					if (current.ShortName.Equals(args[i]))
					{
						if (current.IsFound && !current.AllowDuplicated)
						{
							throw new ArgumentException("duplicated option.", args[i]);
						}
						if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
						{
							this.CallOptionHandler(current, args[i], args[i + 1]);
							i++;
						}
						else
						{
							this.CallOptionHandler(current, args[i], null);
						}
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(args[i]);
				}
			}
			return list.ToArray();
		}
		private void CallOptionHandler(Option option, string name, string arg)
		{
			switch (option.ArgType)
			{
			case Option.ArgumentType.Non:
			case Option.ArgumentType.Optional:
				if (!option.OnFound(arg))
				{
					throw new ArgumentException("argument handler returns false", arg);
				}
				break;
			case Option.ArgumentType.Required:
				if (arg == null)
				{
					throw new ArgumentException(string.Format("argument is required after the option {0}.", name));
				}
				if (!option.OnFound(arg))
				{
					throw new ArgumentException("argument handler returns false", arg);
				}
				break;
			default:
				throw new ArgumentException("unknown ArgumentType", name);
			}
		}
	}
}
