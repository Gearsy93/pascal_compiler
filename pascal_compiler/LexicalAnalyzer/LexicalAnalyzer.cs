/* Осуществляет лекcический анализ, строит идентификаторы, ключевые слова, разделители, числа
 * Вход: последовательность литер
 * Выход: лексически проверенная программа, лексические ошибки
 */


/*
 * 
*/

using System;
using System.Linq;
using InputOutput;

namespace LexicalAnalyzer
{

	public class Lexical
	{
		public bool is_end = false;

		public class Lexem
        {
			public string value;
			public int LinePosition;
			public int LineNumber;
			public string type;


			public Lexem (int pos, int num, string input_value)
            {
				LineNumber = num;
				LinePosition = pos;
				value = input_value;
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


		public class Keyword : Lexem
        {
			new public string value;

			public Keyword(int pos, int num, string input_value) : base(pos, num, input_value)
            {
				value = input_value;
            }

		}

		public class Simple_Type : Lexem
		{
			new public string value;

			public Simple_Type(int pos, int num, string input_value) : base(pos, num, input_value)
			{
				value = input_value;
			}

		}

		public class Real : Lexem
		{
			new public double value;

			public Real(int pos, int num, string input_value) : base(pos, num, input_value)
			{
				value = double.Parse(input_value);
			}

		}

		public class Int : Lexem
		{
			new public int value;

			public Int(int pos, int num, string input_value) : base(pos, num, input_value)
			{
				value = Int32.Parse(input_value);
			}

		}

		public class Operation : Lexem
		{
			new public string value;

			public Operation(int pos, int num, string input_value) : base(pos, num, input_value)
			{
				value = input_value;
			}

		}

		public class Id : Lexem
		{
			new public string value;

			public Id(int pos, int num, string input_value) : base(pos, num, input_value)
			{
				value = input_value;
			}

		}


		public class Limiter : Lexem
		{
			new public string value;

			public Limiter(int pos, int num, string input_value) : base(pos, num, input_value)
			{
				value = input_value;
			}

		}
		public class Specifier : Lexem
		{
			new public string value;

			public Specifier(int pos, int num, string input_value) : base(pos, num, input_value)
			{
				value = input_value;
			}

		}

        public class String : Lexem 
		{
			new public string value;

			public String(int pos, int num, string input_value) : base(pos, num, input_value)
			{
				value = input_value;
			}
		}

		public class Char : Lexem
		{
			new public char value;

			public Char(int pos, int num, string input_value) : base(pos, num, input_value)
			{
				value = input_value[0];
			}
		}

		public IO Reader { get; }

		public Lexical(IO Input_Reader)
        {
			Reader = Input_Reader;
        }

		public static string Get_Error(int code)
		{
			string Error_Text = "";
			switch (code)
			{
				case 1: Error_Text = "Ошибка в простом типе"; break;
				case 2: Error_Text = "Должно идти имя"; break;
				case 3: Error_Text = "Должно быть служебное слово PROGRAM"; break;
				case 4: Error_Text = "Должен идти символ ‘)’"; break;
				case 5: Error_Text = "Должен идти символ ‘:’"; break;
				case 6: Error_Text = "Запрещенный символ"; break;
				case 7: Error_Text = "Ошибка в списке параметров"; break;
				case 8: Error_Text = "Должно идти OF"; break;
				case 9: Error_Text = "Должен идти символ ‘(‘"; break;
				case 10: Error_Text = "Ошибка в типе"; break;
				case 11: Error_Text = "Должен идти символ ‘[‘"; break;
				case 12: Error_Text = "Должен идти символ ‘]’"; break;
				case 13: Error_Text = "Должно идти слово END"; break;
				case 14: Error_Text = "Должен идти символ ‘;’"; break;
				case 15: Error_Text = "Должно идти целое"; break;
				case 16: Error_Text = "Должен идти символ ‘=’"; break;
				case 17: Error_Text = "Должно идти слово BEGIN"; break;
				case 18: Error_Text = "Ошибка в разделе описаний"; break;
				case 19: Error_Text = "Ошибка в списке полей"; break;
				case 20: Error_Text = "Должен идти символ ‘,’"; break;
				case 50: Error_Text = "Ошибка в константе"; break;
				case 51: Error_Text = "Должен идти символ ‘:=’"; break;
				case 52: Error_Text = "Должно идти слово THEN"; break;
				case 53: Error_Text = "Должно идти слово UNTIL"; break;
				case 54: Error_Text = "Должно идти слово DO"; break;
				case 55: Error_Text = "Должно идти слово TO или DOWNTO"; break;
				case 56: Error_Text = "Должно идти слово IF"; break;
				case 58: Error_Text = "Должно идти слово TO или DOWNTO"; break;
				case 61: Error_Text = "Должен идти символ ‘.’"; break;
				case 74: Error_Text = "Должен идти символ ‘..’"; break;
				case 75: Error_Text = "Ошибка в символьной константе"; break;
				case 76: Error_Text = "Слишком длинная строковая константа"; break;
				case 86: Error_Text = "Комментарий не закрыт"; break;
				case 100: Error_Text = "Использование имени не соответствует описанию"; break;
				case 101: Error_Text = "Имя описано повторно"; break;
				case 102: Error_Text = "Нижняя граница превосходит верхнюю"; break;
				case 104: Error_Text = "Имя не описано"; break;
				case 105: Error_Text = "Недопустимое рекурсивное определение"; break;
				case 108: Error_Text = "Файл здесь использовать нельзя"; break;
				case 109: Error_Text = "Тип не должен быть REAL"; break;
				case 111: Error_Text = "Несовместимость с типом дискриминанта"; break;
				case 112: Error_Text = "Недопустимый ограниченный тип"; break;
				case 114: Error_Text = "Тип основания не должен быть REAL или INTEGER"; break;
				case 115: Error_Text = "Файл должен быть текстовым"; break;
				case 116: Error_Text = "Ошибка в типе параметра стандартной процедуры"; break;
				case 117: Error_Text = "Неподходящее опережающее описание"; break;
				case 118: Error_Text = "Недопустимый тип пpизнака ваpиантной части записи"; break;
				case 119: Error_Text = "Опережающее описание: повторение списка параметров не допускается"; break;
				case 120: Error_Text = "Тип результата функции должен быть скалярным, ссылочным или ограниченным"; break;
				case 121: Error_Text = "Параметр-значение не может быть файлом"; break;
				case 122: Error_Text = "Опережающее описание функции: повторять тип результата нельзя"; break;
				case 123: Error_Text = "В описании функции пропущен тип результата"; break;
				case 124: Error_Text = "F-формат только для REAL"; break;
				case 125: Error_Text = "Ошибка в типе параметра стандартной функции"; break;
				case 126: Error_Text = "Число параметров не согласуется с описанием"; break;
				case 127: Error_Text = "Недопустимая подстановка параметров"; break;
				case 128: Error_Text = "Тип результата функции не соответствует описанию"; break;
				case 130: Error_Text = "Выражение не относится к множественному типу"; break;
				case 131: Error_Text = "Элементы множества не должны выходить из диапазона 0 .. 255"; break;
				case 135: Error_Text = "Тип операнда должен быть BOOLEAN"; break;
				case 137: Error_Text = "Недопустимые типы элементов множества"; break;
				case 138: Error_Text = "Переменная не есть массив"; break;
				case 139: Error_Text = "Тип индекса не соответствует описанию"; break;
				case 140: Error_Text = "Переменная не есть запись"; break;
				case 141: Error_Text = "Переменная должна быть файлом или ссылкой"; break;
				case 142: Error_Text = "Недопустимая подстановка параметров"; break;
				case 143: Error_Text = "Недопустимый тип параметра цикла"; break;
				case 144: Error_Text = "Недопустимый тип выражения"; break;
				case 145: Error_Text = "Конфликт типов"; break;
				case 147: Error_Text = "Тип метки не совпадает с типом выбирающего выражения"; break;
				case 149: Error_Text = "Тип индекса не может быть REAL или INTEGER"; break;
				case 152: Error_Text = "В этой записи нет такого поля"; break;
				case 156: Error_Text = "Метка варианта определяется несколько раз"; break;
				case 165: Error_Text = "Метка определяется несколько раз"; break;
				case 166: Error_Text = "Метка описывается несколько раз"; break;
				case 167: Error_Text = "Неописанная метка"; break;
				case 168: Error_Text = "Неопределенная метка"; break;
				case 169: Error_Text = "Ошибка в основании множества (в базовом типе)"; break;
				case 170: Error_Text = "Тип не может быть упакован"; break;
				case 177: Error_Text = "Здесь не допускается присваивание имени функции"; break;
				case 182: Error_Text = "Типы не совместны"; break;
				case 183: Error_Text = "Запрещенная в данном контексте операция"; break;
				case 184: Error_Text = "Элемент этого типа не может иметь знак"; break;
				case 186: Error_Text = "Несоответствие типов для операции отношения"; break;
				case 189: Error_Text = "Конфликт типов параметров"; break;
				case 190: Error_Text = "Повторное опережающее описание"; break;
				case 191: Error_Text = "Ошибка в конструкторе множества"; break;
				case 193: Error_Text = "Лишний индекс для доступа к элементу массива"; break;
				case 194: Error_Text = "Указано слишком мало индексов для доступа к элементу массива"; break;
				case 195: Error_Text = "Выбирающая константа вне границ описанного диапазона"; break;
				case 196: Error_Text = "Недопустимый тип выбирающей константы"; break;
				case 197: Error_Text = "Параметры процедуры (функции) должны быть параметрами-значениями"; break;
				case 198: Error_Text = "Несоответствие количества параметров параметра-процедуры (функции)"; break;
				case 199: Error_Text = "Несоответствие типов параметров параметра-процедуры(функции)"; break;
				case 200: Error_Text = "Тип парамера-функции не соответствует описанию"; break;
				case 201: Error_Text = "Ошибка в вещественной константе: должна идти цифра"; break;
				case 203: Error_Text = "Целая константа превышает предел"; break;
				case 204: Error_Text = "Деление на нуль"; break;
				case 206: Error_Text = "Слишком маленькая вещественная константа"; break;
				case 207: Error_Text = "Слишком большая вещественная константа"; break;
				case 208: Error_Text = "Недопустимые типы операндов операции IN"; break;
				case 209: Error_Text = "Вторым операндом IN должно быть множество"; break;
				case 210: Error_Text = "Операнды AND, NOT, OR должны быть булевыми"; break;
				case 212: Error_Text = "Недопустимые типы операндов операции + или —"; break;
				case 213: Error_Text = "Операнды DIV и MOD должны быть целыми"; break;
				case 214: Error_Text = "Недопустимые типы операндов операции *"; break;
				case 215: Error_Text = "Недопустимые типы операндов операции /"; break;
				case 216: Error_Text = "Первым операндом IN должно быть выражение базового типа множества"; break;
				case 305: Error_Text = "Индексное значение выходит за границы"; break;
				case 306: Error_Text = "Присваиваемое значение выходит за границы"; break;
				case 307: Error_Text = "Выражение для элемента множества выходит за пределы"; break;
				case 308: Error_Text = "Выражение выходит за допустимые пределы"; break;
			}
			return Error_Text;
		}

		public void Raise_Error(int code)
		{
			//Добавить форматирование
			Console.WriteLine("Col: " + Reader.Line_Position + "Ln: " + Reader.Line_Number + " Description: " + Lexical.Get_Error(code));
			System.Environment.Exit(1);
		}

		public void PrintLexem()
        {
			Lexem CurrentLexem = NextSym();
			
		    while(!is_end)
            {
				if (CurrentLexem is Simple_Type) CurrentLexem = (Simple_Type)CurrentLexem;
				else if (CurrentLexem is Real) CurrentLexem = (Real)CurrentLexem;
				else if (CurrentLexem is Int) CurrentLexem = (Int)CurrentLexem;
				else if (CurrentLexem is Operation) CurrentLexem = (Operation)CurrentLexem;
				else if (CurrentLexem is Id) CurrentLexem = (Id)CurrentLexem;
				else if (CurrentLexem is Limiter) CurrentLexem = (Limiter)CurrentLexem;
				else if (CurrentLexem is Specifier) CurrentLexem = (Specifier)CurrentLexem;
				else if (CurrentLexem is String) CurrentLexem = (String)CurrentLexem;
				else if (CurrentLexem is Char) CurrentLexem = (Char)CurrentLexem;
				string raw_token_type = CurrentLexem.GetType().ToString();
				string token_type = raw_token_type.Substring(24);
				Console.WriteLine("Token: " + token_type + "| Value: " + CurrentLexem.value.ToString() + "| Position: " + CurrentLexem.LineNumber.ToString() + "| Line: " + (CurrentLexem.LinePosition + 1).ToString() + "| Count: " + Reader.Count.ToString()); ;
				if (Reader.Count < Reader.ProgramText.Length)
				{
					CurrentLexem = NextSym();
				}
				else is_end = true;
			}
		}

		public bool Is_Void(char symbol)
        {
			if (symbol == ' ' || symbol == '\n' || symbol == '\t' || symbol == '\r')
			{
				return true;
			}
			else return false;
		}


		public Lexem NextSym()
        {
			Lexem CurrentLexem = new Lexem(Reader.Line_Number, Reader.Line_Position, "");
			bool void_lexem = false;
			if (Reader.Count < Reader.ProgramText.Length)
			{
				char symbol = Reader.Nextch();
				
				while(Is_Void(symbol))
                {
					symbol = Reader.Nextch();
				}
				string[] operation = { "+", "-", "*", "/", "%", "=", "<", ">", "@" };
				string[] limiter = { ",", ".", "'", "(", ")", "[", "]", "{", "}", ":", ";" };
				string[] specifier = { "^", "#", "$" };
				string[] keywords = { "if", "do", "of", "or", "in", "to", "end", "var", "div", "and", "not", "for", "mod", "nil", "set", "then", "else", "case", "file", "goto", "type", "with", "begin", "while", "downto", "packed", "record", "repeat", "program", "function", "procedure" };
				string[] simple_type = { "integer", "shortint", "longint", "byte", "word", "real", "single", "double", "exntended", "char", "string", "boolean" };


				if(System.Char.IsDigit(symbol))
				{
					string raw = "" + symbol;
					symbol = Reader.Nextch();
					while (!Is_Void(symbol) && System.Char.IsDigit(symbol))
                    {
						raw += symbol;
						symbol = Reader.Nextch();
                    }

					//Типа закончили: либо не цифра, либо пустой

					//Фиксируем плавающее
					if (symbol == '.')
					{
						raw += symbol;
						symbol = Reader.Nextch();

						if (System.Char.IsDigit(symbol))
						{
							raw += symbol;
							symbol = Reader.Nextch();
							while (System.Char.IsDigit(symbol))
							{
								raw += symbol;
								symbol = Reader.Nextch();
							}

							if (symbol == 'e' || symbol == 'E')
							{
								raw += symbol;
								symbol = Reader.Nextch();
								if (symbol == '+' || symbol == '-')
								{
									
									raw += symbol;
									symbol = Reader.Nextch();
								}
								if (System.Char.IsDigit(symbol))
								{
									raw += symbol;
									symbol = Reader.Nextch();
									while (System.Char.IsDigit(symbol))
									{
										raw += symbol;
										symbol = Reader.Nextch();
									}

									//Формируем вещественную
									//
									CurrentLexem = new Real(Reader.Line_Number, Reader.Line_Position, raw);
									Reader.Back();

								}
								else
								{
									//Бросаем ошибку - ожидались цифры
									Raise_Error(50);
								}
							}
							else
							{
								CurrentLexem = new Int(Reader.Line_Number, Reader.Line_Position, raw);
								Reader.Back();
							}
						}
						else
						{
							//Бросаем ошибку - ожидались цифры
							Raise_Error(50);
						}
					}
					else if (symbol == 'e' || symbol == 'E')
					{
						raw += symbol;
						symbol = Reader.Nextch();
						if (symbol == '+' || symbol == '-')
						{
							raw += symbol;
							symbol = Reader.Nextch();
						}
						if (System.Char.IsDigit(symbol))
						{
							raw += symbol;
							symbol = Reader.Nextch();
							while (System.Char.IsDigit(symbol))
							{
								raw += symbol;
								symbol = Reader.Nextch();
							}

							//Формируем вещественную
							CurrentLexem = new Real(Reader.Line_Number, Reader.Line_Position, raw);
							Reader.Back();

						}
						else
						{
							//Бросаем ошибку - ожидались цифры
							Raise_Error(50);
						}
					}
					else
					{
						//Левый символ, формируем целочисленную
						CurrentLexem = new Real(Reader.Line_Number, Reader.Line_Position, raw);
						Reader.Back();
					}
				}


				if (System.Char.IsLetter(symbol))
                {
					string raw = "" + symbol;
					bool found_key = false;
					bool found_simple_type = false;
					//symbol = Reader.Nextch();
					
					string lowered = raw.ToLower();
					//9 - макс длина ключевого слова
					while (raw.Length <= 9 && !found_key && !found_simple_type)
                    {
						
						foreach (string keyword in keywords)
                        {
							if (lowered == keyword)
                            {
								if(!(System.Char.IsLetter(Reader.Nextch())))
                                {
									found_key = true;
									break;
								}
                                else
                                {
									Reader.Back();
								}
                            }
							
						}
						foreach (string type in simple_type)
						{
							if (lowered == type)
							{
								found_simple_type = true;
								break;
							}
						}
						if (!found_key && !found_simple_type)
                        {
							symbol = Reader.Nextch();
							//Проверяем, что перед нами буква
							if (System.Char.IsLetter(symbol))
                            {
								raw += symbol;
								lowered = raw.ToLower();
							}
                            else
                            {
								//Иначе это уже не ключевое слово
								while (System.Char.IsLetter(symbol) || System.Char.IsDigit(symbol) || symbol == '_')
                                {
									raw += symbol;
									lowered = raw.ToLower();
									symbol = Reader.Nextch();
								}

								CurrentLexem = new Id(Reader.Line_Number, Reader.Line_Position, lowered);
								Reader.Back();
								break;
                            }
						}
                        else if (found_key)
                        {
							CurrentLexem = new Keyword(Reader.Line_Number, Reader.Line_Position, lowered);
							Reader.Back();
							break;
						}
						else if (found_simple_type)
                        {
							CurrentLexem = new Simple_Type(Reader.Line_Number, Reader.Line_Position, lowered);
							break;
						}
                    }
				}
			
				else if (operation.Contains("" + symbol))
				{
					CurrentLexem = new Operation(Reader.Line_Number, Reader.Line_Position, "" + symbol);
					if (symbol == '/')
					{
						string st = "/";
						symbol = Reader.Nextch();
						if (symbol == '/')
						{
							st = "//";
							do
							{
								symbol = Reader.Nextch();
								st += symbol;

							} while (symbol != '\n' && symbol != '\r' && symbol != '\t');
							st = st.Substring(0, st.Length - 1);
							void_lexem = true;
						}
						else if (symbol == '*')
						{
							CurrentLexem.type = "comment";
							void_lexem = true;
							st = "/*";
							while (st[st.Length - 2].ToString() + st[st.Length - 1].ToString() != "*/")
							{
								char a = Reader.Nextch();

								if (a != '\n' && a != '\r' && a != '\t') st += a;
								if (Reader.Count == Reader.ProgramText.Length)
								{
									Raise_Error(86);
									break;
								}

							}
							void_lexem = true;
						}
						else
						{
							//Все же операция деления
							Reader.Back();
						}

					}
					else if (symbol == '<')
					{
						symbol = Reader.Nextch();
						if (symbol == '>')
						{
							CurrentLexem = new Operation(Reader.Line_Number, Reader.Line_Position, "<>");
						}
						else if (symbol == '=')
						{
							CurrentLexem = new Operation(Reader.Line_Number, Reader.Line_Position, "<=");
						}
						else
						{
							Reader.Back();
						}
					}
					else if (symbol == '>')
					{
						symbol = Reader.Nextch();
						if (symbol == '=')
						{
							CurrentLexem = new Operation(Reader.Line_Number, Reader.Line_Position, ">=");
						}
						else Reader.Back();
					}
				}
				else if (symbol == ':')
				{
					CurrentLexem = new Limiter(Reader.Line_Number, Reader.Line_Position, ":");
					symbol = Reader.Nextch();
					if (symbol == '=')
					{
						CurrentLexem = new Operation(Reader.Line_Number, Reader.Line_Position, ":=");
					}
					else
					{
						Reader.Back();
					}
				}
				else if (limiter.Contains("" + symbol))
				{
					int count = 0;
					if (symbol == '\'')
                    {
						string raw = "" + '\'';
						if (Reader.Count < Reader.ProgramText.Length)
                        {
							symbol = Reader.Nextch();
							count++;
							while (symbol != '\'')
                            {
								raw += symbol;
								if (Reader.Count < Reader.ProgramText.Length)
                                {
									symbol = Reader.Nextch();
									count++;
                                }
                                else
                                {
									// не закрыли строковую константу
									Raise_Error(50);
								}
                            }
							raw += '\'';
							if (count == 2)
                            {
								CurrentLexem = new Char(Reader.Line_Number, Reader.Line_Position, raw);
							}
                            else
                            {
								CurrentLexem = new String(Reader.Line_Number, Reader.Line_Position, raw);
							}
						}
                        else
                        {
							// не закрыли строковую константу
							Raise_Error(50);
                        }
                    }
                    else
                    {
						CurrentLexem = new Limiter(Reader.Line_Number, Reader.Line_Position, "" + symbol);
					}
					
				}
				else if (specifier.Contains("" + symbol))
				{
					CurrentLexem = new Specifier(Reader.Line_Number, Reader.Line_Position, "" + symbol);
				}
				else if (symbol == '_')
                {
					//Идентификатор
					string raw = "" + symbol;
					symbol = Reader.Nextch();
					while (System.Char.IsLetter(symbol) || System.Char.IsDigit(symbol) || symbol == '_')
					{
						raw += symbol;
						symbol = Reader.Nextch();
					}

					CurrentLexem = new Id(Reader.Line_Number, Reader.Line_Position, raw);
					Reader.Back();
				}
                else 
                {
					if (symbol == '\0' || symbol == '\t' || symbol == '\r' || symbol == '\n' || symbol == '\v' || symbol == '\f' || symbol == '\b' || symbol == ' ')
                    {
						void_lexem = true;
					}
                    else
                    {
						//Левый символ
						void_lexem = true;
						Raise_Error(75);
					}
				}
			}
            else
            {
				Console.WriteLine("Неожиданный конец файла");
				System.Environment.Exit(1);
			}
			//Комментарии пропускаем, вернуть надо следующую лексему
			if (void_lexem && Reader.Count < Reader.ProgramText.Length)
            {
				CurrentLexem = NextSym();
            }
			return CurrentLexem;
        }
	}
}
