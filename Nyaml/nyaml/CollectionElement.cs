using System;
namespace nyaml
{
	public abstract class CollectionElement
	{
		public virtual CollectionElement this[int index]
		{
			get
			{
				if (base.GetType() == typeof(Sequence))
				{
					return ((Sequence)this)[index];
				}
				throw new NotSupportedException();
			}
		}
		public virtual CollectionElement this[string key]
		{
			get
			{
				if (base.GetType() == typeof(Mapping))
				{
					return ((Mapping)this)[key];
				}
				throw new NotSupportedException();
			}
		}
		public abstract bool IsNullScalar
		{
			get;
		}
		public abstract bool IsCollection
		{
			get;
		}
		public abstract bool IsScalar
		{
			get;
		}
		public abstract int Count
		{
			get;
		}
		public abstract void Dump(int indent);
		public CollectionElement GetCollectionElement(string name)
		{
			return this.GetCollectionElementFromArray(name.Split(new char[]
			{
				'/'
			}));
		}
		public CollectionElement GetCollectionElementFromArray(params string[] names)
		{
			CollectionElement collectionElement = this;
			int i = 0;
			while (i < names.Length)
			{
				string key = names[i];
				CollectionElement result;
				if (collectionElement.GetType() != typeof(Mapping))
				{
					result = null;
				}
				else
				{
					collectionElement = collectionElement[key];
					if (collectionElement != null)
					{
						i++;
						continue;
					}
					result = null;
				}
				return result;
			}
			return collectionElement;
		}
		public abstract int GetInteger();
		public abstract string GetString();
		public abstract bool GetBoolean();
		public abstract long GetLong();
		public abstract bool Remove(CollectionElement element);
	}
}
