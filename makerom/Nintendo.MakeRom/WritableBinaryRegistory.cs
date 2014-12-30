using System;
using System.Collections.Generic;
using System.IO;
namespace Nintendo.MakeRom
{
	public abstract class WritableBinaryRegistory : IWritableBinary
	{
		private List<IWritableBinary> m_WritableBinaryList = new List<IWritableBinary>();
		public virtual long Size
		{
			get
			{
				ulong num = 0uL;
				this.Update();
				foreach (IWritableBinary current in this.m_WritableBinaryList)
				{
					num += (ulong)current.Size;
				}
				Util.CheckSize(num, 9223372036854775807uL);
				return (long)num;
			}
		}
		public void ClearBinaries()
		{
			this.m_WritableBinaryList.Clear();
		}
		public void SetBinaries(params IWritableBinary[] binaries)
		{
			this.ClearBinaries();
			this.AddBinaries(binaries);
		}
		public void SetBinariesWithPadding(uint alignment, params IWritableBinary[] binaries)
		{
			this.ClearBinaries();
			for (int i = 0; i < binaries.Length; i++)
			{
				IWritableBinary writableBinary = binaries[i];
				this.AddBinary(writableBinary);
				if (writableBinary.Size % (long)((ulong)alignment) != 0L)
				{
					this.AddBinary(new ReservedBlock((uint)((ulong)alignment - (ulong)(writableBinary.Size % (long)((ulong)alignment)))));
				}
			}
		}
		public void SetEnumerableBinary<T>(IEnumerable<T> binaries) where T : IWritableBinary
		{
			this.ClearBinaries();
			foreach (T current in binaries)
			{
				this.AddBinary(current);
			}
		}
		public void AddBinary(IWritableBinary binary)
		{
			this.m_WritableBinaryList.Add(binary);
		}
		public void AddBinaries(params IWritableBinary[] binaries)
		{
			for (int i = 0; i < binaries.Length; i++)
			{
				IWritableBinary binary = binaries[i];
				this.AddBinary(binary);
			}
		}
		public void AddUInt32Binary(uint i)
		{
			this.AddBinary(new UInt32Data(i));
		}
		public void AddByteArrayBinary(byte[] bytes)
		{
			this.AddBinary(new ByteArrayData(bytes));
		}
		public virtual void WriteBinary(BinaryWriter writer)
		{
			this.Update();
			foreach (IWritableBinary current in this.m_WritableBinaryList)
			{
				current.WriteBinary(writer);
			}
		}
		protected abstract void Update();
	}
}
