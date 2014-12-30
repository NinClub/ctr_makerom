using Nintendo.MakeRom.Extensions;
using Nintendo.MakeRom.MakeFS;
using Nintendo.MakeRom.Ncch.RomFs;
using System;
using System.IO;
namespace Nintendo.MakeRom
{
	internal class RomFsBinary : WritableCryptoBinaryRegistory, IRomFsBinary, IWritableBinary
	{
		private const int PADDING_ALIGN = 16;
		private RomFsDataBlock m_DataBlock;
		private ByteArrayData m_FootPadding = new byte[0];
		public RomFsInfo RomFsInfo
		{
			get;
			private set;
		}
		public RomFsBinary(MakeCxiOptions options, string rootPath)
		{
			this.RomFsInfo = new RomFsInfo();
			this.RomFsInfo.Root = Path.GetFullPath(rootPath);
			FileSearcher searcher = new FileSearcher(options);
			FileNameTable fileNameTable = new FileNameTable(rootPath, searcher);
			Entry.Input[] array = new Entry.Input[fileNameTable.NumFiles];
			Layouter.Input[] array2 = new Layouter.Input[fileNameTable.NumFiles];
			Creator.Input[] array3 = new Creator.Input[fileNameTable.NumFiles];
			int num = 0;
			foreach (FileSystemInfo current in fileNameTable.NameEntryTable)
			{
				FileInfo fileInfo = (FileInfo)current;
				array2[num] = new Layouter.Input();
				array2[num].m_PCName = fileInfo.FullName;
				array2[num].m_SizeAlign = 16u;
				num++;
				this.RomFsInfo.AddFile(fileInfo.FullName);
			}
			CrrUpdater crrUpdater = new CrrUpdater(this.RomFsInfo.Root);
			crrUpdater.Update(options.TitleUniqueId);
			Layouter.Output[] array4 = Layouter.Create(array2);
			num = 0;
			Layouter.Output[] array5 = array4;
			for (int i = 0; i < array5.Length; i++)
			{
				Layouter.Output output = array5[i];
				array[num] = new Entry.Input();
				array[num].m_Offset = output.m_Offset;
				array[num].m_PathName = output.m_PCName.Replace(this.RomFsInfo.Root.TrimEnd(new char[]
				{
					'\\',
					'/'
				}), "").Replace("\\", "/");
				array[num].m_Size = output.m_Size;
				array3[num] = new Creator.Input();
				array3[num].m_PCName = output.m_PCName;
				array3[num].m_Offset = output.m_Offset;
				array3[num].m_Size = output.m_Size;
				num++;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Entry.Create(memoryStream, array, 16u);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				this.m_DataBlock = new RomFsDataBlock(array3, memoryStream);
			}
			this.m_FootPadding = Util.MakePaddingData(Util.CalculatePaddingSize(this.Size, 512), 255);
		}
		public byte[] GetSuperBlockHash()
		{
			return this.m_DataBlock.GetSuperBlockHash();
		}
		public uint GetHashRegionSize()
		{
			return this.m_DataBlock.GetHashRegionSize();
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_DataBlock,
				this.m_FootPadding
			});
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			Profiler.Entry("Romfs");
			base.WriteBinary(writer);
			Profiler.Exit("Romfs");
		}
	}
}
