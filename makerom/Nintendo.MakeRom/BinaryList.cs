using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	public class BinaryList<T> : WritableBinaryRegistory where T : IWritableBinary
	{
		public List<T> List
		{
			get;
			private set;
		}
		public int Count
		{
			get
			{
				return this.List.Count;
			}
		}
		public T this[int i]
		{
			get
			{
				return this.List[i];
			}
			set
			{
				this.List[i] = value;
			}
		}
		public BinaryList()
		{
			this.List = new List<T>();
		}
		public void Add(T t)
		{
			this.List.Add(t);
		}
		protected override void Update()
		{
			base.SetEnumerableBinary<T>(this.List);
		}
	}
}
