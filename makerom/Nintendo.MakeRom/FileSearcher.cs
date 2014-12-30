using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace Nintendo.MakeRom
{
	internal class FileSearcher
	{
		private const string CRR_DIR_PATTERN = "^/\\.crr(/.*)?\\z";
		private List<string> m_WhiteList = new List<string>();
		private List<string> m_BlackList = new List<string>();
		private List<string> m_IncludeList = new List<string>();
		private string m_Root;
		public FileSearcher(MakeCxiOptions options)
		{
			this.m_Root = Path.GetFullPath(options.RomFsRoot);
			if (options.Files != null)
			{
				this.m_WhiteList.AddRange(options.Files);
			}
			else
			{
				this.m_WhiteList.Add("*");
			}
			if (options.RejectFiles != null)
			{
				this.m_BlackList.AddRange(options.RejectFiles);
			}
			if (options.DefaultRejectFiles != null)
			{
				this.m_BlackList.AddRange(options.DefaultRejectFiles);
			}
			if (options.IncludeFiles != null)
			{
				this.m_IncludeList.AddRange(options.IncludeFiles);
			}
		}
		private static string WildCardToRegex(Match target)
		{
			string value = target.Value;
			if (value.Equals("?"))
			{
				return ".";
			}
			if (value.Equals("*"))
			{
				return ".*";
			}
			return Regex.Escape(value);
		}
		private static bool IsNewSpecifiedPattern(string pattern)
		{
			return pattern[0] == '/' || pattern[0] == '>';
		}
		private static string GetNewPatternRegex(string pattern)
		{
			if (FileSearcher.IsNewSpecifiedPattern(pattern))
			{
				string text;
				if (pattern[0] == '/')
				{
					if (pattern.Contains('\\'))
					{
						throw new MakeromException(string.Format("\"{0}\" is invalid. Path separeter must be '/'. ", pattern));
					}
					text = Regex.Replace(pattern, ".", new MatchEvaluator(FileSearcher.WildCardToRegex));
					text = "^" + text + "\\z";
				}
				else
				{
					text = pattern.Substring(1);
				}
				return text;
			}
			return null;
		}
		public FileSystemInfo[] GetFileSystemInfos(DirectoryInfo root)
		{
			List<FileSystemInfo> list = new List<FileSystemInfo>();
			foreach (string current in this.m_WhiteList)
			{
				list.AddRange(root.GetFileSystemInfos(current));
			}
			foreach (string black in this.m_BlackList)
			{
				string regex = FileSearcher.GetNewPatternRegex(black);
				if (regex != null)
				{
					list.RemoveAll(delegate(FileSystemInfo match)
					{
						if (this.IsCrrDirectory(match.FullName))
						{
							return false;
						}
						string text = match.FullName.Replace(this.m_Root, "");
						text = text.Replace('\\', '/');
						return Regex.Match(text, regex).Success;
					});
				}
				else
				{
					list.RemoveAll(delegate(FileSystemInfo match)
					{
						if (this.IsCrrDirectory(match.FullName))
						{
							return false;
						}
						string text = Regex.Replace(black, ".", new MatchEvaluator(FileSearcher.WildCardToRegex));
						text = "^" + text + "\\z";
						return Regex.Match(match.Name, text).Success;
					});
				}
			}
			foreach (string include in this.m_IncludeList)
			{
				list.ForEach(delegate(FileSystemInfo info)
				{
					string newPatternRegex = FileSearcher.GetNewPatternRegex(include);
					if (newPatternRegex == null)
					{
						throw new MakeromException(string.Format("Include pattern \"{0}\" is invalid. \n", include));
					}
					string text = info.FullName.Replace(this.m_Root, "");
					text = text.Replace('\\', '/');
					if (!Regex.Match(text, newPatternRegex).Success)
					{
						throw new MakeromException(string.Format("Not found \"{0}\" in \"{1}\"", include, this.m_Root));
					}
				});
			}
			return list.ToArray();
		}
		private bool IsCrrDirectory(string path)
		{
			string text = path.Replace(this.m_Root, "");
			text = text.Replace('\\', '/');
			return Regex.Match(text, "^/\\.crr(/.*)?\\z").Success;
		}
		public DirectoryInfo[] GetDirectoryInfos(DirectoryInfo root)
		{
			List<DirectoryInfo> list = new List<DirectoryInfo>();
			foreach (string current in this.m_WhiteList)
			{
				list.AddRange(root.GetDirectories(current));
			}
			foreach (string black in this.m_BlackList)
			{
				list.RemoveAll(delegate(DirectoryInfo match)
				{
					if (this.IsCrrDirectory(match.FullName))
					{
						return false;
					}
					string text = Regex.Replace(black, ".", new MatchEvaluator(FileSearcher.WildCardToRegex));
					text = "^" + text + "\\z";
					return Regex.Match(match.Name, text).Success;
				});
			}
			return list.ToArray();
		}
	}
}
