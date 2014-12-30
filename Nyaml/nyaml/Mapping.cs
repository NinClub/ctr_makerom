using System;
using System.Collections;
using System.Collections.Generic;
namespace nyaml
{
	public class Mapping : Collection, IEnumerable<KeyValuePair<string, CollectionElement>>, IEnumerable
	{
		public override CollectionElement this[string key]
		{
			get
			{
				CollectionElement result;
				try
				{
					result = this.Elements[key];
				}
				catch (KeyNotFoundException)
				{
					result = null;
				}
				return result;
			}
		}
		public Dictionary<string, CollectionElement> Elements
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
		public Mapping()
		{
			this.Elements = new Dictionary<string, CollectionElement>();
		}
		public Mapping(string key, CollectionElement value) : this()
		{
			this.Elements.Add(key, value);
		}
		public IEnumerator<KeyValuePair<string, CollectionElement>> GetEnumerator()
		{
			return this.Elements.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Elements.GetEnumerator();
		}
		public override void Dump(int indent)
		{
			foreach (KeyValuePair<string, CollectionElement> current in this.Elements)
			{
				Console.WriteLine(new string(' ', indent) + current.Key + ":");
				current.Value.Dump(indent + 1);
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
			string text = null;
			foreach (KeyValuePair<string, CollectionElement> current in this.Elements)
			{
				if (current.Value == element)
				{
					text = current.Key;
					break;
				}
			}
			if (text != null)
			{
				this.Elements.Remove(text);
				return true;
			}
			return false;
		}
	}
}
