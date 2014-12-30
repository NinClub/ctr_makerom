using System;
namespace nyaml
{
	public abstract class Scalar : CollectionElement
	{
		public override bool IsScalar
		{
			get
			{
				return true;
			}
		}
		public override bool IsCollection
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
				throw new NotImplementedException();
			}
		}
		public override void Dump(int indent)
		{
			Console.WriteLine(new string(' ', indent) + this.ToString());
		}
		public override int GetInteger()
		{
			throw new WrongCastException(this, typeof(int));
		}
		public override string GetString()
		{
			throw new WrongCastException(this, typeof(string));
		}
		public override bool GetBoolean()
		{
			throw new WrongCastException(this, typeof(bool));
		}
		public override long GetLong()
		{
			throw new WrongCastException(this, typeof(long));
		}
		public override bool Remove(CollectionElement element)
		{
			throw new NotImplementedException();
		}
	}
}
