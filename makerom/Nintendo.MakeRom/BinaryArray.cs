using System;
namespace Nintendo.MakeRom
{
	public class BinaryArray<T> : WritableBinaryRegistory where T : IWritableBinary, new()
	{
		public T[] Array
		{
			get;
			private set;
		}
		public BinaryArray(int size)
		{
			this.InitArray(size);
		}
		private void InitArray(int size)
		{
			this.Array = new T[size];
			for (int i = 0; i < this.Array.Length; i++)
			{
				this.Array[i] = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			}
		}
		protected override void Update()
		{
			base.SetEnumerableBinary<T>(this.Array);
		}
		protected void ClearArray()
		{
			this.InitArray(this.Array.Length);
		}
	}
}
