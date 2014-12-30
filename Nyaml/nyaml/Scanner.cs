using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
namespace nyaml
{
	internal class Scanner : IEnumerator<TokenItem>, IDisposable, IEnumerator, IEnumerable<TokenItem>, IEnumerable
	{
		private readonly string BLOCK_BEGIN = "<BLOCK_BEGIN>";
		private readonly string BLOCK_END = "<BLOCK_END>";
		private readonly string SEQUENCE_HEADER = "-";
		private readonly string MAPPER = ":";
		private readonly string BLOCK_SCALAR_HEADER = "|";
		private List<string> m_Lines;
		private List<TokenItem> m_TokenItems;
		private List<TokenItem>.Enumerator m_TokenEnumerator;
		public TokenItem Current
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		object IEnumerator.Current
		{
			get
			{
				return this.m_TokenEnumerator.Current;
			}
		}
		private void Initialize(string[] inputs)
		{
			this.m_Lines = this.ConvertIndentToBlock(inputs);
			this.AnalysisData();
			this.m_TokenItems.Insert(0, new TokenItem(Token.token_DocumentBegin, "---", -1));
			this.m_TokenItems.Add(new TokenItem(Token.token_DocumentEnd, "...", -1));
		}
		public static Scanner GetScannerFromFile(string filename)
		{
			Scanner scanner = new Scanner();
			string[] inputs = File.ReadAllLines(filename, Encoding.GetEncoding("Shift_JIS"));
			scanner.Initialize(inputs);
			return scanner;
		}
		public static Scanner GetScannerFromText(string text)
		{
			Scanner scanner = new Scanner();
			string[] inputs = text.Split(new char[]
			{
				'\n',
				'\r'
			});
			scanner.Initialize(inputs);
			return scanner;
		}
		private Scanner()
		{
		}
		private void Test(List<string> lines)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in lines)
			{
				stringBuilder.Append(current);
				stringBuilder.Append("\n");
			}
			File.WriteAllText("test.nyaml", stringBuilder.ToString(), Encoding.GetEncoding("Shift_JIS"));
		}
		private int GetIndent(string line)
		{
			for (int i = 0; i < line.Length; i++)
			{
				if (line[i] != ' ')
				{
					return i;
				}
			}
			return line.Length;
		}
		private bool IsWhiteLine(string line)
		{
			return Regex.Match(line, "^\\s*(#.*)?\\z").Success;
		}
		private List<string> ConvertIndentToBlock(string[] inputData)
		{
			string[] array = new string[inputData.Length + 1];
			Array.Copy(inputData, array, inputData.Length);
			array[array.Length - 1] = "EOF\n";
			Stack<int> stack = new Stack<int>();
			stack.Push(-1);
			List<string> list = new List<string>();
			int num = 0;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (!this.IsWhiteLine(text))
				{
					int indent = this.GetIndent(text);
					if (indent > stack.Peek())
					{
						stack.Push(indent);
						list.Add(new string(' ', indent) + this.BLOCK_BEGIN);
					}
					if (indent < stack.Peek())
					{
						while (stack.Peek() > indent)
						{
							list.Add(new string(' ', stack.Peek()) + this.BLOCK_END);
							stack.Pop();
						}
						if (indent > stack.Peek())
						{
							Console.WriteLine("Indent error!");
							Console.WriteLine("Line: " + (num + 1));
							throw new Exception("Indent error\n");
						}
					}
				}
				list.Add(text);
				num++;
			}
			list.RemoveAt(list.Count - 1);
			list.Add(this.BLOCK_END ?? "");
			return list;
		}
		private void DumpReplacedData(List<string> data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in data)
			{
				stringBuilder.Append(current);
				stringBuilder.Append("\n");
			}
			File.WriteAllText("dump.txt", stringBuilder.ToString());
		}
		private List<string> ParseLine(string line)
		{
			List<string> list = new List<string>();
			byte[] array = new byte[Encoding.ASCII.GetByteCount(line) + 1];
			Encoding.ASCII.GetBytes(line, 0, array.Length - 1, array, 0);
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				switch (num)
				{
				case 0:
					if (array[i] != 32 && array[i] != 9)
					{
						stringBuilder.Append((char)array[i]);
						if (array[i] == 34)
						{
							num = 1;
						}
						else
						{
							if (array[i] == 35)
							{
								num = 3;
							}
							else
							{
								num = 2;
							}
						}
					}
					break;
				case 1:
					stringBuilder.Append((char)array[i]);
					if (array[i] == 34)
					{
						num = 0;
						list.Add(stringBuilder.ToString());
						stringBuilder = new StringBuilder();
					}
					break;
				case 2:
					if (array[i] == 32 || array[i] == 9 || array[i] == 0 || array[i] == 35)
					{
						list.Add(stringBuilder.ToString());
						stringBuilder = new StringBuilder();
						if (array[i] == 35)
						{
							num = 3;
						}
						else
						{
							num = 0;
						}
					}
					else
					{
						stringBuilder.Append((char)array[i]);
					}
					break;
				}
			}
			return list;
		}
		private void AnalysisData()
		{
			this.m_TokenItems = new List<TokenItem>();
			int num = 0;
			foreach (string current in this.m_Lines)
			{
				bool flag = false;
				List<string> list = this.ParseLine(current);
				foreach (string current2 in list)
				{
					if (!(current2 == "[]") && !(current2 == "{}"))
					{
						if (current2.Length > 1 && current2[current2.Length - 1] == ':')
						{
							this.m_TokenItems.Add(this.GetTokenItem(current2.Substring(0, current2.Length - 1), num, 0));
							this.m_TokenItems.Add(this.GetTokenItem(string.Concat(':'), num, 0));
						}
						else
						{
							this.m_TokenItems.Add(this.GetTokenItem(current2, num, 0));
						}
						if (current2.Equals(this.BLOCK_BEGIN) || current2.Equals(this.BLOCK_END))
						{
							flag = true;
						}
					}
				}
				if (!this.IsWhiteLine(current))
				{
					this.m_TokenItems.Add(new TokenItem(Token.token_EOL, "<EOL>", num));
				}
				if (!flag)
				{
					num++;
				}
			}
		}
		private TokenItem GetScalarToken(string word, int line, int column)
		{
			if (word.Equals(this.SEQUENCE_HEADER) || word.Equals(this.MAPPER))
			{
				return null;
			}
			string a = word.ToLower();
			if (a == "true" || a == "false")
			{
				return new TokenItem(Token.token_Bool, word, line);
			}
			if (Regex.Match(word, RegexPattern.REGEX_TOKEN_NUMBER).Success)
			{
				return new TokenItem(Token.token_Number, word, line);
			}
			if (Regex.Match(word, RegexPattern.REGEX_TOKEN_LONG_NUMBER).Success)
			{
				return new TokenItem(Token.token_LongNumber, word, line);
			}
			if (Regex.Match(word, RegexPattern.REGEX_TOKEN_TEXT).Success)
			{
				if (word.Length >= 2 && word[0] == '"' && word[word.Length - 1] == '"')
				{
					word = word.Remove(word.Length - 1, 1);
					word = word.Remove(0, 1);
				}
				return new TokenItem(Token.token_Text, word, line);
			}
			return null;
		}
		private TokenItem GetTokenItem(string word, int line, int column)
		{
			TokenItem scalarToken = this.GetScalarToken(word, line, column);
			Token token;
			if (word.Equals(this.SEQUENCE_HEADER))
			{
				token = Token.token_SequenceHeader;
			}
			else
			{
				if (word.Equals(this.BLOCK_BEGIN))
				{
					token = Token.token_BlockBegin;
				}
				else
				{
					if (word.Equals(this.BLOCK_END))
					{
						token = Token.token_BlockEnd;
					}
					else
					{
						if (word.Equals(this.MAPPER))
						{
							token = Token.token_Mapper;
						}
						else
						{
							if (word.Equals(this.BLOCK_SCALAR_HEADER))
							{
								token = Token.token_BlockScalarHeader;
							}
							else
							{
								if (scalarToken != null)
								{
									return scalarToken;
								}
								throw new Exception("Invalid token \"" + word + "\"\n");
							}
						}
					}
				}
			}
			return new TokenItem(token, word, line);
		}
		public IEnumerator<TokenItem> GetEnumerator()
		{
			this.m_TokenEnumerator = this.m_TokenItems.GetEnumerator();
			return this.m_TokenEnumerator;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			this.m_TokenEnumerator = this.m_TokenItems.GetEnumerator();
			return this.m_TokenEnumerator;
		}
		public void Dispose()
		{
			this.m_TokenEnumerator.Dispose();
		}
		public bool MoveNext()
		{
			return this.m_TokenEnumerator.MoveNext();
		}
		public void Reset()
		{
			this.m_TokenEnumerator = this.m_TokenItems.GetEnumerator();
			this.m_TokenEnumerator.MoveNext();
		}
	}
}
