/* Осуществляет лекcический анализ, строит идентификаторы, ключевые слова, разделители, числа
 * Вход: последовательность литер
 * Выход: лексически проверенная программа, лексические ошибки
 */
#define star
#define slash

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
			public int LinePosition;
			public int LineNumber;
			public string type;


			public Lexem (int pos, int num, string input_type)
            {
				LineNumber = num;
				LinePosition = pos;
				type = input_type;
				
			}
        };

		public class Keyword : Lexem
        {
			public string value;

			public Keyword(int pos, int num, string val, string input_type) : base(pos, num, input_type)
            {
				value = val;
            }

		}

		public class Real : Lexem
		{
			public double value;

			public Real(int pos, int num, double val, string input_type) : base(pos, num, input_type)
			{
				value = val;
			}

		}

		public class Int : Lexem
		{
			public int value;

			public Int(int pos, int num, int val, string input_type) : base(pos, num, input_type)
			{
				value = val;
			}

		}

		public class Operation : Lexem
		{
			public string value;

			public Operation(int pos, int num, string val, string input_type) : base(pos, num, input_type)
			{
				value = val;
			}

		}

		public class Id : Lexem
		{
			public string value;

			public Id(int pos, int num, string val, string input_type) : base(pos, num, input_type)
			{
				value = val;
			}

		}

		public class Limiter : Lexem
		{
			public string value;

			public Limiter(int pos, int num, string val, string input_type) : base(pos, num, input_type)
			{
				value = val;
			}

		}

		public class Specifier : Lexem
		{
			public string value;

			public Specifier(int pos, int num, string val, string input_type) : base(pos, num, input_type)
			{
				value = val;
			}

		}
		public IO Reader { get; }

		public Lexical(IO Input_Reader)
        {
			Reader = Input_Reader;
        }

        //Создание списка ошибик и токенов 
        //float.Parse(st, System.Globalization.CultureInfo.InvariantCulture)

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
			string raw_value;
			Lexem CurrentLexem = NextSym(out raw_value);
            while (raw_value != null)
            {
				if (CurrentLexem.type != "unused")
                {
					Console.WriteLine("Token: " + CurrentLexem.type + "| Value: " + raw_value.ToString() + "| Position: " + CurrentLexem.LineNumber.ToString() + "| Line: " + (CurrentLexem.LinePosition + 1).ToString() + "| Count: " + Reader.Count.ToString());
				}

				CurrentLexem = NextSym(out raw_value);
			}
		}


		public Lexem NextSym(out string raw_value)
        {
			Lexem CurrentLexem = new Lexem(Reader.Line_Number, Reader.Line_Position, "");
			raw_value = null;
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
					raw_value = st;
				}
				else if (Char.IsDigit(symbol))
				{
					string st = "" + symbol;
					symbol = Reader.Nextch();
					while (Char.IsLetter(symbol) || symbol == '.')
					{
						if (symbol == '.') CurrentLexem.type = "real";
						st += symbol;
						raw_value = st;
						if (Math.Abs(int.Parse(st, System.Globalization.CultureInfo.InvariantCulture)) > 32768)
                        {
							Errors.Add(Create_error(1, Reader.Line_Number, Reader.Line_Position));
                        }
					}
					if (CurrentLexem.type == "")
					{
						CurrentLexem.type = "int";
						raw_value = st;
					}
					Reader.Back();

				}
				else if (operation.Contains("" + symbol))
				{
					CurrentLexem.type = "operation";
					raw_value = "" + symbol;
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
							st = st.Substring(0, st.Length - 2);
							raw_value = st;
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
								else if (Reader.Count == Reader.ProgramText.Length - 1)
                                {
									Errors.Add(Create_error(6, Reader.Line_Number, Reader.Line_Position));
                                }

							}
                            raw_value = st;
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
							raw_value = "<>";
						}
						else if (Reader.Nextch() == '=')
						{
							raw_value = "<=";
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
							raw_value = ">=";
						}
						else Reader.Back();
					}
				}
				else if (symbol == ':')
				{
					CurrentLexem.type = "operation";
					raw_value = "" + symbol;
					if (Reader.Nextch() == '=')
					{
						CurrentLexem.type = "operation";
						raw_value = ":=";
					}
					else
					{
						Reader.Back();
					}
				}
				else if (limiter.Contains("" + symbol))
				{
					CurrentLexem.type = "limiter";
					raw_value = "" + symbol;
					
				}
				else if (specifier.Contains("" + symbol))
				{
					raw_value = "" + symbol;
					CurrentLexem.type = "specifier";

				}
				else
                {
					CurrentLexem.type = "unused";
					raw_value = "???";
					Errors.Add(Create_error(2, Reader.Line_Number, Reader.Line_Position));
				}
				if (Reader.Nextch() == '.')
				{
					raw_value = ".";
					if (Error_st.dot_flag == true)
					{
						Errors.Add(Create_error(4, Reader.Line_Number, Reader.Line_Position));
					}
					Error_st.dot_flag = true;
				}
				else
                {
					Reader.Back();
                }
			}
			return CurrentLexem;
        }
	}
}
