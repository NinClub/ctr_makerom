using Nintendo.MakeRom.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class ExeFsBinary : WritableCryptoBinaryRegistory
	{
		public class Entry
		{
			public string Name
			{
				get;
				set;
			}
			public byte[] Data
			{
				get;
				set;
			}
		}
		private const int PADDING_ALIGN = 512;
		private ExeFsSuperBlock m_SuperBlock = new ExeFsSuperBlock();
		private ExeFsDataBlock m_DataBlock;
		private ByteArrayData m_FootPadding = new byte[0];
		private int RoundUp(int num, int baseNum)
		{
			return num + baseNum - 1 & ~(baseNum - 1);
		}
		public ExeFsBinary(IEnumerable<ExeFsBinary.Entry> entries, byte paddingChar)
		{
			this.m_DataBlock = new ExeFsDataBlock(512, paddingChar);
			uint num = 0u;
			int num2 = 0;
			foreach (ExeFsBinary.Entry current in entries)
			{
				this.AddDataEntry(current.Name, num, current.Data, num2);
				num += (uint)this.RoundUp(current.Data.Length, 512);
				num2++;
			}
			this.UpdateFootPadding(paddingChar);
		}
		public void AddDataEntry(string name, uint offset, byte[] data, int index)
		{
			this.m_SuperBlock.AddData(name, offset, data, index);
			this.m_DataBlock.AddDataBlock(data);
		}
		private void UpdateFootPadding(byte paddingChar)
		{
			uint paddingSize = Util.CalculatePaddingSize(this.Size, 512);
			this.m_FootPadding = Util.MakePaddingData(paddingSize, paddingChar);
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_SuperBlock,
				this.m_DataBlock,
				this.m_FootPadding
			});
		}
		public byte[] GetSuperBlockHash()
		{
			return this.m_SuperBlock.GetSha256Hash();
		}
		public uint GetHashRegionSize()
		{
			return (uint)this.m_SuperBlock.Size;
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			Profiler.Entry("Code");
			base.WriteBinary(writer);
			Profiler.Exit("Code");
		}
	}
}
