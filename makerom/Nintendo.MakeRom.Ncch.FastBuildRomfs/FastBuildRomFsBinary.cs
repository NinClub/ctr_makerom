using Nintendo.MakeRom.MakeFS;
using Nintendo.MakeRom.Ncch.RomFs;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace Nintendo.MakeRom.Ncch.FastBuildRomfs
{
	internal class FastBuildRomFsBinary
	{
		private const int PADDING_FILE_ALIGN = 16;
		private const int FILECONTENT_START_ALIGN = 16;
		private FastBuildRomFsDataBlock m_DataBlock;
		private ByteArrayData m_FootPadding = new byte[0];
		private MakeCxiOptions m_options;
		internal long RomfsSize
		{
			get;
			private set;
		}
		internal ulong PartitionID
		{
			get;
			private set;
		}
		internal uint ProtectionArea
		{
			get;
			private set;
		}
		internal byte[] ProtectionAreaHash
		{
			get;
			private set;
		}
		internal FastBuildRomFsInfo RomFsInfo
		{
			get;
			private set;
		}
		internal FastBuildRomFsBinary(MakeCxiOptions options)
		{
			this.m_options = options;
		}
		internal void LoadRomfsInfo(string filename)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(FastBuildRomFsInfo));
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				XmlTextReader xmlTextReader = new XmlTextReader(fileStream);
				xmlTextReader.ReadToFollowing("RomFsInfo");
				this.RomFsInfo = (xmlSerializer.Deserialize(xmlTextReader.ReadSubtree()) as FastBuildRomFsInfo);
			}
		}
		private void SetupRomFsInfoFromRsf()
		{
			FileSearcher searcher = new FileSearcher(this.m_options);
			FileNameTable fileNameTable = new FileNameTable(this.m_options.RomFsRoot, searcher);
			this.RomFsInfo = new FastBuildRomFsInfo();
			this.RomFsInfo.Root = Path.GetFullPath(this.m_options.RomFsRoot);
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
			}
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
				this.RomFsInfo.AddFile(array[num], array3[num]);
				num++;
			}
		}
		private void SetupRomFsInfoFromLayoutFile()
		{
			this.RomFsInfo.MakeEntriesAndCreators();
			ulong num = 0uL;
			Entry.Input[] entries = this.RomFsInfo.GetEntries();
			for (int i = 0; i < entries.Length; i++)
			{
				Entry.Input input = entries[i];
				if (num > input.m_Offset)
				{
					throw new FormatException("File Layout is Invalid, Overlap occurs");
				}
				num += input.m_Size;
			}
		}
		internal void Write(Stream writer)
		{
			if (this.RomFsInfo == null)
			{
				this.SetupRomFsInfoFromRsf();
			}
			else
			{
				this.SetupRomFsInfoFromLayoutFile();
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Entry.Create(memoryStream, this.RomFsInfo.GetEntries(), 16u);
				AesCtr ctr = new AesCtr(this.m_options.AesKey, Util.MakePartitionId(this.m_options), 216172782113783808uL);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				this.m_DataBlock = new FastBuildRomFsDataBlock(this.RomFsInfo.GetCreators(), memoryStream, writer, ctr);
			}
			this.m_FootPadding = Util.MakePaddingData(Util.CalculatePaddingSize(this.m_DataBlock.Size, 512), 255);
			writer.Write(this.m_FootPadding.Data, 0, (int)this.m_FootPadding.Size);
			this.PartitionID = Util.MakePartitionId(this.m_options);
			this.ProtectionArea = this.m_DataBlock.GetHashRegionSize();
			this.ProtectionAreaHash = this.m_DataBlock.GetActualHash();
			this.RomfsSize = this.m_DataBlock.Size;
		}
	}
}
