using Nintendo.MakeRom.MakeFS;
using Nintendo.MakeRom.Ncch.FastBuildRomfs;
using System;
using System.IO;
using System.Xml.Serialization;
namespace Nintendo.MakeRom.Test
{
	internal class RomfsInfoXmlTest
	{
		public static void RunTest()
		{
			RomfsInfoXmlTest.DumpAndRead();
			Console.WriteLine("Waiting..");
			Console.In.ReadLine();
		}
		private static void TryDump()
		{
			FastBuildRomFsInfo fastBuildRomFsInfo = new FastBuildRomFsInfo();
			fastBuildRomFsInfo.AddFile(new Entry.Input
			{
				m_Offset = 3uL,
				m_PathName = "/relocate.test",
				m_Size = 100uL
			}, new Creator.Input
			{
				m_Offset = 74565uL,
				m_PCName = "C:/cygwin/home/N2746/.emacs",
				m_Size = 13398uL
			});
			fastBuildRomFsInfo.Entries.Add(new LayoutLocal
			{
				CtrPath = "/layout",
				Resource = new DataResource
				{
					Type = ResourceType.InlineString,
					Inline = "sample data"
				},
				m_Position = new HexableNumber(291L)
			});
			XmlSerializer xmlSerializer = new XmlSerializer(fastBuildRomFsInfo.GetType());
			xmlSerializer.Serialize(Console.Out, fastBuildRomFsInfo);
		}
		private static void DumpAndRead()
		{
			string tempFileName = Path.GetTempFileName();
			FastBuildRomFsInfo fastBuildRomFsInfo = new FastBuildRomFsInfo();
			fastBuildRomFsInfo.AddFile(new Entry.Input
			{
				m_Offset = 3uL,
				m_PathName = "/relocate.test",
				m_Size = 100uL
			}, new Creator.Input
			{
				m_Offset = 74565uL,
				m_PCName = "C:/cygwin/home/N2746/.emacs",
				m_Size = 13398uL
			});
			fastBuildRomFsInfo.Entries.Add(new LayoutLocal
			{
				CtrPath = "/layout",
				Resource = new DataResource
				{
					Type = ResourceType.InlineString,
					Inline = "sample data"
				},
				m_Position = new HexableNumber(291L)
			});
			XmlSerializer xmlSerializer = new XmlSerializer(fastBuildRomFsInfo.GetType());
			using (FileStream fileStream = new FileStream(tempFileName, FileMode.Create, FileAccess.ReadWrite))
			{
				xmlSerializer.Serialize(fileStream, fastBuildRomFsInfo);
				fileStream.Seek(0L, SeekOrigin.Begin);
				xmlSerializer.Deserialize(fileStream);
				for (int i = 0; i < fastBuildRomFsInfo.Entries.Count; i++)
				{
				}
			}
			File.Delete(tempFileName);
		}
	}
}
