using Nintendo.MakeRom.Extensions;
using Nintendo.MakeRom.Ncch.RomFs;
using Nintendo.MakeRom.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Nintendo.MakeRom
{
	public class NcchBinaryCore : NcchBinary
	{
		private readonly NcchHeader m_RomHeader;
		private readonly PlainRegionBinary m_Plain;
		private readonly ByteArrayData m_PaddingBeforeNcchEnd = new ByteArrayData(0u);
		private readonly IRomFsBinary m_RomFs;
		private readonly ExeFsBinary m_ExeFs;
		private uint m_PageSize;
		private bool m_UseAes;
		private bool m_CipCompress;
		private bool m_MakeCci2;
		private byte m_PaddingChar;
		private ulong m_PartitionId;
		private ByteArrayData m_RomFsHeaderPadding = new ByteArrayData(0u);
		private ByteArrayData m_ExeFsFooterPadding = new ByteArrayData(0u);
		internal NcchBinaryCore(MakeCxiOptions options)
		{
			this.m_PartitionId = Util.MakePartitionId(options);
			this.m_PageSize = options.PageSize;
			this.m_UseAes = options.UseAes;
			this.m_MakeCci2 = options.MakeCci2;
			this.m_PaddingChar = options.Padding;
			this.m_CipCompress = options.CipCompress;
			Elf elf = null;
			if (!options.IsCfa && !options.IsCaa)
			{
				elf = new Elf(options.Source, options);
			}
			CodeSetInfo codeSetInfo = new CodeSetInfo(options);
			this.m_Plain = new PlainRegionBinary(elf, options);
			List<ExeFsBinary.Entry> list = new List<ExeFsBinary.Entry>();
			if (!options.IsCfa && !options.IsCaa)
			{
				Profiler.Entry("Code");
				CodeSegment codeSegment = null;
				CodeSegment codeSegment2 = null;
				CodeSegment codeSegment3 = null;
				this.CreateCodeSegments(out codeSegment, out codeSegment2, out codeSegment3, elf, options);
				int num = options.IsCip ? codeSegment.Data.Length : ((int)(codeSegment.Info.NumMaxPages * 4096u));
				int num2 = options.IsCip ? codeSegment2.Data.Length : ((int)(codeSegment2.Info.NumMaxPages * 4096u));
				int num3 = options.IsCip ? codeSegment3.Data.Length : ((int)(codeSegment3.Info.NumMaxPages * 4096u));
				byte[] array = new byte[num + num2 + num3];
				int num4 = 0;
				Array.Copy(codeSegment.Data, 0, array, num4, codeSegment.Data.Length);
				num4 += num;
				Array.Copy(codeSegment2.Data, 0, array, num4, codeSegment2.Data.Length);
				num4 += num2;
				Array.Copy(codeSegment3.Data, 0, array, num4, codeSegment3.Data.Length);
				base.CxiInfo.CodeInfo.OriginalSize = array.Length;
				if (options.Compress || options.CipCompress)
				{
					int num5 = Compress.CompressData(array);
					if (num5 < 0)
					{
						options.Compressed = false;
					}
					else
					{
						options.Compressed = true;
						Array.Resize<byte>(ref array, num5);
						base.CxiInfo.CodeInfo.CompressedSize = array.Length;
					}
				}
				list.Add(new ExeFsBinary.Entry
				{
					Name = options.TopExefsSectionName,
					Data = array
				});
				codeSetInfo = new CodeSetInfo(codeSegment.Info, codeSegment2.Info, codeSegment3.Info, this.GetBssSizeFromElf(elf, options), options);
			}
			if (options.IsCaa && options.SourcePath != null)
			{
				list.Add(new ExeFsBinary.Entry
				{
					Name = ".code",
					Data = File.ReadAllBytes(options.SourcePath)
				});
			}
			if (!string.IsNullOrEmpty(options.BannerPath))
			{
				list.Add(new ExeFsBinary.Entry
				{
					Name = "banner",
					Data = File.ReadAllBytes(options.BannerPath)
				});
			}
			if (!string.IsNullOrEmpty(options.IconPath))
			{
				list.Add(new ExeFsBinary.Entry
				{
					Name = "icon",
					Data = File.ReadAllBytes(options.IconPath)
				});
			}
			if (options.Logo != MakeCxiOptions.LogoName.NONE)
			{
				list.Add(new ExeFsBinary.Entry
				{
					Name = "logo",
					Data = this.GetLogoData(options)
				});
			}
			if (list.Count > 0)
			{
				this.m_ExeFs = new ExeFsBinary(list, options.Padding);
			}
			Profiler.Exit("Code");
			if (options.UseRomFs)
			{
				if (options.DotRomfsPath != null)
				{
					Profiler.Entry("Romfs");
					this.m_RomFs = new DotRomFsBinary(options, options.DotRomfsPath);
					Profiler.Exit("Romfs");
				}
				else
				{
					Profiler.Entry("Romfs");
					this.m_RomFs = new RomFsBinary(options, options.RomFsRoot);
					Profiler.Exit("Romfs");
				}
			}
			NcchExtendedHeader ncchExtendedHeader = null;
			NcchAccessControlExtended ncchAccessControlExtended = null;
			if (!options.IsCfa)
			{
				ARM11SystemLocalCapabilities sysLocalCap = new ARM11SystemLocalCapabilities(options);
				ARM11KernelCapabilities kernelCap = new ARM11KernelCapabilities(options);
				CoreInfo coreInfo = new CoreInfo(codeSetInfo, options);
				SystemInfo systemInfo = new SystemInfo(options);
				ARM9AccessControlInfo arm9Cont = new ARM9AccessControlInfo(options);
				AccessControlInfo accContInfo = new AccessControlInfo(sysLocalCap, kernelCap, arm9Cont);
				SystemControlInfo sysContInfo = new SystemControlInfo(coreInfo, systemInfo);
				ncchExtendedHeader = new NcchExtendedHeader(accContInfo, sysContInfo, options);
				ncchAccessControlExtended = new NcchAccessControlExtended(options, options.NcchCommonHeaderKeyParams);
			}
			this.m_RomHeader = new NcchHeader(ncchExtendedHeader, ncchAccessControlExtended, options);
			this.SetRegions(ncchExtendedHeader, ncchAccessControlExtended, this.m_Plain, this.m_ExeFs, this.m_RomFs, options);
			if (this.m_UseAes)
			{
				AesCtr aesCtr = new AesCtr(options.AesKey, this.m_PartitionId, 144115188075855872uL);
				AesCtr aesCtr2 = new AesCtr(options.AesKey, this.m_PartitionId, 72057594037927936uL);
				AesCtr aesCtr3 = new AesCtr(options.AesKey, this.m_PartitionId, 216172782113783808uL);
				this.CheckAesKey(options);
				if (ncchExtendedHeader != null && ncchAccessControlExtended != null)
				{
					ncchExtendedHeader.CryptoTransform = aesCtr2;
					ncchAccessControlExtended.CryptoTransform = new AesCtr(options.AesKey, this.m_PartitionId, 72057594037927936uL + ((ulong)ncchExtendedHeader.Size >> 4));
				}
				if (this.m_ExeFs != null)
				{
					this.m_ExeFs.CryptoTransform = aesCtr;
				}
				if (this.m_RomFs != null)
				{
					this.m_RomFs.CryptoTransform = aesCtr3;
				}
				base.CxiCoreInfo.ExeFsIvRaw = (byte[])aesCtr.GetCounter().Clone();
				base.CxiCoreInfo.RomFsIvRaw = (byte[])aesCtr3.GetCounter().Clone();
				base.CxiCoreInfo.HeaderIvRaw = (byte[])aesCtr2.GetCounter().Clone();
			}
			this.m_PaddingBeforeNcchEnd = Util.MakePaddingData(Util.CalculatePaddingSize(this.Size, options.AlignSize), options.Padding);
			this.m_RomHeader.ContentSize = Util.Int64ToMediaUnitSize(this.Size, options.MediaUnitSize);
			base.CxiInfo.Title = options.Name.ToString();
			base.CxiInfo.CoreInfo.ProgramIdRaw = TitleIdUtil.MakeProgramId(options);
			if (!options.OutputCoreInfo)
			{
				base.CxiInfo.CoreInfo = null;
			}
		}
		private byte[] GetLogoData(MakeCxiOptions options)
		{
			Dictionary<MakeCxiOptions.LogoName, byte[]> dictionary = new Dictionary<MakeCxiOptions.LogoName, byte[]>
			{

				{
					MakeCxiOptions.LogoName.NINTENDO,
					Resources.Nintendo_LZ
				},

				{
					MakeCxiOptions.LogoName.LICENSED,
					Resources.Nintendo_LicensedBy_LZ
				},

				{
					MakeCxiOptions.LogoName.DISTRIBUTED,
					Resources.Nintendo_DistributedBy_LZ
				},

				{
					MakeCxiOptions.LogoName.IQUE,
					Resources.iQue_with_ISBN_LZ
				},

				{
					MakeCxiOptions.LogoName.IQUEFORSYSTEM,
					Resources.iQue_without_ISBN_LZ
				}
			};
			return dictionary[options.Logo];
		}
		private void CheckAesKey(MakeCxiOptions options)
		{
			if (!TitleIdUtil.IsSystemCategory(options.CategoryFlags))
			{
				if (!options.AesKey.SequenceEqual(Resources.key))
				{
					Util.PrintWarning("Cxi is application title, but aes key is not Default key.\n");
					return;
				}
			}
			else
			{
				if (options.AesKey.SequenceEqual(Resources.key))
				{
					Util.PrintWarning("Cxi is system title, but aes key is Default key.\n");
				}
			}
		}
		private void SetRegions(NcchExtendedHeader exHeader, NcchAccessControlExtended acExtended, PlainRegionBinary plain, ExeFsBinary exeFs, IRomFsBinary romFs, MakeCxiOptions options)
		{
			long num = 0L;
			if (exHeader != null && acExtended != null)
			{
				num = exHeader.Size + acExtended.Size + 512L;
			}
			else
			{
				if (exHeader == null && acExtended == null)
				{
					num = 512L;
				}
			}
			if (plain != null && plain.Size != 0L)
			{
				uint mediaUnitSize = Util.Int64ToMediaUnitSize(this.m_Plain.Size, options.MediaUnitSize);
				this.m_RomHeader.PlainRegionOffset = Util.Int64ToMediaUnitSize(num, options.MediaUnitSize);
				this.m_RomHeader.PlainRegionSize = Util.Int64ToMediaUnitSize(this.m_Plain.Size, options.MediaUnitSize);
				num += Util.MediaUnitSizeToInt64(mediaUnitSize, options.MediaUnitSize);
			}
			if (exeFs != null)
			{
				this.m_RomHeader.ExeFsOffset = Util.Int64ToMediaUnitSize(num, options.MediaUnitSize);
				this.m_RomHeader.ExeFsSize = Util.Int64ToMediaUnitSize(this.m_ExeFs.Size, options.MediaUnitSize);
				this.m_RomHeader.ExeFsHashRegionSize = Util.Int64ToMediaUnitSize((long)((ulong)this.m_ExeFs.GetHashRegionSize()), options.MediaUnitSize);
				this.m_RomHeader.ExeFsSuperBlockHash = exeFs.GetSuperBlockHash();
				num += this.m_ExeFs.Size;
				base.CxiCoreInfo.ExeFsSize = this.m_ExeFs.Size;
				if (options.ExefsReserveSize != 0L)
				{
					if (Util.MediaUnitSizeToInt64(this.m_RomHeader.ExeFsSize, options.MediaUnitSize) > options.ExefsReserveSize)
					{
						throw new MakeromException("Memory size (-m) exceeded");
					}
					this.m_ExeFsFooterPadding = Util.MakePaddingData((uint)(options.ExefsReserveSize - Util.MediaUnitSizeToInt64(this.m_RomHeader.ExeFsSize, options.MediaUnitSize)), options.Padding);
					num += this.m_ExeFsFooterPadding.Size;
				}
			}
			if (romFs != null)
			{
				this.m_RomFsHeaderPadding = Util.MakePaddingData(Util.CalculatePaddingSize(num, 4096), options.Padding);
				num += this.m_RomFsHeaderPadding.Size;
				this.m_RomHeader.RomFsOffset = Util.Int64ToMediaUnitSize(num, options.MediaUnitSize);
				this.m_RomHeader.RomFsSize = Util.Int64ToMediaUnitSize(this.m_RomFs.Size, options.MediaUnitSize);
				this.m_RomHeader.RomFsHashRegionSize = Util.Int64ToMediaUnitSize((long)((ulong)this.m_RomFs.GetHashRegionSize()), options.MediaUnitSize);
				this.m_RomHeader.RomFsSuperBlockHash = romFs.GetSuperBlockHash();
				num += this.m_RomFs.Size;
				base.CxiInfo.RomFsInfo = this.m_RomFs.RomFsInfo;
				base.CxiCoreInfo.RomFsSize = this.m_RomFs.Size;
			}
		}
		public override ulong GetPartitionId()
		{
			return this.m_RomHeader.PartitionId;
		}
		public override ulong GetProgramId()
		{
			return this.m_RomHeader.ProgramId;
		}
		public override byte GetCryptoType()
		{
			return this.m_RomHeader.CryptoType;
		}
		private void CreateCodeSegments(out CodeSegment text, out CodeSegment ro, out CodeSegment data, Elf elf, MakeCxiOptions options)
		{
			text = this.CreateCodeSegmentFromElf(elf, options.ExeFs["Text"].ToArray());
			ro = this.CreateCodeSegmentFromElf(elf, options.ExeFs["ReadOnly"].ToArray());
			data = this.CreateCodeSegmentFromElf(elf, options.ExeFs["ReadWrite"].ToArray());
			if (text.Info.CodeSize == 0u)
			{
				text = new CodeSegment(0u, 0u, 0u, new byte[0]);
			}
			if (ro.Info.CodeSize == 0u)
			{
				ro = new CodeSegment(text.Info.Address + (options.AllowsUnalignedSection ? (uint)text.Info.CodeSize : (text.Info.NumMaxPages * options.PageSize)), 0u, 0u, new byte[0]);
			}
			if (data.Info.CodeSize == 0u)
			{
				data = new CodeSegment(ro.Info.Address + (options.AllowsUnalignedSection ? (uint)ro.Info.CodeSize : (ro.Info.NumMaxPages * options.PageSize)), 0u, 0u, new byte[0]);
			}
		}
		private CodeSegment CreateCodeSegmentFromElf(Elf elf, params string[] names)
		{
			List<ElfSegment> continuousSegments = elf.GetContinuousSegments(names);
			if (continuousSegments == null)
			{
				return new CodeSegment(0u, 0u, 0u, new byte[0]);
			}
			uint num = 0u;
			uint num2 = 0u;
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			foreach (ElfSegment current in continuousSegments)
			{
				if (current == continuousSegments[0])
				{
					num = current.VAddr;
				}
				else
				{
					uint num3 = current.VAddr - (num + num2);
					binaryWriter.Write(new byte[num3]);
					num2 += num3;
				}
				num2 += current.Header.MemorySize;
				uint num4 = 0u;
				ElfSectionHeaderInfo[] sections = current.Sections;
				for (int i = 0; i < sections.Length; i++)
				{
					ElfSectionHeaderInfo elfSectionHeaderInfo = sections[i];
					if (elfSectionHeaderInfo.Header.IsBss())
					{
						if (elfSectionHeaderInfo != current.Sections.Last<ElfSectionHeaderInfo>())
						{
							binaryWriter.Write(new byte[elfSectionHeaderInfo.Header.Size]);
							num4 += elfSectionHeaderInfo.Header.Size;
						}
						else
						{
							num2 -= elfSectionHeaderInfo.Header.Size;
						}
					}
					else
					{
						binaryWriter.Write(elf.GetData(elfSectionHeaderInfo.Header.Offset, elfSectionHeaderInfo.Header.Size));
						num4 += elfSectionHeaderInfo.Header.Size;
					}
				}
			}
			binaryWriter.Flush();
			return new CodeSegment(num, this.SizeToPage(num2), num2, memoryStream.ToArray());
		}
		private CodeSegment CreateCodeSegmentFromSectionHeaders(List<ElfSectionHeader> headers, Elf elf)
		{
			if (headers == null)
			{
				return null;
			}
			uint address = headers.First<ElfSectionHeader>().Address;
			uint arg_1C_0 = headers.First<ElfSectionHeader>().Offset;
			uint num = 0u;
			foreach (ElfSectionHeader current in headers)
			{
				num += current.Size;
			}
			return new CodeSegment(address, this.SizeToPage(num), num, elf.GetData(headers));
		}
		private uint SizeToPage(uint size)
		{
			return (size + this.m_PageSize - 1u) / this.m_PageSize;
		}
		private uint GetBssSizeFromElf(Elf elf, MakeCxiOptions options)
		{
			string[] array = null;
			array = ((options.ExeFs["Text"].Count > 0) ? options.ExeFs["Text"].ToArray() : array);
			array = ((options.ExeFs["ReadOnly"].Count > 0) ? options.ExeFs["ReadOnly"].ToArray() : array);
			array = ((options.ExeFs["ReadWrite"].Count > 0) ? options.ExeFs["ReadWrite"].ToArray() : array);
			if (array == null)
			{
				return 0u;
			}
			List<ElfSegment> continuousSegments = elf.GetContinuousSegments(array);
			if (continuousSegments == null)
			{
				return 0u;
			}
			ElfSectionHeaderInfo elfSectionHeaderInfo = continuousSegments.Last<ElfSegment>().Sections.Last<ElfSectionHeaderInfo>();
			if (elfSectionHeaderInfo != null & elfSectionHeaderInfo.Header.IsBss())
			{
				return elfSectionHeaderInfo.Header.Size;
			}
			return 0u;
		}
		protected override void Update()
		{
			base.SetBinaries(new IWritableBinary[]
			{
				this.m_RomHeader,
				this.m_Plain
			});
			if (this.m_ExeFs != null)
			{
				base.AddBinaries(new IWritableBinary[]
				{
					this.m_ExeFs,
					this.m_ExeFsFooterPadding
				});
			}
			if (this.m_RomFs != null)
			{
				base.AddBinaries(new IWritableBinary[]
				{
					this.m_RomFsHeaderPadding
				});
				base.AddBinaries(new IWritableBinary[]
				{
					this.m_RomFs
				});
			}
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_PaddingBeforeNcchEnd
			});
		}
		private void WriteBinaryForCci2(BinaryWriter writer)
		{
			foreach (IWritableBinary current in new List<IWritableBinary>
			{
				this.m_RomHeader,
				this.m_Plain,
				this.m_ExeFs
			})
			{
				current.WriteBinary(writer);
			}
		}
		public override void WriteBinary(BinaryWriter writer)
		{
			if (this.m_MakeCci2)
			{
				this.WriteBinaryForCci2(writer);
				return;
			}
			base.WriteBinary(writer);
		}
		public override byte[] GetCommonHeader()
		{
			return this.m_RomHeader.CommonHeader.GetByteArray();
		}
		public override void SetProgramId(ulong programId)
		{
			if (this.m_RomHeader.CommonHeader.Struct.ProgramId != programId)
			{
				throw new NotSupportedException("Partition 0's ID cannnot be modified");
			}
		}
		public override void SetPartitionId(ulong partitionId)
		{
			if (this.m_RomHeader.CommonHeader.Struct.PartitionId != partitionId)
			{
				throw new NotSupportedException("Partition 0's ID cannot be modified");
			}
		}
	}
}
