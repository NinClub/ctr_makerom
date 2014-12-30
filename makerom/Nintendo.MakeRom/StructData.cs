using System;
using System.Runtime.InteropServices;
namespace Nintendo.MakeRom
{
	public class StructData<T> : WritableBinaryRegistory where T : struct
	{
		public T Struct;
		private readonly ByteArrayData m_Data;
		protected StructData()
		{
			this.m_Data = new ByteArrayData((uint)Marshal.SizeOf(this.Struct));
		}
		protected override void Update()
		{
			GCHandle gCHandle = GCHandle.Alloc(this.m_Data.Data, GCHandleType.Pinned);
			Marshal.StructureToPtr(this.Struct, gCHandle.AddrOfPinnedObject(), false);
			gCHandle.Free();
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_Data
			});
		}
		public byte[] GetByteArray()
		{
			return this.m_Data.Data;
		}
	}
}
