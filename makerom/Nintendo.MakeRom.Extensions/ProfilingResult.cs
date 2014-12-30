using System;
using System.Collections.Generic;
using System.Linq;
namespace Nintendo.MakeRom.Extensions
{
	internal class ProfilingResult
	{
		private const string Sepalate = "##";
		private Dictionary<string, long> m_result = new Dictionary<string, long>();
		public string GetPathName(Stack<ProfilingDepth> depth, string name)
		{
			string str = "";
			foreach (ProfilingDepth current in depth.Reverse<ProfilingDepth>())
			{
				str = str + current.Name + "##";
			}
			return str + name;
		}
		public void Append(string depthName, long elapse)
		{
			if (this.m_result.ContainsKey(depthName))
			{
				Dictionary<string, long> result;
				(result = this.m_result)[depthName] = result[depthName] + elapse;
				return;
			}
			this.m_result[depthName] = elapse;
		}
		private void DumpWithPrefix(string prefix, int depth)
		{
			foreach (string current in this.m_result.Keys)
			{
				if (current.StartsWith(prefix) && !current.Substring(prefix.Length).Contains("##"))
				{
					string text = current.Substring(prefix.Length);
					Console.Write(new string(' ', depth));
					Console.Write(string.Format("- {0} : {1} ms\n", text, this.m_result[current]));
					this.DumpWithPrefix(prefix + text + "##", depth + 2);
				}
			}
		}
		public void Dump()
		{
			this.DumpWithPrefix("", 0);
		}
	}
}
