using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
namespace Nintendo.MakeRom.MakeFS
{
	public class MetaGenerator
	{
		public class Input
		{
			public bool m_IsFile;
			public string m_PCPathName;
			public string m_PathName;
			public uint m_ID;
			public ulong m_Offset;
			public ulong m_Size;
		}
		private class TreeNode
		{
			public List<MetaGenerator.TreeNode> m_ChildList = new List<MetaGenerator.TreeNode>();
			public string m_Path = "";
			public MetaGenerator.Input m_Input;
		}
		private static string[] m_ReservedWords = new string[]
		{
			"auto",
			"bool",
			"break",
			"case",
			"catch",
			"char",
			"class",
			"const",
			"const_cast",
			"continue",
			"default",
			"delete",
			"do",
			"double",
			"dynamic_cast",
			"else",
			"enum",
			"explicit",
			"extern",
			"false",
			"finally",
			"float",
			"for",
			"friend",
			"goto",
			"if",
			"inline",
			"int",
			"long",
			"mutable",
			"namespace",
			"new",
			"null",
			"nullptr",
			"operator",
			"private",
			"property",
			"protected",
			"public",
			"ref",
			"register",
			"reinterpret_cast",
			"return",
			"short",
			"signed",
			"sizeof",
			"static",
			"static_cast",
			"struct",
			"switch",
			"template",
			"this",
			"throw",
			"true",
			"try",
			"typedef",
			"typeid",
			"typename",
			"union",
			"unsigned",
			"using",
			"virtual",
			"void",
			"volatile",
			"while"
		};
		private static HashSet<string> m_ReservedWordsHash = new HashSet<string>(MetaGenerator.m_ReservedWords);
		private static MetaGenerator.TreeNode CreateNode(MetaGenerator.Input[] inData)
		{
			MetaGenerator.TreeNode treeNode = new MetaGenerator.TreeNode();
			for (int i = 0; i < inData.Length; i++)
			{
				MetaGenerator.Input input = inData[i];
				if (input.m_PathName[0] != '/')
				{
					throw new InvalidPathFormatException(input.m_PathName);
				}
				if (input.m_PathName == "/")
				{
					treeNode.m_Path = "";
					treeNode.m_Input = input;
				}
				else
				{
					MetaGenerator.CreateNodeSub(treeNode, input.m_PathName.Substring(1), input);
				}
			}
			return treeNode;
		}
		private static void CreateNodeSub(MetaGenerator.TreeNode nodeParent, string path, MetaGenerator.Input inData)
		{
			int num = path.IndexOf('/');
			string text = null;
			if (num == 0)
			{
				throw new InvalidPathFormatException(inData.m_PathName);
			}
			if (-1 != num)
			{
				string text2 = path.Substring(0, num);
				if (path.Length > num + 1)
				{
					text = path.Substring(num + 1);
				}
				path = text2;
			}
			bool flag = false;
			foreach (MetaGenerator.TreeNode current in nodeParent.m_ChildList)
			{
				if (current.m_Path == path)
				{
					if (text == null)
					{
						current.m_Input = inData;
					}
					else
					{
						MetaGenerator.CreateNodeSub(current, text, inData);
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				MetaGenerator.TreeNode treeNode = new MetaGenerator.TreeNode();
				treeNode.m_Path = path;
				nodeParent.m_ChildList.Add(treeNode);
				if (text == null)
				{
					treeNode.m_Input = inData;
					return;
				}
				MetaGenerator.CreateNodeSub(treeNode, text, inData);
			}
		}
		private static void WriteNodeInfo(XmlTextWriter xml, MetaGenerator.TreeNode node, long level)
		{
			if (node.m_Input != null && node.m_Input.m_IsFile)
			{
				xml.WriteStartElement("file");
				xml.WriteAttributeString("name", node.m_Path);
				if (node.m_Input != null)
				{
					xml.WriteAttributeString("id", node.m_Input.m_ID.ToString());
					xml.WriteAttributeString("offset", node.m_Input.m_Offset.ToString());
					xml.WriteAttributeString("size", node.m_Input.m_Size.ToString());
					if (node.m_Input.m_PCPathName != null && node.m_Input.m_PCPathName.Length > 0)
					{
						xml.WriteAttributeString("hostPath", node.m_Input.m_PCPathName.ToString());
					}
				}
			}
			else
			{
				xml.WriteStartElement("directory");
				xml.WriteAttributeString("name", node.m_Path);
				if (level == 0L && node.m_Path == "")
				{
					xml.WriteAttributeString("root", "true");
				}
				if (node.m_Input != null)
				{
					xml.WriteAttributeString("id", node.m_Input.m_ID.ToString());
				}
				foreach (MetaGenerator.TreeNode current in node.m_ChildList)
				{
					MetaGenerator.WriteNodeInfo(xml, current, level + 1L);
				}
			}
			xml.WriteEndElement();
		}
		public static void CreateXML(Stream outStream, MetaGenerator.Input[] inData)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(outStream, Encoding.UTF8);
			MetaGenerator.TreeNode node = MetaGenerator.CreateNode(inData);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.IndentChar = ' ';
			xmlTextWriter.Indentation = 4;
			xmlTextWriter.WriteStartDocument(true);
			xmlTextWriter.WriteStartElement("rom");
			MetaGenerator.WriteNodeInfo(xmlTextWriter, node, 0L);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
		}
		private static string EscapeWord(string str, bool CheckReservedWord)
		{
			long num = 0L;
			string text = "";
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (num == 0L && -1 == "_ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".IndexOf(c))
				{
					text += '_';
				}
				else
				{
					if (-1 == "0123456789_ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".IndexOf(c))
					{
						text += '_';
					}
					else
					{
						text += c;
					}
				}
				num += 1L;
			}
			if (CheckReservedWord && MetaGenerator.m_ReservedWordsHash.Contains(text))
			{
				text += "_";
			}
			return text;
		}
		private static void WriteHeader(string dirRoot, string strNameSpace, string strNameSpaceUnescaped, MetaGenerator.TreeNode node, string strAdditionalNameSpace)
		{
			if (!Directory.Exists(dirRoot))
			{
				Directory.CreateDirectory(dirRoot);
			}
			Stream stream = new FileStream(Path.Combine(dirRoot, "entries.h"), FileMode.Create, FileAccess.Write);
			StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
			string text = "";
			if (strAdditionalNameSpace.Length > 0)
			{
				text = text + strAdditionalNameSpace + "_";
			}
			text += "ROM_";
			if (strNameSpace.Length > 0)
			{
				text = text + strNameSpaceUnescaped.Replace(":", "_") + "_";
			}
			text += "ENTRIES_H_";
			streamWriter.WriteLine("#ifndef " + text);
			streamWriter.WriteLine("#define " + text);
			streamWriter.WriteLine("");
			streamWriter.WriteLine("#ifndef NN_TYPES_H_");
			streamWriter.WriteLine("#include <nn/types.h>");
			streamWriter.WriteLine("#endif /* NN_TYPES_H_ */\r\n");
			streamWriter.WriteLine("#ifdef __cplusplus\r\n");
			string text2 = "";
			if (strAdditionalNameSpace.Length > 0)
			{
				text2 = text2 + "namespace " + strAdditionalNameSpace + " { ";
			}
			text2 += "namespace rom { ";
			if (strNameSpace.Length > 0)
			{
				text2 += "namespace ";
				text2 += strNameSpace.Replace(":", " { namespace ");
				text2 += " {";
			}
			string text3 = "";
			string text4 = text2;
			for (int i = 0; i < text4.Length; i++)
			{
				char c = text4[i];
				if (c == '{')
				{
					if (text3.Length > 0)
					{
						text3 += " ";
					}
					text3 += "}";
				}
			}
			streamWriter.WriteLine(text2 + "\r\n");
			if (node.m_Input != null)
			{
				streamWriter.WriteLine("  extern bit32 id;");
			}
			foreach (MetaGenerator.TreeNode current in node.m_ChildList)
			{
				if (current.m_Input != null && current.m_Input.m_IsFile)
				{
					string arg = MetaGenerator.EscapeWord(current.m_Path, false);
					streamWriter.WriteLine(string.Format("  extern const bit32 id_{0};", arg));
					streamWriter.WriteLine(string.Format("  extern const bit64 size_{0};", arg));
				}
				else
				{
					if (current.m_Input == null || !current.m_Input.m_IsFile)
					{
						string str = MetaGenerator.EscapeWord(current.m_Path, true);
						string dirRoot2 = dirRoot + str + "\\";
						string text5 = strNameSpace;
						if (text5.Length > 0)
						{
							text5 += ":";
						}
						text5 += str;
						str = MetaGenerator.EscapeWord(current.m_Path, false);
						string text6 = strNameSpaceUnescaped;
						if (text6.Length > 0)
						{
							text6 += ":";
						}
						text6 += str;
						MetaGenerator.WriteHeader(dirRoot2, text5, text6, current, strAdditionalNameSpace);
					}
				}
			}
			if (node.m_ChildList.Count > 0)
			{
				streamWriter.WriteLine("");
			}
			streamWriter.WriteLine(text3 + "\r\n");
			streamWriter.WriteLine("#endif /*__ cplusplus */\r\n");
			string text7 = "";
			if (strAdditionalNameSpace.Length > 0)
			{
				text7 = text7 + strAdditionalNameSpace + "_";
			}
			text7 += "ROM";
			if (strNameSpace.Length > 0)
			{
				text7 = text7 + "_" + strNameSpaceUnescaped.Replace(":", "_");
			}
			if (node.m_Input != null)
			{
				streamWriter.WriteLine("extern const bit32 ID_{0};", text7);
			}
			foreach (MetaGenerator.TreeNode current2 in node.m_ChildList)
			{
				if (current2.m_Input != null && current2.m_Input.m_IsFile)
				{
					string arg2 = MetaGenerator.EscapeWord(current2.m_Path, false);
					streamWriter.WriteLine(string.Format("extern const bit32 ID_{0}_{1};", text7, arg2));
					streamWriter.WriteLine(string.Format("extern const bit64 SIZE_{0}_{1};", text7, arg2));
				}
			}
			streamWriter.WriteLine("");
			streamWriter.WriteLine("#endif /* " + text + " */");
			streamWriter.Flush();
			streamWriter.Close();
			stream.Close();
		}
		private static void WriteBodyCpp(StreamWriter sw, MetaGenerator.TreeNode node, string strAdditionalNameSpace, long level)
		{
			string text = "";
			string str;
			if (level == 0L)
			{
				if (strAdditionalNameSpace.Length > 0)
				{
					sw.WriteLine("namespace " + strAdditionalNameSpace + " {");
					level = 1L;
					text = "  ";
				}
				str = "rom";
			}
			else
			{
				str = MetaGenerator.EscapeWord(node.m_Path, true);
				while ((long)text.Length < level * 2L)
				{
					text += "  ";
				}
			}
			sw.WriteLine(text + "namespace " + str + " {");
			if (node.m_Input != null)
			{
				sw.WriteLine(text + "  extern const bit32 id = {0};", node.m_Input.m_ID);
			}
			foreach (MetaGenerator.TreeNode current in node.m_ChildList)
			{
				if (current.m_Input != null && current.m_Input.m_IsFile)
				{
					string arg = MetaGenerator.EscapeWord(current.m_Path, false);
					sw.WriteLine(string.Format(text + "  extern const bit32 id_{0} = {1};", arg, current.m_Input.m_ID));
					sw.WriteLine(string.Format(text + "  extern const bit64 size_{0} = {1}ULL;", arg, current.m_Input.m_Size));
				}
				else
				{
					if (current.m_Input == null || !current.m_Input.m_IsFile)
					{
						MetaGenerator.WriteBodyCpp(sw, current, strAdditionalNameSpace, level + 1L);
					}
				}
			}
			sw.WriteLine(text + "}");
			if (level == 1L && strAdditionalNameSpace.Length > 0)
			{
				sw.WriteLine("}");
			}
		}
		private static void WriteBodyC(StreamWriter sw, string strNameSpace, MetaGenerator.TreeNode node, string strAdditionalNameSpace, long level)
		{
			string text = "";
			if (strAdditionalNameSpace.Length > 0)
			{
				text = text + strAdditionalNameSpace + "_";
			}
			text += "ROM";
			if (strNameSpace.Length > 0)
			{
				text = text + "_" + strNameSpace.Replace(":", "_");
			}
			if (node.m_Input != null)
			{
				sw.WriteLine("extern const bit32 ID_{0} = {1};", text, node.m_Input.m_ID);
			}
			foreach (MetaGenerator.TreeNode current in node.m_ChildList)
			{
				if (current.m_Input != null && current.m_Input.m_IsFile)
				{
					string arg = MetaGenerator.EscapeWord(current.m_Path, false);
					sw.WriteLine(string.Format("extern const bit32 ID_{0}_{1} = {2};", text, arg, current.m_Input.m_ID));
					sw.WriteLine(string.Format("extern const bit64 SIZE_{0}_{1} = {2}ULL;", text, arg, current.m_Input.m_Size));
				}
				else
				{
					if (current.m_Input == null || !current.m_Input.m_IsFile)
					{
						string str = MetaGenerator.EscapeWord(current.m_Path, false);
						string text2 = strNameSpace;
						if (text2.Length > 0)
						{
							text2 += ":";
						}
						text2 += str;
						MetaGenerator.WriteBodyC(sw, text2, current, strAdditionalNameSpace, level + 1L);
					}
				}
			}
		}
		public static void CreateHeader(Stream outStream, string strOutHeaderDir, MetaGenerator.Input[] inData, string strAdditionalNamespace)
		{
			if (strOutHeaderDir.Length > 0 && strOutHeaderDir[strOutHeaderDir.Length - 1] != '/' && strOutHeaderDir[strOutHeaderDir.Length - 1] != '\\')
			{
				strOutHeaderDir += "\\";
			}
			MetaGenerator.TreeNode node = MetaGenerator.CreateNode(inData);
			MetaGenerator.WriteHeader(strOutHeaderDir, "", "", node, strAdditionalNamespace);
			StreamWriter streamWriter = new StreamWriter(outStream, Encoding.UTF8);
			streamWriter.WriteLine("");
			streamWriter.WriteLine("#ifndef NN_TYPES_H_");
			streamWriter.WriteLine("#include <nn/types.h>");
			streamWriter.WriteLine("#endif /* NN_TYPES_H_ */\r\n");
			streamWriter.WriteLine("#ifdef __cplusplus");
			streamWriter.WriteLine("");
			MetaGenerator.WriteBodyCpp(streamWriter, node, strAdditionalNamespace, 0L);
			streamWriter.WriteLine("#endif /* __cplusplus */\r\n");
			streamWriter.WriteLine("");
			MetaGenerator.WriteBodyC(streamWriter, "", node, strAdditionalNamespace, 0L);
			streamWriter.Flush();
		}
	}
}
