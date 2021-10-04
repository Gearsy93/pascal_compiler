/* Осуществляет лекcический анализ, строит идентификаторы, ключевые слова, разделители, числа
 * Вход: последовательность литер
 * Выход: лексически проверенная программа, лексические ошибки
 */
#define star
#define slash

using System;
using System.Linq;
using InputOutput;


namespace LexicalAnalyzer
{

	public class Lexical
	{
		public struct Lexem
        {
			public int LinePosition;
			public int LineNumber;
			public object value;
			public string type;

			public Lexem (int pos, int num, object val, string input_type)
            {
				LineNumber = num;
				LinePosition = pos;
				value = val;
				type = input_type;
            }
        };
		public IO Reader { get; }
		//public int CurrentToken { get; private set; }

		public Lexical(IO Input_Reader)
        {
			Reader = Input_Reader;
        }

		public void PrintLexem()
        {
			Lexem value = NextSym();
			while (value.value != null)
            {
				Console.WriteLine("Token: " + value.type + "; Value: " + value.value.ToString() + "; Position: " + value.LineNumber.ToString() + "; Line: " + value.LinePosition.ToString() + "; Count: " + Reader.Count.ToString());
				value = NextSym();
			}
        }

		public Lexem NextSym()
        {
			Lexem CurrentLexem = new Lexem(Reader.Line_Number, Reader.Line_Position, null, "");
			if (Reader.Count < Reader.ProgramText.Length)
			{
				char symbol = Reader.Nextch();
				if (symbol == '\n')
                {
					Console.WriteLine("log");
                }
				if (symbol == ' ' || symbol == '\n')
                {
					symbol = Reader.Nextch();
				}
				string[] operation = { "+", "-", "*", "/", "%", "=", "<", ">", "@" };
				string[] limiter = { ",", ".", "'", "(", ")", "[", "]", "{", "}", ":", ";" };
				string[] specifier = { "^", "#", "$" };
				if (Char.IsLetter(symbol) || symbol == '_')
				{
					string st = "" + symbol;
					symbol = Reader.Nextch();
					while (Char.IsLetter(symbol) || Char.IsDigit(symbol) || symbol == '_')
					{
						if (Char.IsDigit(symbol) || symbol == '_')
						{
							CurrentLexem.type = "id";
						}
						st += symbol;
						symbol = Reader.Nextch();
					}
					Reader.Back();
					//Console.WriteLine(st);
					if (CurrentLexem.type == "")
					{
						string[] array = { "if", "do", "of", "or", "in", "to", "end", "var", "div", "and", "not", "for", "mod", "nil", "set", "then", "else", "case", "file", "goto", "type", "with", "begin", "while", "downto", "packed", "record", "repeat", "program", "function", "procedure" };
						if (array.Contains(st.ToLower()))
						{
							CurrentLexem.type = "keyword";
						}
						else
						{
							CurrentLexem.type = "id";

						}
					}
					CurrentLexem.value = st;
				}
				else if (Char.IsDigit(symbol))
				{
					string st = "" + symbol;
					symbol = Reader.Nextch();
					while (Char.IsLetter(symbol) || symbol == '.')
					{
						if (symbol == '.') CurrentLexem.type = "real";
						st += symbol;
						CurrentLexem.value = float.Parse(st, System.Globalization.CultureInfo.InvariantCulture);
					}
					if (CurrentLexem.type == "")
					{
						CurrentLexem.type = "int";
						CurrentLexem.value = Int32.Parse(st);
					}
					Reader.Back();

				}
				else if (operation.Contains("" + symbol))
				{
					CurrentLexem.type = "operation";
					CurrentLexem.value = "" + symbol;
					if (symbol == '/')
					{
						if (Reader.Nextch() == '/')
						{
							CurrentLexem.type = "comment";
							string st = "/";
							do
							{
								symbol = Reader.Nextch();
								st += symbol;

							} while (symbol != '\n');
							CurrentLexem.value = st;
						}
						else
						{
							CurrentLexem.type = "operation";
							Reader.Back();
						}
					}
					if (symbol == '<')
					{
						if (Reader.Nextch() == '>')
						{
							CurrentLexem.value = "<>";
						}
						else if (Reader.Nextch() == '=')
						{
							CurrentLexem.value = "<=";
						}
						else
						{
							Reader.Back();
						}
					}
					else if (symbol == '>')
					{
						if (Reader.Nextch() == '=')
						{
							CurrentLexem.value = ">=";
						}
						else Reader.Back();
					}
				}
				else if (symbol == ':')
				{
					CurrentLexem.type = "operation";
					CurrentLexem.value = "" + symbol;
					if (Reader.Nextch() == '=')
					{
						CurrentLexem.type = "operation";
						CurrentLexem.value = ":=";
					}
					else
					{
						Reader.Back();
					}
				}
				else if (limiter.Contains("" + symbol))
				{
					CurrentLexem.type = "limiter";
					CurrentLexem.value = "" + symbol;
					if (symbol == '(' && Reader.Nextch() == '*')
					{
						CurrentLexem.type = "comment";
						string st = "(*";
						while (st[st.Length - 2].ToString() + st[st.Length - 1].ToString() != "*)")
						{
							st += Reader.Nextch();
						}
						CurrentLexem.value = st;
					}
					else
					{
						CurrentLexem.type = "limiter";
						Reader.Back();
					}
					if (Reader.Nextch() == '.')
					{
						CurrentLexem.value = "..";
					}
					else
					{
						//Reader.Back();
					}
				}
				else if (specifier.Contains("" + symbol))
				{
					CurrentLexem.value = "" + symbol;
					CurrentLexem.type = "specifier";

				}
				else
                {
					symbol = Reader.Nextch();
				}
			}
			Console.WriteLine("cock");
			return CurrentLexem;
        }


	}
}
