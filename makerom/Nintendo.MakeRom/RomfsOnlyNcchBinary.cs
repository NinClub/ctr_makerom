using Nintendo.MakeRom.Ncch.FastBuildRomfs;
using System;
using System.IO;
namespace Nintendo.MakeRom
{
	public class RomfsOnlyNcchBinary : NcchBinary
	{
		private ulong m_programID;
		private ulong m_partitionID;
		private MakeCxiOptions m_options;
		private FastBuildRomFsInfo m_romfsInfo;
		public FastBuildRomFsInfo RomfsInfo
		{
			get
			{
				return this.m_romfsInfo;
			}
		}
		internal RomfsOnlyNcchBinary(CxiOption option)
		{
			this.m_options = new MakeCxiOptions(option);
			this.m_programID = TitleIdUtil.MakeProgramId(this.m_options);
			this.m_partitionID = Util.MakePartitionId(this.m_options);
		}
		public override ulong GetPartitionId()
		{
			return this.m_partitionID;
		}
		public override ulong GetProgramId()
		{
			return this.m_programID;
		}
		public override void SetPartitionId(ulong partitionId)
		{
			if (this.m_partitionID != partitionId)
			{
				throw new NotImplementedException();
			}
		}
		public override void SetProgramId(ulong programId)
		{
			if (this.m_programID != programId)
			{
				throw new NotImplementedException();
			}
		}
		public override byte GetCryptoType()
		{
			throw new NotImplementedException();
		}
		public override byte[] GetCommonHeader()
		{
			throw new NotImplementedException();
		}
		protected override void Update()
		{
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			FastBuildRomfsHeader fastBuildRomfsHeader = new FastBuildRomfsHeader();
			Stream baseStream = writer.BaseStream;
			writer.Flush();
			long position = baseStream.Position;
			fastBuildRomfsHeader.Dump(baseStream);
			if (this.m_options.RomFsRoot != null)
			{
				FastBuildRomFsBinary fastBuildRomFsBinary = new FastBuildRomFsBinary(this.m_options);
				if (this.m_options.RomfsLayoutPath != null)
				{
					fastBuildRomFsBinary.LoadRomfsInfo(this.m_options.RomfsLayoutPath);
				}
				CrrUpdater crrUpdater = new CrrUpdater(this.m_options.RomFsRoot);
				crrUpdater.Update(this.m_options.TitleUniqueId);
				fastBuildRomFsBinary.Write(baseStream);
				long position2 = baseStream.Position;
				fastBuildRomfsHeader.ProtectionArea = fastBuildRomFsBinary.ProtectionArea;
				fastBuildRomfsHeader.ProtectionHash = fastBuildRomFsBinary.ProtectionAreaHash;
				fastBuildRomfsHeader.RomfsSize = fastBuildRomFsBinary.RomfsSize;
				baseStream.Seek(position, SeekOrigin.Begin);
				fastBuildRomfsHeader.Dump(baseStream);
				baseStream.Seek(position2, SeekOrigin.Begin);
				this.m_romfsInfo = fastBuildRomFsBinary.RomFsInfo;
				return;
			}
			long position3 = baseStream.Position;
			fastBuildRomfsHeader.ProtectionArea = 0u;
			fastBuildRomfsHeader.ProtectionHash = new byte[32];
			fastBuildRomfsHeader.RomfsSize = 0L;
			baseStream.Seek(position, SeekOrigin.Begin);
			fastBuildRomfsHeader.Dump(baseStream);
			baseStream.Seek(position3, SeekOrigin.Begin);
			this.m_romfsInfo = null;
		}
	}
}
