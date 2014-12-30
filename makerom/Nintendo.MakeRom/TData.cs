using System;
using System.IO;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	internal abstract class TData<T> : IWritableBinary where T : struct
	{
		public T Data
		{
			get;
			set;
		}
		public long Size
		{
			get
			{
				return (long)Marshal.SizeOf(this.Data);
			}
		}
		public TData()
		{
		}
		public TData(T data)
		{
			this.Data = data;
		}
		public abstract void WriteBinary(BinaryWriter writer);
	}
}
