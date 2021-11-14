/* Осуществляет лекcический анализ, строит идентификаторы, ключевые слова, разделители, числа
 * Вход: последовательность литер
 * Выход: лексически проверенная программа, лексические ошибки
 */

using System;
using System.Linq;
using InputOutput;
using System.Collections.Generic;

namespace LexicalAnalyzer
{

	public class Lexical
	{
		public List<string> Errors;
       
        public struct Error_struct
        {
			public bool dot_flag;

			public Error_struct(bool dot_flag_)
            {
				this.dot_flag = dot_flag_;

			}
		};
		public Error_struct Error_st;
		public Lexical()
        {
			Error_st = new Error_struct(false);
			Errors.Add(Create_error(1, 0, 0));
		}
		
		public class Lexem
        {
			public string raw_value;
			public int LinePosition;
			public int LineNumber;
			public string type;


			public Lexem (int pos, int num, string input_type, string input_value)
            {
				raw_value = input_value;
				LineNumber = num;
				LinePosition = pos;
				type = input_type;
				
			}
        };

		public struct Position_L
		{
			public int Line_Number;
			public int Line_Position;
			public int Last_Line_Number;
			public int Last_Line_Position;
			public int Count;
		}

		public Position_L Save_Position()
		{
			Position_L Backup = new Position_L();

			Backup.Count = Reader.Count;
			Backup.Line_Number = Reader.Line_Number;
			Backup.Line_Position = Reader.Line_Number;
			Backup.Last_Line_Number = Reader.Last_Line_Number;
			Backup.Last_Line_Position = Reader.Last_Line_Position;

			return Backup;
		}

		public void Set_Position(Position_L Backup)
		{
			Reader.Count = Backup.Count;
			Reader.Line_Number = Backup.Line_Number;
			Reader.Line_Position = Backup.Line_Position;
			Reader.Last_Line_Number = Backup.Last_Line_Number;
			Reader.Last_Line_Position = Backup.Last_Line_Position;
		}


		//Пока не используются, в лексемах хранятся сырые строковые значения
		public class Keyword : Lexem
        {
			public string value;

			public Keyword(int pos, int num, string val, string input_type, string input_value) : base(pos, num, input_type, input_value)
            {
				value = val;
            }

		}

		public class Real : Lexem
		{
			public double value;

			public Real(int pos, int num, double val, string input_type, string input_value) : base(pos, num, input_type, input_value)
			{
				value = val;
			}

		}

		public class Int : Lexem
		{
			public int value;

			public Int(int pos, int num, int val, string input_type, string input_value) : base(pos, num, input_type, input_value)
			{
				value = val;
			}

		}

		public class Operation : Lexem
		{
			public string value;

			public Operation(int pos, int num, string val, string input_type, string input_value) : base(pos, num, input_type, input_value)
			{
				value = val;
			}

		}

		public class Id : Lexem
		{
			public string value;

			public Id(int pos, int num, string val, string input_type, string input_value) : base(pos, num, input_type, input_value)
			{
				value = val;
			}

		}

		public class Limiter : Lexem
		{
			public string value;

			public Limiter(int pos, int num, string val, string input_type, string input_value) : base(pos, num, input_type, input_value)
			{
				value = val;
			}

		}

		public class Specifier : Lexem
		{
			public string value;

			public Specifier(int pos, int num, string val, string input_type, string input_value) : base(pos, num, input_type, input_value)
			{
				value = val;
			}

		}
		public IO Reader { get; }

		public Lexical(IO Input_Reader)
        {
			Reader = Input_Reader;
        }

        public static string Create_error(int error_num, int line, int position)
        {
			string error_title = "";
            switch (error_num)
            {
				//Размер целочисленной константы +
				case 1:
                    {
						error_title = "Размер целочисленной переменной превышает допустимый диапазон";
						break;
                    }
				//Недопустимые символы +
				case 2:
					{
						error_title = "Недопустимые символы";
						break;
					}
				//Неправильное количество точек +
				case 5:
					{
						error_title = "Неправильное количество точек";
						break;
					}
					//Незаконченный комментарий
				case 6:
                    {
						error_title = "Обнаружен незаконченный комментарий";
						break;
                    }
			}
			return "String: " + line + " | Position: " + position + " | Error number: " + error_num + ". " + error_title;
        }


        public void PrintLexem()
        {
			Lexem CurrentLexem = NextSym();
            while (CurrentLexem.raw_value != null)
            {
				if (CurrentLexem.type != "unused" && CurrentLexem.type != "comment")
                {
					Console.WriteLine("Token: " + CurrentLexem.type + "| Value: " + CurrentLexem.raw_value.ToString() + "| Position: " + CurrentLexem.LineNumber.ToString() + "| Line: " + (CurrentLexem.LinePosition + 1).ToString() + "| Count: " + Reader.Count.ToString());
				}
				CurrentLexem = NextSym();
			}
		}


		public Lexem NextSym()
        {
			Lexem CurrentLexem = new Lexem(Reader.Line_Number, Reader.Line_Position, "", null);
			if (Reader.Count < Reader.ProgramText.Length)
			{
				char symbol = Reader.Nextch();
				
				if (symbol == ' ' || symbol == '\n' || symbol == '\t' || symbol == '\r')
                {
					symbol = Reader.Nextch();
				}
				string[] operation = { "+", "-", "*", "/", "%", "=", "<", ">", "@" };
				string[] limiter = { ",", ".", "'", "(", ")", "[", "]", "{", "}", ":", ";" };
				string[] specifier = { "^", "#", "$" };
				string[] keywords = { "if", "do", "of", "or", "in", "to", "end", "var", "div", "and", "not", "for", "mod", "nil", "set", "then", "else", "case", "file", "goto", "type", "with", "begin", "while", "downto", "packed", "record", "repeat", "program", "function", "procedure" };


				if (Char.IsLetter(symbol))
				{

					char Symbol_Backup = symbol;
					Position_L Backup = Save_Position();

					string raw = "";
					bool found = false;

					//9 - макс длина ключевого слова
					while (raw.Length <= 9 && !found)
                    {
						raw += symbol;

						foreach (string keyword in keywords)
                        {
							if(raw.ToLower() == keyword)
                            {
								found = true;
								break;
                            }
                        }
						if (!found)
						{
							symbol = Reader.Nextch();
						}
						else break;
					}

					if(!found)
                    {
						Set_Position(Backup);
						symbol = Symbol_Backup;
						CurrentLexem.type = "letter";
						CurrentLexem.raw_value = "" + symbol;
					}
					else
                    {
						CurrentLexem.type = "keyword";
						CurrentLexem.raw_value = raw;
					}

					
				}
				else if (Char.IsDigit(symbol))
				{

					CurrentLexem.type = "digit";
					CurrentLexem.raw_value = "" + symbol;

				}
				else if (operation.Contains("" + symbol))
				{
					CurrentLexem.type = "operation";
					CurrentLexem.raw_value = "" + symbol;
					if (symbol == '/')
					{
						if (Reader.Nextch() == '/')
						{
							CurrentLexem.type = "comment";
							string st = "//";
							do
							{
								symbol = Reader.Nextch();
								st += symbol;

							} while (symbol != '\n' && symbol != '\r' && symbol != '\t');
							st = st.Substring(0, st.Length - 1);
							CurrentLexem.raw_value = st;
							return CurrentLexem;
						}
						else
						{
							CurrentLexem.type = "operation";
							Reader.Back();
						}

						if (Reader.Nextch() == '*')
						{
							CurrentLexem.type = "comment";
							string st = "/*";
							while (st[st.Length - 2].ToString() + st[st.Length - 1].ToString() != "*/")
							{
								char a = Reader.Nextch();

								if (a != '\n' && a != '\r' && a != '\t') st += a;
								if (Reader.Count == Reader.ProgramText.Length)
								{
									Errors.Add(Create_error(6, Reader.Line_Number, Reader.Line_Position));
									break;
								}

							}
							CurrentLexem.raw_value = st;
						}
						else
						{
							CurrentLexem.type = "limiter";
							Reader.Back();
						}
					}
					if (symbol == '<')
					{
						if (Reader.Nextch() == '>')
						{
							CurrentLexem.raw_value = "<>";
						}
						else if (Reader.Nextch() == '=')
						{
							CurrentLexem.raw_value = "<=";
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
							CurrentLexem.raw_value = ">=";
						}
						else Reader.Back();
					}
				}
				else if (symbol == ':')
				{
					CurrentLexem.type = "operation";
					CurrentLexem.raw_value = "" + symbol;
					if (Reader.Nextch() == '=')
					{
						CurrentLexem.type = "operation";
						CurrentLexem.raw_value = ":=";
					}
					else
					{
						Reader.Back();
					}
				}
				else if (limiter.Contains("" + symbol))
				{
					CurrentLexem.type = "limiter";
					CurrentLexem.raw_value = "" + symbol;

				}
				else if (specifier.Contains("" + symbol))
				{
					CurrentLexem.raw_value = "" + symbol;
					CurrentLexem.type = "specifier";

				}
				else if (symbol == '_')
                {
					CurrentLexem.raw_value = "" + symbol;
					CurrentLexem.type = "_";
				}
                else 
                {
					if (symbol == '\0' || symbol == '\t' || symbol == '\r' || symbol == '\n' || symbol == '\v' || symbol == '\f' || symbol == '\b' || symbol == ' ')
                    {
						CurrentLexem.type = "unused";
						CurrentLexem.raw_value = "???";
					}
                    else
                    {
						Console.WriteLine(symbol);
						Errors.Add(Create_error(2, Reader.Line_Number, Reader.Line_Position));
					}
				}
			}
            else
            {
				Console.WriteLine("Неожиданный конец файла");
				System.Environment.Exit(1);
			}
			return CurrentLexem;
        }
	}
}
