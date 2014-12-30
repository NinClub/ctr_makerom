using Nintendo.MakeRom;
using Nintendo.MakeRom.Extensions;
using nyaml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
namespace makerom
{
	internal class MakeCciOptions
	{
		public enum MediaTypeName
		{
			Card,
			NAND
		}
		public enum OutputFormatName
		{
			CXI,
			CCI,
			CFA,
			ROMFS,
			CCL,
			EBIN,
			RBIN,
			CAA
		}
		public CxiOption CxiOption
		{
			get;
			private set;
		}
		public FileStream Source
		{
			get;
			private set;
		}
		public string ObjectName
		{
			get;
			private set;
		}
		public string InfoName
		{
			get;
			private set;
		}
		public bool IsForCard
		{
			get;
			private set;
		}
		public bool IsCfa
		{
			get;
			private set;
		}
		public bool IsCip
		{
			get;
			private set;
		}
		public uint PageSize
		{
			get;
			private set;
		}
		public MakeCciOptions.OutputFormatName OutputFormat
		{
			get;
			private set;
		}
		public int AlignSize
		{
			get;
			private set;
		}
		public string MediaSize
		{
			get;
			private set;
		}
		public bool MediaFootPadding
		{
			get;
			private set;
		}
		public byte Padding
		{
			get;
			private set;
		}
		public byte MediaUnitSize
		{
			get;
			private set;
		}
		public NcsdHeader.MediaType NcsdMediaType
		{
			get;
			private set;
		}
		public NcsdHeader.MediaPlatform[] NcsdMediaPlatforms
		{
			get;
			private set;
		}
		public MakeCciOptions.MediaTypeName MediaType
		{
			get;
			private set;
		}
		public ulong CardInfo
		{
			get;
			private set;
		}
		public ulong UnusedSize
		{
			get;
			private set;
		}
		public string CardType
		{
			get;
			private set;
		}
		public NyamlOption.CardDeviceName CardDevice
		{
			get;
			private set;
		}
		public string Cci2a
		{
			get;
			private set;
		}
		public string Cci2b
		{
			get;
			private set;
		}
		public bool WillMakeCci2Merge
		{
			get;
			private set;
		}
		public NyamlOption UserOptions
		{
			get;
			private set;
		}
		public NyamlOption DefaultOptions
		{
			get;
			private set;
		}
		private string UserOptionFile
		{
			get;
			set;
		}
		private string DescriptorFile
		{
			get;
			set;
		}
		public NyamlParameter UserParam
		{
			get;
			private set;
		}
		public NyamlParameter DefaultParam
		{
			get;
			private set;
		}
		public NyamlParameter DefaultMakeromParam
		{
			get;
			private set;
		}
		public bool CreateNcch
		{
			get;
			private set;
		}
		public List<string> NcchFiles
		{
			get;
			private set;
		}
		public List<int> NoWarnList
		{
			get;
			private set;
		}
		public Dictionary<string, string> UserVariables
		{
			get;
			private set;
		}
		public bool DecMode
		{
			get;
			private set;
		}
		public bool VerifyMode
		{
			get;
			private set;
		}
		public MakeCciOptions(string[] args)
		{
			this.CxiOption = new CxiOption();
			this.NcchFiles = new List<string>();
			this.NoWarnList = new List<int>();
			this.CreateNcch = true;
			this.IsForCard = true;
			this.PageSize = 4096u;
			this.UserVariables = new Dictionary<string, string>();
			this.DecMode = false;
			this.AlignSize = 512;
			this.MediaUnitSize = (byte)(Math.Log((double)this.AlignSize, 2.0) - 9.0);
			NcsdHeader.MediaPlatform[] ncsdMediaPlatforms = new NcsdHeader.MediaPlatform[1];
			this.NcsdMediaPlatforms = ncsdMediaPlatforms;
			this.IsCip = false;
			this.IsCfa = false;
			this.WillMakeCci2Merge = false;
			this.ParseOption(args);
			this.CxiOption.UserVariables = this.UserVariables;
			this.CxiOption.MediaUnitSize = this.MediaUnitSize;
			this.CheckParameterCombination();
			if (this.OutputFormat == MakeCciOptions.OutputFormatName.CCL)
			{
				return;
			}
			Warning.NoPrintList = this.NoWarnList;
			Parameter parameter = new Parameter(this.UserOptionFile, this.DescriptorFile, this.UserVariables);
			NyamlParameter mergedUserParameter = parameter.GetMergedUserParameter();
			this.UserOptions = new NyamlOption(mergedUserParameter);
			this.SetOptionsFromYaml();
			this.SetDefaults();
			if (this.UserOptions.GetCardMediaType() == NyamlOption.CardMediaType.CARD2)
			{
				this.NcsdMediaType = NcsdHeader.MediaType.CARD2;
				return;
			}
			this.NcsdMediaType = NcsdHeader.MediaType.CARD1;
		}
		public ulong GetRawMediaSize()
		{
			Dictionary<string, ulong> dictionary = new Dictionary<string, ulong>
			{

				{
					"128mb",
					134217728uL
				},

				{
					"256mb",
					268435456uL
				},

				{
					"512mb",
					536870912uL
				},

				{
					"1gb",
					1073741824uL
				},

				{
					"2gb",
					2147483648uL
				},

				{
					"4gb",
					4294967296uL
				},

				{
					"8gb",
					8589934592uL
				},

				{
					"16gb",
					17179869184uL
				},

				{
					"32gb",
					34359738368uL
				}
			};
			string text = this.UserOptions.GetMediaSize().ToLower();
			if (!dictionary.ContainsKey(text))
			{
				throw new InvalidParameterException("MediaSize", text);
			}
			return dictionary[text];
		}
		public uint GetMediaSize()
		{
			return Util.ByteToMediaUnit((long)this.GetRawMediaSize(), this.MediaUnitSize);
		}
		public uint GetUnusedSize()
		{
			string key = this.UserOptions.GetMediaSize().ToLower();
			Dictionary<string, uint> dictionary = new Dictionary<string, uint>
			{

				{
					"128mb",
					2621440u
				},

				{
					"256mb",
					5242880u
				},

				{
					"512mb",
					10485760u
				},

				{
					"1gb",
					73924608u
				},

				{
					"2gb",
					147324928u
				},

				{
					"4gb",
					294649856u
				},

				{
					"8gb",
					587202560u
				}
			};
			Dictionary<string, uint> dictionary2 = new Dictionary<string, uint>
			{

				{
					"512mb",
					37224448u
				},

				{
					"1gb",
					73924608u
				},

				{
					"2gb",
					147324928u
				},

				{
					"4gb",
					294649856u
				},

				{
					"8gb",
					587202560u
				}
			};
			Dictionary<string, uint> dictionary3;
			if (this.UserOptions.GetCardMediaType() == NyamlOption.CardMediaType.CARD1)
			{
				dictionary3 = dictionary;
			}
			else
			{
				if (this.UserOptions.GetCardMediaType() != NyamlOption.CardMediaType.CARD2)
				{
					throw new InvalidParameterException("CardType", this.UserOptions.GetCardType());
				}
				dictionary3 = dictionary2;
			}
			return dictionary3[key];
		}
		private void CheckParameterCombination()
		{
			if (this.UserOptionFile == null && this.OutputFormat != MakeCciOptions.OutputFormatName.CCL)
			{
				throw new ArgumentException("No rsf input is found.");
			}
			if (this.ObjectName == null && this.CxiOption.ElfPath == null)
			{
				throw new ArgumentException("No output file is set.");
			}
			if (this.OutputFormat == MakeCciOptions.OutputFormatName.CCI && this.CreateNcch)
			{
				if (this.DescriptorFile == null)
				{
					throw new ArgumentException("No desc input is found.");
				}
				if (this.CxiOption.ElfPath == null)
				{
					throw new ArgumentException("No elf input is found.");
				}
				if (this.CxiOption.ExefsReserveSize != 0L)
				{
					throw new ArgumentException("Memory size is unavailable (-f card): -m");
				}
			}
			if (this.OutputFormat == MakeCciOptions.OutputFormatName.CXI || this.OutputFormat == MakeCciOptions.OutputFormatName.CAA)
			{
				if (this.DescriptorFile == null)
				{
					throw new ArgumentException("No desc input is found.");
				}
				if (this.CxiOption.ElfPath == null)
				{
					throw new ArgumentException("No elf input is found.");
				}
				if (this.CxiOption.ExefsReserveSize != 0L)
				{
					throw new ArgumentException("Memory size is unavailable (-f nand): -m");
				}
			}
			if (this.OutputFormat == MakeCciOptions.OutputFormatName.CFA)
			{
				if (this.DescriptorFile != null)
				{
					throw new ArgumentException("Invalid parameter combination \"-f data\" and \"-desc\"");
				}
				if (this.CxiOption.ElfPath != null)
				{
					throw new ArgumentException("Invalid parameter combination \"-f data\" and \"elf file\"");
				}
			}
			if (this.OutputFormat == MakeCciOptions.OutputFormatName.ROMFS || this.OutputFormat == MakeCciOptions.OutputFormatName.RBIN)
			{
				if (this.DescriptorFile == null)
				{
					throw new ArgumentException("No desc input is found.");
				}
				if (this.CxiOption.ElfPath != null)
				{
					throw new ArgumentException("Invalid parameter combination \"-f lr\" and \"elf file\"");
				}
			}
			if (this.OutputFormat == MakeCciOptions.OutputFormatName.EBIN)
			{
				if (this.DescriptorFile == null)
				{
					throw new ArgumentException("No desc input is found.");
				}
				if (this.CxiOption.ElfPath == null)
				{
					throw new ArgumentException("No elf input is found.");
				}
				if (this.CxiOption.ExefsReserveSize == 0L)
				{
					throw new ArgumentException("No memory size option: -m");
				}
				if (this.Cci2b == null)
				{
					throw new ArgumentException("No rbin input is found.");
				}
				if (this.CxiOption.DotRomfsPath != null)
				{
					throw new ArgumentException("-romfs option is unavailable (-f lr)");
				}
			}
			if (this.OutputFormat == MakeCciOptions.OutputFormatName.CCL)
			{
				if (this.DescriptorFile != null)
				{
					throw new ArgumentException("Desc file not required");
				}
				if (this.CxiOption.ElfPath != null)
				{
					throw new ArgumentException("Elf file not required");
				}
				if (this.Cci2a == null)
				{
					throw new ArgumentException("No ebin input is found.");
				}
				if (this.Cci2b == null)
				{
					throw new ArgumentException("No rbin input is found.");
				}
			}
		}
		private NyamlParameter DecideParams()
		{
			NyamlParameter nyamlParameter = (NyamlParameter)this.UserParam.Clone();
			nyamlParameter.Merge(this.DefaultParam);
			return nyamlParameter;
		}
		private ulong CreateCardInfo()
		{
			string cardType = this.UserOptions.GetCardType();
			int num;
			if (cardType == "s1")
			{
				num = 0;
			}
			else
			{
				if (!(cardType == "s2"))
				{
					throw new MakeromException("Invalid card type: " + cardType);
				}
				num = 1;
			}
			int cardCryptoType = this.UserOptions.GetCardCryptoType();
			ulong num2 = this.UserOptions.GetWritableAddress();
			if (this.UserOptions.GetCardMediaType() == NyamlOption.CardMediaType.CARD2 && num2 == 2199023255040uL)
			{
				if (this.GetRawMediaSize() / 2uL < this.UserOptions.GetSaveDataSize())
				{
					throw new MakeromException(string.Format("Too large SaveDataSize \"{0}\".\n", (this.UserOptions.GetSaveDataSize() / 1024uL).ToString() + "K"));
				}
				if (this.UserOptions.GetSaveDataSize() > 2146435072uL)
				{
					throw new MakeromException(string.Format("Too large SaveDataSize \"{0}\".\n", (this.UserOptions.GetSaveDataSize() / 1024uL).ToString() + "K"));
				}
				num2 = this.GetRawMediaSize() - (ulong)this.GetUnusedSize() - this.UserOptions.GetSaveDataSize();
			}
			byte[] array = new byte[8];
			BitConverter.GetBytes(num2 / 512uL).CopyTo(array, 0);
			array[7] = (byte)(cardCryptoType << 6 | num << 5);
			return BitConverter.ToUInt64(array, 0);
		}
		private void SetOptionsFromYaml()
		{
			this.IsForCard = (!this.IsCip && this.UserOptions.GetIsForCard());
			this.MediaSize = this.UserOptions.GetMediaSize();
			this.MediaFootPadding = this.UserOptions.GetMediaFootPadding();
			this.Padding = this.UserOptions.GetPadding();
			this.CardType = this.UserOptions.GetCardType();
			if (this.OutputFormat == MakeCciOptions.OutputFormatName.CCI || this.OutputFormat == MakeCciOptions.OutputFormatName.EBIN)
			{
				this.CardInfo = this.CreateCardInfo();
				this.CardDevice = this.UserOptions.GetCardDevice();
			}
		}
		private void ParseOption(string[] args)
		{
			OptionParser optionParser = new OptionParser();
			optionParser.AddOption(new Option("-D DECLARE-VARIABLE", "--declare-variable", delegate(string arg)
			{
				try
				{
					string[] array2 = arg.Split(new char[]
					{
						'='
					});
					if (array2.Length >= 2)
					{
						this.UserVariables.Add(array2[0].Trim(), array2[1].Trim());
					}
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
				return true;
			}, true, true));
			optionParser.AddOption(new Option("-nowarn WARNING-NUM", "--no-warning", delegate(string arg)
			{
				if (!Regex.Match(arg, "[0-9]+").Success)
				{
					throw new ArgumentException("-nowarn should be specified with decimal format");
				}
				int num = int.Parse(arg);
				if (num != 0)
				{
					throw new ArgumentException(string.Format("not implemented warning number \"{0}\".", num));
				}
				this.NoWarnList.Add(num);
				return true;
			}, true, true));
			optionParser.AddOption(new Option("-f OUTPUT-FORMAT", "--output-format", delegate(string arg)
			{
				if (arg.ToLower() == "card")
				{
					this.OutputFormat = MakeCciOptions.OutputFormatName.CCI;
					this.CxiOption.ForCard = true;
				}
				else
				{
					if (arg.ToLower() == "nand" || arg.ToLower() == "exec")
					{
						this.OutputFormat = MakeCciOptions.OutputFormatName.CXI;
					}
					else
					{
						if (arg.ToLower() == "data")
						{
							this.OutputFormat = MakeCciOptions.OutputFormatName.CFA;
							this.IsCfa = true;
							this.CxiOption.Cfa = true;
						}
						else
						{
							if (arg.ToLower() == "romfs")
							{
								this.OutputFormat = MakeCciOptions.OutputFormatName.ROMFS;
							}
							else
							{
								if (arg.ToLower() == "list")
								{
									this.OutputFormat = MakeCciOptions.OutputFormatName.CCL;
									this.CxiOption.ForCci2 = true;
								}
								else
								{
									if (arg.ToLower() == "le")
									{
										this.OutputFormat = MakeCciOptions.OutputFormatName.EBIN;
										this.CxiOption.ForCci2 = true;
									}
									else
									{
										if (arg.ToLower() == "lr")
										{
											this.OutputFormat = MakeCciOptions.OutputFormatName.RBIN;
											this.CxiOption.ForCci2 = true;
										}
										else
										{
											if (!(arg.ToLower() == "raw"))
											{
												throw new Exception(string.Format("-f cannot accept {0}. -f (card|nand)", arg));
											}
											this.OutputFormat = MakeCciOptions.OutputFormatName.CAA;
											this.CxiOption.Caa = true;
										}
									}
								}
							}
						}
					}
				}
				return true;
			}, true));
			optionParser.AddOption(new Option("-m RESERVE-MEMORY-SIZE", "--reserve-memory-size", delegate(string arg)
			{
				if (!arg.StartsWith("0x"))
				{
					throw new ArgumentException("-m should be specified with hex format");
				}
				this.CxiOption.ExefsReserveSize = long.Parse(arg.Substring(2), NumberStyles.AllowHexSpecifier);
				return true;
			}, true));
			optionParser.AddOption(new Option("-le CCL-EBIN", "--ccl-ebin", delegate(string arg)
			{
				this.Cci2a = arg;
				return true;
			}, true));
			optionParser.AddOption(new Option("-lr CCL-RBIN", "--ccl-rbin", delegate(string arg)
			{
				this.Cci2b = arg;
				return true;
			}, true));
			optionParser.AddOption(new Option("-ccl-merge-image", "--ccl-merge-image", delegate(string arg)
			{
				this.WillMakeCci2Merge = true;
				return true;
			}, true));
			optionParser.AddOption(new Option("-o OBJECT-NAME", "--object-name", delegate(string arg)
			{
				this.ObjectName = arg;
				return true;
			}, true));
			optionParser.AddOption(new Option("-info INFORMATION-FILE-NAME", "--informateion-file-name", delegate(string arg)
			{
				this.InfoName = arg;
				return true;
			}, true));
			optionParser.AddOption(new Option("-rsf RSF_FILE", "--rsf-file", delegate(string arg)
			{
				this.UserOptionFile = arg;
				this.CxiOption.RsfPath = arg;
				return true;
			}, false));
			optionParser.AddOption(new Option("-desc DESCRIPTER_FILE", "--desc-file", delegate(string arg)
			{
				this.DescriptorFile = arg;
				this.CxiOption.DescPath = arg;
				return true;
			}, false));
			optionParser.AddOption(new Option("-align", "--align-size", delegate(string arg)
			{
				this.AlignSize = int.Parse(arg);
				if (this.AlignSize <= 0 || this.AlignSize > 512)
				{
					throw new ArgumentException("align size must be 1 - 512");
				}
				this.CxiOption.Align = this.AlignSize;
				return true;
			}, true));
			optionParser.AddOption(new Option("-cip", "--ctr-initial-process", delegate(string arg)
			{
				this.CxiOption.Cip = true;
				return true;
			}, true));
			optionParser.AddOption(new Option("-content", "--additional-content", delegate(string arg)
			{
				this.NcchFiles.Add(arg);
				return true;
			}, true, true));
			optionParser.AddOption(new Option("-coreinfo", "--output-core-info", delegate(string arg)
			{
				this.CxiOption.OutputCoreInfo = true;
				return true;
			}, true));
			optionParser.AddOption(new Option("-banner BANNER-PATH", "--banner-path", delegate(string arg)
			{
				this.CxiOption.BannerPath = arg;
				return true;
			}, true));
			optionParser.AddOption(new Option("-icon ICON-PATH", "--icon-path", delegate(string arg)
			{
				this.CxiOption.IconPath = arg;
				return true;
			}, true));
			optionParser.AddOption(new Option("-key KEY-DATA", "--key-data", delegate(string arg)
			{
				this.CxiOption.AesKey = Nintendo.MakeRom.Util.GetKeyData(arg);
				return true;
			}, true));
			optionParser.AddOption(new Option("-keyfile KEY-DATA-FILE", "--key-data-file", delegate(string arg)
			{
				this.CxiOption.AesKey = File.ReadAllBytes(arg);
				return true;
			}, true));
			optionParser.AddOption(new Option("-cdi", "--ctr-development-image", delegate(string arg)
			{
				this.CxiOption.Cdi = true;
				return true;
			}, true));
			optionParser.AddOption(new Option("-exename EXEFS-SECTION-NAME", "--exefs-section-name", delegate(string arg)
			{
				this.CxiOption.TopExefsSectionName = arg;
				return true;
			}, true));
			optionParser.AddOption(new Option("-dv DEPENDECY-VARIATION", "--dependency-variation", delegate(string arg)
			{
				this.CxiOption.DependencyVariation = byte.Parse(arg);
				return true;
			}, true));
			optionParser.AddOption(new Option("-romfs ROMFS-FILENAME", "--romfs-path", delegate(string arg)
			{
				this.CxiOption.DotRomfsPath = arg;
				return true;
			}, true));
			optionParser.AddOption(new Option("-layout ROMFS-LAYOUT-FILENAME", "--romfs-layout", delegate(string arg)
			{
				this.CxiOption.RomfsLayoutPath = arg;
				return true;
			}, true));
			optionParser.AddOption(new Option("-p", "--profile", delegate(string arg)
			{
				Profiler.IsEnableProfile = true;
				return true;
			}, true));
			optionParser.AddOption(new Option("-cc", "--cip-compress", delegate(string arg)
			{
				this.CxiOption.CipCompress = true;
				return true;
			}, true));
			optionParser.AddOption(new Option("-j", "--jobs", delegate(string arg)
			{
				int num = int.Parse(arg);
				if (num > 0)
				{
					MulticoreCryptoStream.SetUsingThreadNumber(num);
				}
				return true;
			}, true));
			args = this.ConvertDeclareVariablesArgs(args);
			string[] array = optionParser.Parse(args);
			if (array != null && array.Length == 1)
			{
				this.CxiOption.ElfPath = array[0];
			}
			if ((array == null || array.Length == 0) && this.NcchFiles.Count != 0)
			{
				this.CreateNcch = false;
			}
			if (array.Length > 1)
			{
				throw new ArgumentException(string.Format("Too many arguments or Unknown arguments", new object[0]));
			}
		}
		private string[] ConvertDeclareVariablesArgs(string[] args)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < args.Length; i++)
			{
				string text = args[i];
				string pattern = "-D(\\w+=(" + RegexPattern.REGEX_REGEX_CHAR + "|\\s)*)";
				MatchCollection matchCollection = Regex.Matches(text, pattern);
				if (matchCollection.Count == 1)
				{
					list.Add("-D");
					list.Add(matchCollection[0].Groups[1].ToString());
				}
				else
				{
					list.Add(text);
				}
			}
			return list.ToArray();
		}
		private void SetDefaults()
		{
			if (this.ObjectName == null)
			{
				this.ObjectName = this.GetObjectNameFromFilePath(this.CxiOption.ElfPath, this.IsCip ? "cip" : "cci");
				this.ObjectName = (this.IsCfa ? this.GetObjectNameFromFilePath(this.CxiOption.ElfPath, "cfa") : this.ObjectName);
				switch (this.OutputFormat)
				{
				case MakeCciOptions.OutputFormatName.CXI:
				case MakeCciOptions.OutputFormatName.CAA:
					this.ObjectName = this.GetObjectNameFromFilePath(this.CxiOption.ElfPath, "cxi");
					break;
				case MakeCciOptions.OutputFormatName.ROMFS:
				case MakeCciOptions.OutputFormatName.CCL:
				case MakeCciOptions.OutputFormatName.EBIN:
				case MakeCciOptions.OutputFormatName.RBIN:
					this.ObjectName = this.GetObjectNameFromFilePath(this.CxiOption.ElfPath, Enum.GetName(typeof(MakeCciOptions.OutputFormatName), this.OutputFormat).ToLower());
					break;
				}
			}
			if (this.IsCip)
			{
				this.OutputFormat = MakeCciOptions.OutputFormatName.CXI;
			}
			if (this.InfoName == null)
			{
				this.InfoName = this.ObjectName + ".xml";
			}
		}
		private string GetObjectNameFromFilePath(string filePath, string suffix)
		{
			Regex regex = new Regex("\\.[^\\.]*\\z");
			Match match = regex.Match(filePath);
			string result;
			if (match.Success)
			{
				result = regex.Replace(filePath, "." + suffix, 1);
			}
			else
			{
				result = filePath + "." + suffix;
			}
			return result;
		}
	}
}
