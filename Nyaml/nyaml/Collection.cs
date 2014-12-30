using System;
namespace nyaml
{
	public abstract class Collection : CollectionElement
	{
		public override bool IsCollection
		{
			get
			{
				return true;
			}
		}
		public override bool IsScalar
		{
			get
			{
				return false;
			}
		}
	}
}
