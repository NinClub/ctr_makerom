using System;
using System.Collections;
using System.Collections.Generic;
namespace nyaml
{
	public class Sequence : Collection, IEnumerable<CollectionElement>, IEnumerable
	{
		public override CollectionElement this[int index]
		{
			get
			{
				CollectionElement result;
				try
				{
					result = this.Elements[index];
				}
				catch (ArgumentOutOfRangeException)
				{
					result = null;
				}
				return result;
			}
		}
		public List<CollectionElement> Elements
		{
			get;
			private set;
		}
		public override bool IsNullScalar
		{
			get
			{
				return false;
			}
		}
		public override int Count
		{
			get
			{
				return this.Elements.Count;
			}
		}
		public Sequence()
		{
			this.Elements = new List<CollectionElement>();
		}
		public Sequence(CollectionElement element) : this()
		{
			this.Elements.Add(element);
		}
		public IEnumerator<CollectionElement> GetEnumerator()
		{
			return this.Elements.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Elements.GetEnumerator();
		}
		public override void Dump(int indent)
		{
			foreach (CollectionElement current in this.Elements)
			{
				Console.WriteLine(new string(' ', indent) + "-");
				current.Dump(indent + 1);
			}
		}
		public override int GetInteger()
		{
			throw new NotImplementedException();
		}
		public override bool GetBoolean()
		{
			throw new NotImplementedException();
		}
		public override string GetString()
		{
			throw new NotImplementedException();
		}
		public override long GetLong()
		{
			throw new NotImplementedException();
		}
		public override bool Remove(CollectionElement element)
		{
			return this.Elements.Remove(element);
		}
	}
}
