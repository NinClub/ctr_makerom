using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
namespace nyaml
{
	public class Nyaml
	{
		public CollectionElement Collection
		{
			get;
			private set;
		}
		public CollectionElement this[int index]
		{
			get
			{
				return this.Collection[index];
			}
		}
		public CollectionElement this[string key]
		{
			get
			{
				return this.Collection[key];
			}
		}
		public Nyaml(Collection collection)
		{
			if (collection != null)
			{
				this.Collection = collection;
				return;
			}
			this.Collection = new Mapping();
		}
		public static string GetNotIncludedKey(CollectionElement template, CollectionElement other)
		{
			string text = null;
			if (other.GetType() == typeof(Mapping))
			{
				if (template.GetType() == typeof(ScalarNull))
				{
					return null;
				}
				if (template.GetType() != typeof(Mapping))
				{
					throw new NotImplementedException();
				}
				foreach (KeyValuePair<string, CollectionElement> current in (Mapping)other)
				{
					Mapping mapping = (Mapping)template;
					if (mapping[current.Key] == null)
					{
						string result = current.Key;
						return result;
					}
					if (current.Value.GetType() == typeof(Mapping))
					{
						string notIncludedKey = Nyaml.GetNotIncludedKey(mapping[current.Key], current.Value);
						if (notIncludedKey != null)
						{
							string result = notIncludedKey;
							return result;
						}
						if (text != null)
						{
							string result = text;
							return result;
						}
					}
				}
				return text;
			}
			return text;
		}
		public void Dump()
		{
			this.Collection.Dump(0);
		}
		public static string[] GetStringArray(CollectionElement collection)
		{
			if (collection == null)
			{
				return null;
			}
			if (collection.IsNullScalar)
			{
				return new string[0];
			}
			if (collection.GetType() != typeof(Sequence))
			{
				return null;
			}
			Sequence sequence = (Sequence)collection;
			string[] array = new string[sequence.Elements.Count];
			int num = 0;
			foreach (CollectionElement current in sequence)
			{
				if (!current.IsScalar)
				{
					return null;
				}
				array[num] = current.ToString();
				num++;
			}
			return array;
		}
		private static int StringToInt32(string value)
		{
			if (value.StartsWith("0x"))
			{
				return Convert.ToInt32(value, 16);
			}
			return Convert.ToInt32(value, 10);
		}
		private static long StringToInt64(string value)
		{
			string text = value.Trim(new char[]
			{
				'l',
				'L'
			});
			if (text.StartsWith("0x"))
			{
				return Convert.ToInt64(text, 16);
			}
			return Convert.ToInt64(text, 10);
		}
		private static string GetUserValue(string key, Dictionary<string, string> userVariables)
		{
			if (userVariables == null)
			{
				return "";
			}
			if (userVariables.ContainsKey(key))
			{
				return userVariables[key];
			}
			if (Environment.GetEnvironmentVariables().Contains(key))
			{
				return Environment.GetEnvironmentVariables()[key].ToString();
			}
			return "";
		}
		public static string PreProcess(string filename, Dictionary<string, string> userVariables)
		{
			string[] inputs = File.ReadAllLines(filename, Encoding.GetEncoding("Shift_JIS"));
			return Nyaml.PreProcessImpl(inputs, userVariables);
		}
		public static string PreProcessImpl(string[] inputs, Dictionary<string, string> userVariables)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < inputs.Length; i++)
			{
				string text = inputs[i];
				MatchCollection matchCollection = Regex.Matches(text, "\\$\\(([A-Za-z0-9_]+)\\)");
				string text2 = text;
				foreach (Match match in matchCollection)
				{
					string key = match.Groups[1].ToString();
					string userValue = Nyaml.GetUserValue(key, userVariables);
					text2 = text2.Replace(match.Groups[0].ToString(), userValue);
				}
				list.Add(text2);
			}
			return string.Join("\n", list.ToArray());
		}
		private static Nyaml LoadImpl(string fileName, Scanner scanner)
		{
			SemanticAction sa = new SemanticAction();
			Parser parser = new Parser(sa);
			bool flag = false;
			foreach (TokenItem current in scanner)
			{
				try
				{
					if (current.Token == Token.token_Bool)
					{
						flag = parser.post(current.Token, Convert.ToBoolean(current.Value));
					}
					else
					{
						if (current.Token == Token.token_Number)
						{
							flag = parser.post(current.Token, Nyaml.StringToInt32(current.Value));
						}
						else
						{
							if (current.Token == Token.token_LongNumber)
							{
								flag = parser.post(current.Token, Nyaml.StringToInt64(current.Value));
							}
							else
							{
								flag = parser.post(current.Token, current.Value);
							}
						}
					}
				}
				catch (SyntaxErrorException ex)
				{
					ex.Line = current.Line + 1;
					ex.TokenValue = current.Value;
					ex.Token = current.Token;
					ex.FileName = ((fileName != null) ? fileName : "<TextResource>");
					throw ex;
				}
				if (flag)
				{
					break;
				}
			}
			flag = parser.post(Token.token_eof, "");
			if (flag)
			{
				object obj;
				parser.accept(out obj);
				return (Nyaml)obj;
			}
			Console.WriteLine("Not accepted\n");
			return null;
		}
		public static Nyaml LoadFromFile(string fileName, Dictionary<string, string> userVariables)
		{
			string text = Nyaml.PreProcess(fileName, userVariables);
			Scanner scannerFromText = Scanner.GetScannerFromText(text);
			return Nyaml.LoadImpl(fileName, scannerFromText);
		}
		public static Nyaml LoadFromText(string text)
		{
			Scanner scannerFromText = Scanner.GetScannerFromText(text);
			return Nyaml.LoadImpl(null, scannerFromText);
		}
	}
}
