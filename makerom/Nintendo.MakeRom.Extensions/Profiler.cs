using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Nintendo.MakeRom.Extensions
{
	public class Profiler
	{
		private static Stack<ProfilingDepth> s_depth;
		private static ProfilingResult s_result;
		public static bool IsEnableProfile
		{
			get;
			set;
		}
		static Profiler()
		{
			Profiler.s_depth = new Stack<ProfilingDepth>();
			Profiler.s_result = new ProfilingResult();
			Profiler.IsEnableProfile = false;
		}
		public static void Entry(string name)
		{
			if (Profiler.IsEnableProfile)
			{
				ProfilingDepth profilingDepth = new ProfilingDepth();
				profilingDepth.Name = name;
				profilingDepth.Start();
				Profiler.s_depth.Push(profilingDepth);
			}
		}
		public static void Exit(string name)
		{
			if (Profiler.IsEnableProfile)
			{
				try
				{
					ProfilingDepth profilingDepth = Profiler.s_depth.Pop();
					if (profilingDepth.Name != name)
					{
						Profiler.IsEnableProfile = false;
					}
					else
					{
						profilingDepth.Stop();
						Profiler.s_result.Append(Profiler.s_result.GetPathName(Profiler.s_depth, profilingDepth.Name), profilingDepth.Millisec);
					}
				}
				catch (InvalidOperationException)
				{
					Profiler.IsEnableProfile = false;
				}
			}
		}
		public static void Dump()
		{
			Profiler.s_result.Dump();
		}
		[Conditional("DEBUG")]
		public static void PrivateEntry(string name)
		{
			Profiler.Entry(name);
		}
		[Conditional("DEBUG")]
		public static void PrivateExit(string name)
		{
			Profiler.Exit(name);
		}
	}
}
