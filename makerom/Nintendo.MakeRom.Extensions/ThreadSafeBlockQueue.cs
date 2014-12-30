using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
namespace Nintendo.MakeRom.Extensions
{
	internal class ThreadSafeBlockQueue<T> : Queue<T>
	{
		private int m_timeout = -1;
		private AutoResetEvent m_event = new AutoResetEvent(false);
		private object m_lock = new object();
		public int Timeout
		{
			get
			{
				return this.m_timeout;
			}
			set
			{
				this.m_timeout = value;
			}
		}
		[Conditional("DEBUG")]
		public static void RunTest()
		{
			ThreadSafeBlockQueue<int> queue = new ThreadSafeBlockQueue<int>();
			queue.Timeout = 1000;
			int total = 1048576;
			Thread thread = new Thread((ThreadStart)delegate
			{
				for (int i = 0; i < total; i++)
				{
					queue.EnqueueBlock(i);
				}
			});
			Thread thread2 = new Thread((ThreadStart)delegate
			{
				for (int i = 0; i < total; i++)
				{
				}
				try
				{
					queue.DequeueBlock();
				}
				catch (TimeoutException)
				{
				}
			});
			thread.Start();
			thread2.Start();
			thread.Join();
			thread2.Join();
		}
		public void EnqueueBlock(T q)
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				base.Enqueue(q);
				this.m_event.Set();
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public T DequeueBlock()
		{
			if (base.Count == 0)
			{
				object @lock;
				Monitor.Enter(@lock = this.m_lock);
				try
				{
					if (base.Count == 0)
					{
						this.m_event.Reset();
					}
				}
				finally
				{
					Monitor.Exit(@lock);
				}
				if (!this.m_event.WaitOne(this.m_timeout))
				{
					throw new TimeoutException("Timeout occured in Dequeueing");
				}
			}
			object lock2;
			Monitor.Enter(lock2 = this.m_lock);
			T result;
			try
			{
				result = base.Dequeue();
			}
			finally
			{
				Monitor.Exit(lock2);
			}
			return result;
		}
	}
}
