using makerom.Ncsd2;
using makerom.Properties;
using Nintendo.MakeRom;
using Nintendo.MakeRom.Extensions;
using Nintendo.MakeRom.Ncch.FastBuildRomfs;
using nyaml;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
namespace makerom
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.UnhandledExceptionEventHandler);
			try
			{
				Profiler.IsEnableProfile = true;
				Profiler.Entry("Main");
				Profiler.IsEnableProfile = false;
				MakeCciOptions options = new MakeCciOptions(args);
				Program.Run(options);
				Profiler.Exit("Main");
				if (Profiler.IsEnableProfile)
				{
					Console.WriteLine("Profiling Result-----------------------");
					Profiler.Dump();
				}
			}
			catch (SyntaxErrorException ex)
			{
				Program.ErrorLog(ex.Message);
				Environment.Exit(-1);
			}
			catch (FileNotFoundException ex2)
			{
				Program.ErrorLog(ex2.Message);
				Environment.Exit(-1);
			}
			catch (WrongCastException ex3)
			{
				Program.ErrorLog(ex3.Message);
				Environment.Exit(-1);
			}
			catch (MakeromException ex4)
			{
				Program.ErrorLog(ex4.Message);
				Environment.Exit(-1);
			}
			catch (ArgumentException ex5)
			{
				Program.ErrorLog(ex5.Message);
				Program.PrintUsage();
				Environment.Exit(-1);
			}
			catch (Exception ex6)
			{
				Program.ErrorLog(ex6.Message);
				Program.ErrorLog(ex6.StackTrace);
				Environment.Exit(-1);
			}
		}
		private static void Run(MakeCciOptions options)
		{
			if (options.DecMode)
			{
				byte[] data = File.ReadAllBytes(options.ObjectName);
				AesCtr aes = new AesCtr(Resources.key, 0uL, 0uL);
				File.WriteAllBytes("out.bin", Program.Decrypto(data, aes, 18976, 2048));
				Environment.Exit(0);
			}
			if (options.VerifyMode)
			{
				byte[] data2 = File.ReadAllBytes(options.ObjectName);
				Program.VerifySign(options, data2);
				Environment.Exit(0);
			}
			if (options.OutputFormat == MakeCciOptions.OutputFormatName.CCI)
			{
				if (options.MediaType == MakeCciOptions.MediaTypeName.NAND)
				{
					throw new Exception("NAND application is not to be CCI");
				}
				Ncsd ncsd = new Ncsd(options);
				if (options.CreateNcch)
				{
					Profiler.Entry("Build Partition 0");
					NcchBinary ncch = MakeCxi.Make(options.CxiOption);
					Profiler.Exit("Build Partition 0");
					ncsd.AddNcch(ncch, 0);
				}
				foreach (string current in options.NcchFiles)
				{
					int partitionIndex = Program.GetPartitionIndex(current);
					string cxiPath = Program.GetCxiPath(current);
					Profiler.Entry(string.Format("Load Partition {0}", partitionIndex));
					NcchFileBinary ncch2 = MakeCxi.Load(cxiPath);
					ncsd.AddNcch(ncch2, partitionIndex);
					Profiler.Exit(string.Format("Load Partition {0}", partitionIndex));
				}
				Profiler.Entry("Write into File");
				BinaryWriter binaryWriter = new BinaryWriter(new FileStream(options.ObjectName, FileMode.Create));
				ncsd.WriteBinary(binaryWriter);
				binaryWriter.Close();
				Profiler.Exit("Write into File");
				Program.WriteBinaryInfo(ncsd.CciInfo, options.InfoName);
				return;
			}
			else
			{
				if (options.OutputFormat == MakeCciOptions.OutputFormatName.CXI || options.OutputFormat == MakeCciOptions.OutputFormatName.CFA || options.OutputFormat == MakeCciOptions.OutputFormatName.CAA)
				{
					NcchBinary ncchBinary = MakeCxi.Make(options.CxiOption);
					BinaryWriter binaryWriter2 = new BinaryWriter(new FileStream(options.ObjectName, FileMode.Create));
					ncchBinary.WriteBinary(binaryWriter2);
					Program.WriteBinaryInfo(ncchBinary.CxiInfo, options.InfoName);
					binaryWriter2.Close();
					return;
				}
				if (options.OutputFormat == MakeCciOptions.OutputFormatName.ROMFS)
				{
					using (FileStream fileStream = new FileStream(options.ObjectName, FileMode.Create, FileAccess.ReadWrite))
					{
						using (BinaryWriter binaryWriter3 = new BinaryWriter(fileStream))
						{
							RomfsOnlyNcchBinary romfsOnlyNcchBinary = MakeCxi.MakeRomfsOnlyNcchBinary(options.CxiOption);
							romfsOnlyNcchBinary.WriteBinary(binaryWriter3);
							Program.WriteBinaryInfo(romfsOnlyNcchBinary.RomfsInfo, options.InfoName);
						}
						return;
					}
				}
				if (options.OutputFormat == MakeCciOptions.OutputFormatName.CCL)
				{
					NcsdBinary2 ncsdBinary = new NcsdBinary2(options.Cci2a, options.Cci2b, options.ObjectName);
					Program.WriteBinaryInfo(ncsdBinary.Cci2Image, options.ObjectName);
					if (options.WillMakeCci2Merge)
					{
						ncsdBinary.Dump(options.ObjectName + ".merge");
						return;
					}
				}
				else
				{
					if (options.OutputFormat == MakeCciOptions.OutputFormatName.EBIN)
					{
						if (!options.CreateNcch)
						{
							throw new ArgumentException("CCI2a requires axf");
						}
						NcsdBinary2a ncsdBinary2a = new NcsdBinary2a(options);
						ncsdBinary2a.AddNcch(MakeCxi.Make(options.CxiOption), 0);
						using (FileStream fileStream2 = new FileStream(options.Cci2b, FileMode.Open, FileAccess.Read))
						{
							FastBuildRomfsHeader fastBuildRomfsHeader = new FastBuildRomfsHeader();
							fastBuildRomfsHeader.Read(fileStream2);
							fileStream2.Seek((long)FastBuildRomfsHeader.MakeromfsInfoSize + fastBuildRomfsHeader.RomfsSize, SeekOrigin.Begin);
							while (fileStream2.Position < fileStream2.Length)
							{
								long position = fileStream2.Position;
								NcchBinary ncchBinary2 = MakeCxi.LoadFromStream(fileStream2);
								int partitionIndex2 = (int)((ncchBinary2.GetPartitionId() >> 48) - 4uL);
								ncsdBinary2a.AddNcch(ncchBinary2, partitionIndex2);
								fileStream2.Seek(position + ncchBinary2.Size, SeekOrigin.Begin);
							}
						}
						using (FileStream fileStream3 = new FileStream(options.ObjectName, FileMode.Create))
						{
							using (BinaryWriter binaryWriter4 = new BinaryWriter(fileStream3))
							{
								ncsdBinary2a.WriteBinary(binaryWriter4);
							}
						}
						Program.WriteBinaryInfo(ncsdBinary2a.CciInfo, options.InfoName);
						return;
					}
					else
					{
						if (options.OutputFormat == MakeCciOptions.OutputFormatName.RBIN)
						{
							NcsdBinary2b ncsdBinary2b = new NcsdBinary2b(options);
							RomfsOnlyNcchBinary romfsOnlyNcchBinary2 = MakeCxi.MakeRomfsOnlyNcchBinary(options.CxiOption);
							ncsdBinary2b.AddNcch(romfsOnlyNcchBinary2, 0);
							foreach (string current2 in options.NcchFiles)
							{
								int partitionIndex3 = Program.GetPartitionIndex(current2);
								string cxiPath2 = Program.GetCxiPath(current2);
								NcchFileBinary ncch3 = MakeCxi.Load(cxiPath2);
								ncsdBinary2b.AddNcch(ncch3, partitionIndex3);
							}
							using (FileStream fileStream4 = new FileStream(options.ObjectName, FileMode.Create))
							{
								using (BinaryWriter binaryWriter5 = new BinaryWriter(fileStream4))
								{
									ncsdBinary2b.WriteBinary(binaryWriter5);
								}
							}
							Ncsd2bCciInfoRoot ncsd2bCciInfoRoot = new Ncsd2bCciInfoRoot();
							ncsd2bCciInfoRoot.LoadRomfsInfo(romfsOnlyNcchBinary2.RomfsInfo);
							Program.WriteBinaryInfo(ncsd2bCciInfoRoot.CciInfo, options.InfoName);
						}
					}
				}
				return;
			}
		}
		private static void WriteBinaryInfo(object o, string filename)
		{
			for (int i = 0; i < 40; i++)
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());
					FileStream fileStream = new FileStream(filename, FileMode.Create);
					xmlSerializer.Serialize(fileStream, o);
					fileStream.Close();
					break;
				}
				catch (Exception ex)
				{
					if (i == 39)
					{
						throw ex;
					}
				}
			}
		}
		private static int GetPartitionIndex(string path)
		{
			int num = path.LastIndexOf(':');
			string s = path.Substring(num + 1);
			int result;
			try
			{
				result = int.Parse(s);
			}
			catch (Exception)
			{
				throw new MakeromException("Invalid cxi path and partition index.\n -cxi PATH:[0-7]");
			}
			return result;
		}
		private static string GetCxiPath(string path)
		{
			int length = path.LastIndexOf(':');
			return path.Substring(0, length);
		}
		private static void VerifySign(MakeCciOptions options, byte[] data)
		{
			throw new Exception("Not implemented");
		}
		private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Console.WriteLine("Unexpected error. exiting...");
			Environment.Exit(-1);
		}
		[Conditional("DEBUG")]
		private static void DebugLog(string s)
		{
			Console.WriteLine("[DEBUG LOG] " + s);
		}
		private static void ErrorLog(string s)
		{
			Console.WriteLine("[MAKEROM ERROR] Failed to make cci: " + s);
		}
		private static byte[] Decrypto(byte[] data, AesCtr aes, int begin, int size)
		{
			byte[] array = new byte[data.Length];
			data.CopyTo(array, 0);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, aes, CryptoStreamMode.Write);
			cryptoStream.Write(data, begin, size);
			memoryStream.ToArray().CopyTo(array, begin);
			return array;
		}
		private static void PrintUsage()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("Usage: {0} ELF_FILE -rsf RSF_FILE -desc DESC_FILE\n", Assembly.GetEntryAssembly().ManifestModule.Name));
			stringBuilder.Append("                         [-o OUTPUT_FILE] [-DNAME=VALUE ...]\n");
			Console.Write(stringBuilder.ToString());
		}
	}
}
