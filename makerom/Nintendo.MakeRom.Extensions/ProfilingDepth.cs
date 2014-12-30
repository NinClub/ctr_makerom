using System;
using System.Diagnostics;
namespace Nintendo.MakeRom.Extensions
{
	internal class ProfilingDepth
	{
		private Stopwatch m_watch;
		public string Name
		{
			get;
			set;
		}
		public long Millisec
		{
			get;
			set;
		}
		public void Start()
		{
			this.m_watch = new Stopwatch();
			this.m_watch.Start();
			this.Millisec = 0L;
		}
		public void Stop()
		{
			this.m_watch.Stop();
			this.Millisec = this.m_watch.ElapsedMilliseconds;
		}
	}
}
