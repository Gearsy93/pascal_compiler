/* Проверяет, удовлетворяет ли программа формальным правилам
* Вход: последовательность символов
 * Выход: синтаксически проверенная программа, синтаксические ошибки
 * Главная задача - найти все ошибки, не более
 */

/* Структура выражение: 
 * <S> ::= L
 * Вся программа закидывается в начальное выражение программа, затем последовательно вызываются ожидаемые последоывательности лексем. Если ожидаемые лексемы есть, идем дальше, иначе кидаем ошибку
*/

//Реальная проблема, которую решу чуть позже:
//В самом начале при распознавании последовательности нужно учесть, что, если 1 символ не подошел, это не ошибка, а нужно другую чекать
//Также нужно понимать где кидать ошибки, а где просто вернуться в false

using System;
using InputOutput;
using LexicalAnalyzer;
using System.Linq;

namespace SyntaxAnalyzer
{
	public class Syntaxic //toxic
	{
		//Всем привет с вами Гена Букин и сегодня мы будем разбирать по полочкам выражения
		public struct Position
		{
			public int Line_Number;
			public int Line_Position;
			public int Last_Line_Number;
			public int Last_Line_Position;
			public int Count;
			public Lexical.Lexem Current_Lexem;
		}

		public string[] keywords = { "if", "do", "of", "or", "in", "to", "end", "var", "div", "and", "not", "for", "mod", "nil", "set", "then", "else", "case", "file", "goto", "type", "with", "begin", "while", "downto", "packed", "record", "repeat", "program", "function", "procedure" };

		private string Get_Error(int code)
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

		////////////////ИНИЦИАЛИЗАЦИЯ///////////////////
		public Lexical Lexical_Analyzer;

		public string got;

		public IO Reader { get; }
		public Lexical.Lexem Current_Lexem;
		public Syntaxic(Lexical Input_Lexical_Analyzer, IO Reader_Input, Lexical.Lexem Current_Lexem_Input)
		{
			Lexical_Analyzer = Input_Lexical_Analyzer;
			Reader = Reader_Input;
			Current_Lexem = Current_Lexem_Input;
		}

		//////////////////ОШИБКИ///////////////////////
		public void Raise_Error(int code)
		{
			//Добавить форматирование
			Console.WriteLine("Col: " + Reader.Line_Position + "Ln: " + Reader.Line_Number + "Description: " + Get_Error(code));
			System.Environment.Exit(1);
		}

		public void Unhandled_Error(string expected)
		{
			got = " Получено " + Current_Lexem.raw_value;
			if (Current_Lexem.raw_value == "") got = " Встречен конец файла";
			Console.WriteLine("Col: " + Reader.Line_Position + "Ln: " + Reader.Line_Number + got + ", а ожидалось " + expected);
			System.Environment.Exit(1);
		}

		public Position Save_Position()
		{
			Position Backup = new Position();

			Backup.Count = Reader.Count;
			Backup.Line_Number = Reader.Line_Number;
			Backup.Line_Position = Reader.Line_Number;
			Backup.Last_Line_Number = Reader.Last_Line_Number;
			Backup.Last_Line_Position = Reader.Last_Line_Position;
			Backup.Current_Lexem = Current_Lexem;

			return Backup;
		}

		public void Set_Position(Position Backup)
		{
			Reader.Count = Backup.Count;
			Reader.Line_Number = Backup.Line_Number;
			Reader.Line_Position = Backup.Line_Position;
			Reader.Last_Line_Number = Backup.Last_Line_Number;
			Reader.Last_Line_Position = Backup.Last_Line_Position;
			Current_Lexem = Backup.Current_Lexem;
		}

		//////////////////ПОЛУЧИТЬ СЛЕДУЮЩУЮ ЛЕКСЕМУ///////////////////////
		public void NextSym()
		{
			Current_Lexem = Lexical_Analyzer.NextSym();
		}

		//Проверяет совпадение полученной лекесы с текущей
		public bool Accept_Raw(string raw_value)
		{
			if (Current_Lexem.raw_value.ToLower() == raw_value)
			{
				NextSym();
				return true;
			}
			else return false;
		}

		//Проверяет совпадение типа полученной лекесы с типом текущей
		public bool Accept_Type(string type)
		{
			if (Current_Lexem.type == type)
			{
				NextSym();
				return true;
			}
			else return false;
		}

		//Проверяет, содержится ли текущая лексема в заданном массиве
		public bool Accept_Raw(string[] types)
		{
			if (types.Contains(Current_Lexem.raw_value.ToLower()))
			{
				NextSym();
				return true;
			}
			else return false;
		}


		//////////////////БНФ///////////////////////

		//////////////////ОТНОСИТЕЛЬНО УВЕРЕН/////////////
		public void Accept_Program()
		{
			NextSym();
			if (!Accept_Raw("Program")) Raise_Error(3);
			if (!Accept_Type("id")) Raise_Error(2);
			if (!Accept_Raw(";")) Raise_Error(14);

			//Понятия не имею откуда после имена программы идет список файлов в скобках, ни одного примера не нашел

			Accept_Block();

			if (!Accept_Raw(".")) Raise_Error(61);
		}

		//Типа сначала описание переменных (или нет), затем begin блок end
		public void Accept_Block()
		{
			Accept_Description_Chapter();
			if (!Accept_Operators_Chapter()) Unhandled_Error("раздел описаний");
		}

		public void Accept_Description_Chapter()
		{
			Accept_Tags_Chapter();
			Accept_Const_Chapter();
			Accept_Types_Chapter();
			Accept_Variables_Chapter();
			Accept_Procedures_Funcions_Chapter();
		}

		public void Accept_Tags_Chapter()
		{
			if (Accept_Raw("label"))
			{
				if (!Accept_Operator_Tag())
				{
					Unhandled_Error("метка оператора");
				}

				while (Accept_Raw(","))
				{
					if (!Accept_Operator_Tag())
					{
						Unhandled_Error("метка оператора");
					}
				}

				if (!Accept_Raw(";")) Raise_Error(14);
			}
			//Иначе пусто
		}

		public void Accept_Const_Chapter()
		{
			if (Accept_Raw("const"))
			{
				if (!Accept_Const_Description()) Raise_Error(2);
				if (!Accept_Raw(";")) Raise_Error(14);

				while (Accept_Const_Description())
				{
					if (!Accept_Raw(";")) Raise_Error(14);
				}
			}
			//Иначе пусто
		}

		public void Accept_Types_Chapter()
		{
			if (Accept_Raw("type"))
			{
				if (!Accept_Type_Description()) Raise_Error(2);
				if (!Accept_Raw(";")) Raise_Error(14);

				while (Accept_Type_Description())
				{
					if (!Accept_Raw(";")) Raise_Error(14);
				}

			}
		}

		public void Accept_Variables_Chapter()
		{
			if (Accept_Raw("var"))
			{
				if (!Accept_Variables_Description()) Raise_Error(2);
				if (!Accept_Raw(";")) Raise_Error(14);

				while (Accept_Variables_Description())
				{
					if (!Accept_Raw(";")) Raise_Error(14);
				}
			}
		}

		public void Accept_Procedures_Funcions_Chapter()
		{
			//без приколов, ведь описание всегда с ключевых слов procedure/function
			if (Accept_Procedure_Description() || Accept_Function_Description())
			{
				if (!Accept_Raw(";")) Raise_Error(14);
				while (Accept_Procedure_Description() || Accept_Function_Description())
				{
					if (!Accept_Raw(";")) Raise_Error(14);
				}
			}

		}

		public bool Accept_Operators_Chapter()
		{
			if (!Accept_Compound_Operator())
			{
				Unhandled_Error("составной оператор");
			}
			return true;
		}

		public bool Accept_Operator_Tag()
		{
			if (Accept_Unsigned_Integer())
			{
				return true;
			}
			else return false;
		}

		public bool Accept_Const_Description()
		{
			if (!Accept_Const_Name())
			{
				Unhandled_Error("имя константы");
			}

			if (!Accept_Raw("=")) Raise_Error(16);

			if (!Accept_Const())
			{
				Unhandled_Error("константа");
			}

			return true;
		}

		public bool Accept_Type_Description()
		{
			if (!Accept_Type_Name())
			{
				Unhandled_Error("имя типа");
			}

			if (!Accept_Raw("=")) Raise_Error(16);

			if (!Accept_Type())
			{
				Unhandled_Error("тип");
			}

			return true;
		}

		public bool Accept_Variables_Description()
		{
			do
			{
				if (!Accept_Variable_Name()) Unhandled_Error("имя переменной");
			} while (Accept_Raw(","));

			if (!Accept_Raw(":")) Raise_Error(5);

			if (!Accept_Type())
			{
				Unhandled_Error("тип");
			}

			return true;
		}

		public bool Accept_Procedure_Description()
		{
			if (Accept_Procedure_Heading())
			{
				if (!Accept_Raw(";")) Raise_Error(14);

				//Опасная штука, мне она ваще не нравится
				Accept_Block();

				return true;
			}
			else return false;
		}

		public bool Accept_Function_Description()
		{
			return true;
		}

		public bool Accept_Compound_Operator()
		{
			if (Accept_Raw("begin"))
			{
				if (!Accept_Operator())
				{
					Unhandled_Error("оператор");
				}

				while (Accept_Raw(";"))
				{
					if (!Accept_Operator()) ;
				}

				if (!Accept_Raw("end")) Raise_Error(13);

				return true;
			}
			else return false;
		}

		public bool Accept_Operator()
		{
			if (Accept_Operator_Tag())
			{
				if (!Accept_Raw(":")) Raise_Error(5);
			}

			Position Backup = Save_Position();

			if (!Accept_Main_Operator())
			{
				Set_Position(Backup);
				if (!Accept_Derivative_Operator())
				{
					Unhandled_Error("основной или производный оператор");
				}
			}

			return true;
		}

		public bool Accept_Main_Operator()
		{
			Position Backup = Save_Position();
			if (!Accept_Assignment_Operator())
			{
				Set_Position(Backup);

				if (!Accept_Void_Operator())
				{
					Set_Position(Backup);

					if (!Accept_Transition_Operator())
					{
						Set_Position(Backup);

						if (!Accept_Procedure_Operator())
						{
							Set_Position(Backup);
							return false;
						}
					}
				}
			}
			return true;
		}

		public bool Accept_Derivative_Operator()
		{
			Position Backup = Save_Position();
			if (!Accept_Compound_Operator())
			{
				Set_Position(Backup);
				if (!Accept_Selecting_Operator())
				{
					Set_Position(Backup);
					if (!Accept_Cycle_Operator())
					{
						Set_Position(Backup);
						if (!Accept_Connection_Operator())
						{
							Set_Position(Backup);
							return false;
						}
					}
				}
			}
			return true;
		}

		public bool Accept_Assignment_Operator()
		{
			Position Backup = Save_Position();
			if (Accept_Function_Name())
			{
				if (!Accept_Raw(":=")) Raise_Error(51);

				if (!Accept_Expression())
				{
					Unhandled_Error("выражение");
				}
				return true;
			}

			Set_Position(Backup);

			if (Accept_Variable())
			{

				if (!Accept_Raw(":=")) Raise_Error(51);

				if (!Accept_Expression())
				{
					Unhandled_Error("выражение");
				}
				return true;
			}
			return false;
		}

		public bool Accept_Void_Operator()
		{
			Position Backup = new Position();
			//По сути нужно знать, что следующий токен - ;
			if (!Accept_Raw(";")) return false;
			else
            {
				Set_Position(Backup);
				return true;
            }

		}

		public bool Accept_Transition_Operator()
		{
			if (Accept_Raw("goto"))
			{
				if (!Accept_Operator_Tag())
				{
					Unhandled_Error("метка оператора");
				}
				return true;
			}
			else return false;
		}

		public bool Accept_Procedure_Operator()
		{
			if (Accept_Procedure_Name())
			{
				if (Accept_Raw("("))
				{

					do
					{
						if (!Accept_Actual_Parameter())
						{
							Unhandled_Error("фактический оператор");
						}
					} while (Accept_Raw(","));

					if (!Accept_Raw(")")) Raise_Error(4);
				}
				return true;
			}
			else return false;
		}

		public bool Accept_Variable()
		{
			Position Backup = Save_Position();

			if (!Accept_Variable_Name())
			{
				Set_Position(Backup);

				if (!Accept_Field_Name())
				{
					Set_Position(Backup);
					return false;
				}
			}

			string Accepted_Symbol = Current_Lexem.raw_value;

			while (Accept_Raw("[") || Accept_Raw(".") || Accept_Raw("↑"))
			{
				if (Accepted_Symbol == "[")
				{
					do
					{
						if (!Accept_Index()) Unhandled_Error("индекс");

					} while (Accept_Raw(","));

					if (!Accept_Raw("]")) Raise_Error(12);
				}
				else if (Accepted_Symbol == ".")
				{
					if (!Accept_Field_Name()) Unhandled_Error("имя поля");
				}
				else if (Accepted_Symbol == "↑")
				{
					// ну круто молодец че
				}
				else
				{
					Set_Position(Backup);
					return false;
				}
				Accepted_Symbol = Current_Lexem.raw_value;
			}

			return true;
		}

		public bool Accept_Function_Name()
		{
			return false;
		}

		public bool Accept_Expression()
		{
			Position Backup = Save_Position();
			if (!Accept_Arithmetic_Expression())
			{
				Set_Position(Backup);
				if (!Accept_Literal_Expression())
				{
					Set_Position(Backup);
					if (!Accept_Logical_Expression())
					{
						Set_Position(Backup);
						if (!Accept_Enumerated_Type_Expression())
						{
							Set_Position(Backup);
							if (!Accept_Regular_Type_Expression())
							{
								Set_Position(Backup);
								if (!Accept_Combined_Type_Expression())
								{
									Set_Position(Backup);
									if (!Accept_Multiple_Expression())
									{
										Set_Position(Backup);
										return false;
									}
								}
							}
						}
					}
				}
			}
			return true;
		}

		public bool Accept_Procedure_Name()
		{
			return false;
		}

		public bool Accept_Actual_Parameter()
		{
			Position Backup = Save_Position();
			if (!Accept_Expression())
			{
				Set_Position(Backup);
				if (!Accept_Variable())
				{
					Set_Position(Backup);
					if (!Accept_Function_Name())
					{
						Set_Position(Backup);
						return false;
					}
				}
			}
			return true;
		}

		public bool Accept_Selecting_Operator()
		{
			Position Backup = Save_Position();
			if (!Accept_Conditional_Operator())
			{
				Set_Position(Backup);
				if (!Accept_Variant_Operator())
				{
					Set_Position(Backup);
					return false;
				}
			}
			return true;
		}

		public bool Accept_Cycle_Operator()
		{
			Position Backup = Save_Position();
			if (!Accept_Loop_Operator_With_Parameter())
			{
				Set_Position(Backup);
				if (!Accept_Loop_Operator_With_Precondition())
				{
					Set_Position(Backup);
					if (!Accept_Loop_Operator_With_Postcondition())
					{
						Set_Position(Backup);
						return false;
					}
				}
			}
			return true;
		}

		public bool Accept_Connection_Operator()
		{
			if (!Accept_Raw("with"))
			{
				if (!Accept_Variable())
				{
					Unhandled_Error("переменная");
				}

				while (Accept_Raw(","))
				{
					if (!Accept_Variable())
					{
						Unhandled_Error("переменная");
					}
				}

				if (!Accept_Raw("do")) Raise_Error(54);

				if (!Accept_Variable())
				{
					Unhandled_Error("оператор");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Conditional_Operator()
		{
			if (Accept_Raw("if"))
			{
				if (!Accept_Logical_Expression())
				{
					Unhandled_Error("логическое выражение");
				}

				if (!Accept_Raw("then")) Raise_Error(52);

				if (!Accept_Operator())
				{
					Unhandled_Error("оператор");
				}

				if (Accept_Raw("else"))
				{
					if (!Accept_Operator())
					{
						Unhandled_Error("оператор");
					}
				}

				return true;

			}
			else return false;
		}

		public bool Accept_Variant_Operator()
		{
			if (Accept_Raw("case"))
			{
				if (!Accept_Operator_Selector())
				{
					Unhandled_Error("селектор оператора");
				}

				if (!Accept_Raw("of")) Raise_Error(8);

				do
				{
					if (!Accept_Variant_Tag())
					{
						Unhandled_Error("метка оператора");
					}

					while (Accept_Raw(","))
					{
						Accept_Variant_Tag();
					}

					if (!Accept_Raw(":")) Raise_Error(5);

					if (!Accept_Operator())
					{
						Unhandled_Error("оператор");
					}

				} while (Accept_Raw(";") && !Accept_Raw("end"));

				if (!Accept_Raw(";"))
				{
					if (!Accept_Raw("end")) Raise_Error(13);
				}

				return true;
			}
			return false;
		}

		public bool Accept_Logical_Expression()
		{
			Position Backup = Save_Position();

			if (!Accept_Simple_Logical_Expression())
			{
				Set_Position(Backup);

				if (!Accept_Relationship())
				{
					Set_Position(Backup);
					return false;
				}
			}

			return true;
		}

		public bool Accept_Operator_Selector()
		{
			if (!Accept_Index_Expression())
			{
				Unhandled_Error("индексное выражение");
			}

			return true;
		}

		public bool Accept_Variant_Tag()
		{
			if (!Accept_Const())
			{
				Unhandled_Error("константа");
			}

			return true;
		}

		public bool Accept_Index_Expression()
		{
			Position Backup = Save_Position();

			if (!Accept_Arithmetic_Expression_Integer_Type())
			{
				Set_Position(Backup);
				if (!Accept_Literal_Expression())
				{
					Set_Position(Backup);

					if (!Accept_Logical_Expression())
					{
						Set_Position(Backup);

						if (!Accept_Enumerated_Type_Expression())
						{
							Set_Position(Backup);
							return false;
						}
					}
				}
			}

			return true;
		}

		public bool Accept_Const()
		{
			Position Backup = Save_Position();

			if (!Accept_Name())
			{
				Set_Position(Backup);

				if (!Accept_Literal_Const())
				{
					Set_Position(Backup);

					if (!Accept_String_Const())
					{
						Set_Position(Backup);

						Position Backup_1;
						if (Accept_Raw("+"))
						{
							Backup_1 = Save_Position();
						}
						else if (Accept_Raw("-"))
						{
							Backup_1 = Save_Position();
						}
						else
						{
							Backup_1 = Save_Position();
						}
						if (!Accept_Arithmetic_Const_Name())
						{
							Set_Position(Backup_1);

							if (!Accept_Number_Without_Sign())
							{
								Set_Position(Backup);

								return false;
							}
						}
						return true;
					}
				}
			}

			return true;
		}

		public bool Accept_Loop_Operator_With_Parameter()
		{
			if (Accept_Raw("for"))
			{
				if (!Accept_Cycle_Parameter())
				{
					Unhandled_Error("параметр цикла");
				}

				if (!Accept_Raw(":=")) Raise_Error(51);

				if (!Accept_Expression())
				{
					Unhandled_Error("выражение");
				}

				if (!Accept_Raw("to") && !Accept_Raw("downto"))
				{
					Raise_Error(55);
				}

				if (!Accept_Expression())
				{
					Unhandled_Error("выражение");
				}

				if (!Accept_Raw("do"))
				{
					Raise_Error(54);
				}

				if (!Accept_Operator())
				{
					Unhandled_Error("оператор");
				}

				return true;
			}

			return false;
		}

		public bool Accept_Loop_Operator_With_Precondition()
		{
			if (Accept_Raw("while"))
			{
				if (!Accept_Logical_Expression()) Unhandled_Error("логическое выражение");

				if (!Accept_Raw("do")) Raise_Error(54);

				if (!Accept_Operator()) Unhandled_Error("оператор");

				return true;
			}
			else return false;
		}

		public bool Accept_Loop_Operator_With_Postcondition()
		{
			if (Accept_Raw("repeat"))
			{
				if (!Accept_Operator()) Unhandled_Error("оператор");

				while (Accept_Raw(";"))
				{
					if (!Accept_Operator()) Unhandled_Error("оператор");
				}

				if (!Accept_Raw("until")) Raise_Error(53);

				if (!Accept_Logical_Expression()) Unhandled_Error("логическое выражение");

				return true;
			}
			else return false;
		}

		public bool Accept_Cycle_Parameter()
		{
			if (!Accept_Variable_Name())
			{
				Unhandled_Error("имя переменной");
			}

			return true;
		}

		public bool Accept_Variable_Name()
		{
			return false;
		}

		public bool Accept_Arithmetic_Expression()
		{
			if (Accept_Raw("+") || Accept_Raw("-"))
			{
				if (!Accept_Summand()) Unhandled_Error("слагаемое");

				while (Accept_Raw("+") || Accept_Raw("-"))
				{
					if (!Accept_Summand()) Unhandled_Error("слагаемое");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Literal_Expression()
		{
			Position Backup = Save_Position();
			if (!Accept_Literal_Type_Const())
			{
				Set_Position(Backup);
				if (!Accept_Literal_Type_Variable())
				{
					Set_Position(Backup);
					if (!Accept_Literal_Type_Funcion())
					{
						Set_Position(Backup);
						return false;
					}
				}
			}
			return true;
		}

		public bool Accept_Enumerated_Type_Expression()
		{
			Position Backup = Save_Position();
			if (!Accept_Enumerated_Type_Const())
			{
				Set_Position(Backup);
				if (!Accept_Enumerated_Type_Variable())
				{
					Set_Position(Backup);
					if (!Accept_Enumerated_Type_Function())
					{
						Set_Position(Backup);
						return false;
					}
				}
			}
			return true;
		}

		public bool Accept_Regular_Type_Expression()
		{
			Position Backup = Save_Position();
			if (!Accept_Regular_Type_Variable())
			{
				Set_Position(Backup);
				if (!Accept_String_Const())
				{
					Set_Position(Backup);
					return false;
				}
			}
			return true;
		}

		public bool Accept_Combined_Type_Expression()
		{
			if (!Accept_Combined_Type_Variable()) return false;
			return true;
		}

		public bool Accept_Reference_Expression()
		{
			Position Backup = Save_Position();
			if (!Accept_Raw("nil"))
			{
				Set_Position(Backup);
				if (!Accept_Reference_Type_Variable())
				{
					Set_Position(Backup);
					if (!Accept_Reference_Type_Function())
					{
						Set_Position(Backup);
						return false;
					}
				}
			}

			return true;
		}

		public bool Accept_Multiple_Expression()
		{
			do
			{
				Position Backup = Save_Position();

				if (!Accept_Set_Constructor())
				{
					Set_Position(Backup);
					if (!Accept_Multiple_Type_Variable())
					{
						Set_Position(Backup);
						if (Accept_Raw("("))
						{
							if (!Accept_Multiple_Expression()) Unhandled_Error("множественное выражение");

							if (!Accept_Raw(")")) Raise_Error(4);
						}
						else
						{
							Set_Position(Backup);
							return false;
						}
					}
				}
			} while (Accept_Raw("*") || Accept_Raw("+") || Accept_Raw("-"));

			return true;

		}

		public bool Accept_Summand()
		{
			do
			{
				if (!Accept_Multiplier()) Unhandled_Error("множитель");
			} while (Accept_Raw("*") || Accept_Raw("/")
				|| Accept_Raw("mod") || Accept_Raw("div"));
			return true;
		}

		public bool Accept_Multiplier()
		{
			if (Accept_Raw("("))
			{
				if (!Accept_Arithmetic_Expression()) Unhandled_Error("Арифметическое выражение");

				if (!Accept_Raw(")")) Raise_Error(4);
				return true;
			}
			else
			{
				Position Backup = Save_Position();
				if (!Accept_Real_Without_Sign())
				{
					Set_Position(Backup);
					if (!Accept_Unsigned_Integer())
					{
						Set_Position(Backup);
						if (!Accept_Variable())
						{
							Set_Position(Backup);
							if (!Accept_Funcion())
							{
								Set_Position(Backup);
								return false;
							}
						}
					}
				}
				return true;
			}

		}

		public bool Accept_Real_Without_Sign()
		{
			Position Backup = Save_Position();

			if(!Accept_Fixed_Point_Number())
            {
				Set_Position(Backup);

				if (!Accept_Floating_Point_Number())
                {
					Set_Position(Backup);

					return false;
                }

			}

			return false;
		}

		public bool Accept_Unsigned_Integer()
		{
			bool check = false;
			while (Current_Lexem.type == "digit")
            {
				check = true;
				NextSym();
            }
			return check;
		}

		public bool Accept_Funcion()
		{
			if (Accept_Function_Name())
			{
				if(Accept_Raw("("))
                {
					do
					{
						if (!Accept_Actual_Parameter()) Unhandled_Error("фактический параметр");
					} while (Accept_Raw(","));

					if (!Accept_Raw(")")) Raise_Error(4);

                }

				return true;
			}
			else return false;
		}

		public bool Accept_Literal_Type_Const()
		{
			return false;
		}

		public bool Accept_Literal_Type_Variable()
		{
			return false;
		}

		public bool Accept_Literal_Type_Funcion()
		{
			return false;
		}

		public bool Accept_Enumerated_Type_Const()
		{
			return false;
		}

		public bool Accept_Enumerated_Type_Variable()
		{
			return false;
		}

		public bool Accept_Enumerated_Type_Function()
		{
			return false;
		}

		public bool Accept_Regular_Type_Variable()
		{
			return false;
		}

		public bool Accept_String_Const()
		{
			return false;
		}

		public bool Accept_Combined_Type_Variable()
		{
			return false;
		}

		public bool Accept_Reference_Type_Variable()
		{
			return false;
		}

		public bool Accept_Reference_Type_Function()
		{
			return false;
		}

		//Это сложно так-то
		public bool Accept_Set_Constructor()
		{
			if (Accept_Raw("["))
			{
				if (Accept_Index_Expression())
				{
					if (Accept_Raw(".."))
					{
						if (!Accept_Index_Expression()) Unhandled_Error("индексное выражение");
					}

					while (Accept_Raw(","))
					{
						if (!Accept_Index_Expression()) Unhandled_Error("индексное выражение");

						if (Accept_Raw(".."))
						{
							if (!Accept_Index_Expression()) Unhandled_Error("индексное выражение");
						}
					}
				}

				if (!Accept_Raw("]")) Raise_Error(12);

				return true;
			}

			return false;
		}

		public bool Accept_Multiple_Type_Variable()
		{
			return false;
		}

		public bool Accept_Arithmetic_Expression_Integer_Type()
		{
			return false;
		}

		public bool Accept_Simple_Logical_Expression()
		{
			do
			{
				if (!Accept_Logical_Summand())
				{
					Unhandled_Error("логическое слагаемое");
				}
			} while (Accept_Raw("or"));

			return true;
		}

		public bool Accept_Relationship()
		{
			Position Backup = Save_Position();

			if (!Accept_Scalar_Relationship())
			{
				Set_Position(Backup);

				if (!Accept_String_Relastionship())
				{
					Set_Position(Backup);

					if (!Accept_Multiple_Relastionship())
					{
						Set_Position(Backup);
						return false;
					}
				}
			}

			return true;
		}

		public bool Accept_Logical_Summand()
		{
			do
			{
				if (!Accept_Logical_Multiplier())
				{
					Unhandled_Error("логический множитель");
				}
			} while (Accept_Raw("and"));

			return true;
		}

		public bool Accept_Logical_Multiplier()
		{
			Position Backup = Save_Position();

			if (!Accept_Logical_Type_Const())
			{
				Set_Position(Backup);

				if (!Accept_Logical_Type_Variable())
				{
					Set_Position(Backup);

					if (!Accept_Logical_Type_Function())
					{
						Set_Position(Backup);

						if (Accept_Raw("not"))
						{
							if (!Accept_Logical_Multiplier()) Unhandled_Error("логический множитель");
							return true;
						}

						Set_Position(Backup);

						if (Accept_Raw("("))
						{
							if (!Accept_Logical_Expression())
							{
								Unhandled_Error("логическое выражение");
							}

							if (!Accept_Raw(")")) Raise_Error(4);
						}
						else
						{
							Set_Position(Backup);
							return false;
						}
					}
				}
			}

			return true;
		}

		public bool Accept_Logical_Type_Const()
		{
			return false;
		}

		public bool Accept_Logical_Type_Variable()
		{
			return false;
		}

		public bool Accept_Logical_Type_Function()
		{
			return false;
		}

		public bool Accept_Scalar_Relationship()
		{
			Position Backup = Save_Position();

			if (!Accept_Arithmetic_Expression())
			{
				Set_Position(Backup);

				if (!Accept_Simple_Logical_Expression())
				{
					Set_Position(Backup);

					if (!Accept_Enumerated_Type_Expression())
					{
						Set_Position(Backup);

						if (!Accept_Literal_Expression())
						{
							Set_Position(Backup);

							if (!Accept_Reference_Expression())
							{
								Set_Position(Backup);
								return false;
							}
							else
							{
								if (!Accept_Raw("=") && !Accept_Raw("<>"))
								{
									Unhandled_Error("= или <>");
								}
								return true;
							}
						}
						else
						{
							if (!Accept_Comparison_Operaion())
							{
								Unhandled_Error("операция сравнения");
							}

							if (!Accept_Literal_Expression())
							{
								Unhandled_Error("литерное выражение");
							}
							return true;
						}
					}
					else
					{
						if (!Accept_Comparison_Operaion())
						{
							Unhandled_Error("операция сравнения");
						}

						if (!Accept_Enumerated_Type_Expression())
						{
							Unhandled_Error("выражение перечислимого типа");
						}
						return true;
					}
				}
				else
				{
					if (!Accept_Comparison_Operaion())
					{
						Unhandled_Error("операция сравнения");
					}

					if (!Accept_Simple_Logical_Expression())
					{
						Unhandled_Error("простое логическое выражение");
					}
					return true;
				}
			}
			else
			{
				if (!Accept_Comparison_Operaion())
				{
					Unhandled_Error("операция сравнения");
				}

				if (!Accept_Arithmetic_Expression())
				{
					Unhandled_Error("арифметическое выражение");
				}
				return true;
			}
		}

		public bool Accept_String_Relastionship()
		{
			Position Backup = Save_Position();

			if (!Accept_String_Const())
			{
				Set_Position(Backup);

				if (!Accept_Variable())
				{
					Set_Position(Backup);
					return false;
				}
			}

			if (!Accept_Comparison_Operaion())
			{
				Unhandled_Error("операция сравнения");
			}

			Position Backup_1 = Save_Position();

			if (!Accept_String_Const())
			{
				Set_Position(Backup_1);

				if (!Accept_Variable())
				{
					Set_Position(Backup);
					return false;
				}
			}
			return true;
		}

		public bool Accept_Multiple_Relastionship()
		{
			Position Backup = Save_Position();

			if (!Accept_Multiple_Expression())
			{
				Set_Position(Backup);

				if (!Accept_Index_Expression())
				{
					Set_Position(Backup);
					return false;
				}
				else
				{
					if (!Accept_Raw("=") && !Accept_Raw("<>") && !Accept_Raw("<=") && Accept_Raw(">="))
					{
						Set_Position(Backup);
						return false;
					}
					else
					{
						if (!Accept_Multiple_Expression())
						{
							Unhandled_Error("множественное выражение");
						}
						return true;
					}
				}
			}
			else
			{
				if (!Accept_Index_Expression())
				{
					Unhandled_Error("индексное выражение");
				}

				if (!Accept_Raw("in"))
				{
					Unhandled_Error("in");
				}

				if (!Accept_Multiple_Expression())
				{
					Unhandled_Error("множественное выражение");
				}
				return true;
			}
		}

		public bool Accept_Comparison_Operaion()
		{
			if (!Accept_Raw("=") && !Accept_Raw("<>") && !Accept_Raw("<=") && !Accept_Raw("<") && !Accept_Raw(">=") && !Accept_Raw(">"))
				return false;
			return true;
		}

		public bool Accept_Index()
		{
			if (!Accept_Index_Expression())
			{
				Unhandled_Error("индексное выражение");
			}
			return true;
		}

		public bool Accept_Field_Name()
		{
			return false;
		}

		public bool Accept_Type()
		{
			Position Backup = new Position();

			if (!Accept_Type_Name())
			{
				Set_Position(Backup);

				if (!Accept_Enumerated_Type_Task())
				{
					Set_Position(Backup);

					if (!Accept_Limited_Type_Task())
					{
						Set_Position(Backup);

						if (!Accept_Reference_Type_Task())
						{
							Set_Position(Backup);

							//Интересно...
							Position Backup_1;
							if (Accept_Raw("packed"))
							{
								Backup_1 = new Position();
							}
							else
							{
								Backup_1 = new Position();

							}
							if (!Accept_Regular_Type_Task())
							{
								Set_Position(Backup_1);

								if (!Accept_Combined_Type_Task())
								{
									Set_Position(Backup_1);

									if (!Accept_Multiple_Type_Task())
									{
										Set_Position(Backup_1);

										if (!Accept_File_Type_Task())
										{
											Set_Position(Backup);
											return false;
										}
									}
								}
							}
						}
					}
				}
			}

			return true;
		}

		public bool Accept_Const_Name()
		{
			return false;
		}

		public bool Accept_Name()
		{
			if (Accept_Identifier()) return true;
			else return false;
		}

		public bool Accept_Literal_Const()
		{
			return false;
		}

		public bool Accept_Arithmetic_Const_Name()
		{
			return false;
		}

		//Вообще unsigned, но пусть так...
		public bool Accept_Number_Without_Sign()
		{
			Position Backup = Save_Position();

			if(!Accept_Unsigned_Integer())
            {
				Set_Position(Backup);

				if(!Accept_Real_Without_Sign())
                {
					Set_Position(Backup);
					return false;
                }
            }

			return true;
		}

		public bool Accept_Type_Name()
		{
			return false;
		}

		public bool Accept_Enumerated_Type_Task()
		{
			if (Accept_Raw("("))
			{
				do
				{
					if (!Accept_Identifier())
					{
						Unhandled_Error("идентифакатор");
					}
				} while (Accept_Raw(","));

				if (!Accept_Raw(")")) Raise_Error(4);

				return true;
			}
			else return false;
		}

		public bool Accept_Limited_Type_Task()
		{
			if (Accept_Const())
			{
				if (!Accept_Raw("..")) Raise_Error(74);

				if (Accept_Const()) Unhandled_Error("константа");

				return true;
			}
			else return false;
		}

		public bool Accept_Reference_Type_Task()
		{
			if (Accept_Raw("↑"))
			{
				if (!Accept_Type_Name())
				{
					Unhandled_Error("имя типа");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Regular_Type_Task()
		{
			if (Accept_Raw("array"))
			{
				if (!Accept_Raw("[")) Raise_Error(11);

				do
				{
					if (!Accept_Index_Type())
					{
						Unhandled_Error("тип индекса");
					}
				} while (Accept_Raw(","));

				if (!Accept_Raw("]")) Raise_Error(12);

				if (!Accept_Raw("of")) Raise_Error(8);

				if (!Accept_Type())
				{
					Unhandled_Error("тип");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Combined_Type_Task()
		{
			if (Accept_Raw("record"))
			{
				if (!Accept_Field_List())
				{
					Unhandled_Error("список полей");
				}

				if (Accept_Raw(";"))
				{
					if (!Accept_Raw("end"))
					{
						Unhandled_Error("end");
					}
				}
				else
				{
					if (!Accept_Raw("end"))
					{
						Unhandled_Error("end");
					}
				}

				return true;
			}

			return false;
		}

		public bool Accept_Multiple_Type_Task()
		{
			if (Accept_Raw("set"))
			{
				if (!Accept_Raw("of")) Raise_Error(8);

				if (!Accept_Type())
				{
					Unhandled_Error("тип");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_File_Type_Task()
		{
			if (Accept_Raw("file"))
			{
				if (!Accept_Raw("of")) Raise_Error(8);

				if (!Accept_Type())
				{
					Unhandled_Error("тип");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Identifier()
		{
			if(Current_Lexem.type == "letter")
            {
				NextSym();

				while(Current_Lexem.type == "letter" || Current_Lexem.type == "letter")
            }
            else
            {
				return false;1
            }
		}

		public bool Accept_Index_Type()
		{
			return false;
		}

		public bool Accept_Field_List()
		{
			if (Accept_Record_Section())
			{
				while (Accept_Raw(","))
				{
					if (!Accept_Record_Section())
					{
						Unhandled_Error("секция записи");
					}
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Record_Section()
		{
			if (Accept_Field_Name())
			{
				while (Accept_Raw(","))
				{
					if (!Accept_Field_Name())
					{
						Unhandled_Error("имя поля");
					}
				}

				if (!Accept_Raw(":")) Raise_Error(5);

				if (!Accept_Type())
				{
					Unhandled_Error("тип");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Function_Heading()
		{
			if (Accept_Raw("function"))
			{
				if (!Accept_Function_Name())
				{
					Unhandled_Error("имя функции");
				}

				if (!Accept_Set_Of_Formal_Parameters())
				{
					Unhandled_Error("совокупность формальных параметров");
				}

				if (!Accept_Raw(":")) Raise_Error(5);

				if (!Accept_Type_Name())
				{
					Unhandled_Error("имя типа");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Set_Of_Formal_Parameters()
		{
			if (Accept_Raw("("))
			{
				do
				{
					if (!Accept_Formal_Parameters_Section())
					{
						Unhandled_Error("секция формальных параметров");
					}
				} while (Accept_Raw(";"));

				if (!Accept_Raw(")")) Raise_Error(4);

				return true;
			}
			else return true;
		}

		public bool Accept_Procedure_Heading()
		{
			if (!Accept_Raw("procedure"))
			{
				if (!Accept_Procedure_Name())
				{
					Unhandled_Error("имя процедуры");
				}

				if (!Accept_Set_Of_Formal_Parameters())
				{
					Unhandled_Error("совокупность формальных параметров");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Formal_Parameters_Section()
		{
			Position Backup = Save_Position();

			if (!Accept_Value_Parameters())
			{
				Set_Position(Backup);

				if (!Accept_Variable_Parameters())
				{
					Set_Position(Backup);

					if (!Accept_Function_Parameter())
					{
						Set_Position(Backup);

						if (!Accept_Procedure_Parameter())
						{
							Set_Position(Backup);
							return false;
						}
					}
				}
			}
			return true;
		}

		public bool Accept_Value_Parameters()
		{
			if (Accept_Identifier())
			{
				while (Accept_Raw(","))
				{
					if (!Accept_Identifier())
					{
						Unhandled_Error("идентификатор");
					}
				}

				if (!Accept_Raw(":")) Raise_Error(5);

				if (!Accept_Type())
				{
					Unhandled_Error("имя типа");
				}

				return true;
			}
			else return false;
		}

		public bool Accept_Variable_Parameters()
		{
			if (Accept_Raw("var"))
			{
				if (Accept_Identifier())
				{
					while (Accept_Raw(","))
					{
						if (!Accept_Identifier())
						{
							Unhandled_Error("идентификатор");
						}
					}

					if (!Accept_Raw(":")) Raise_Error(5);

					if (!Accept_Type())
					{
						Unhandled_Error("имя типа");
					}

					return true;
				}
				else return false;
			}
			else return false;
		}

		public bool Accept_Function_Parameter()
		{
			if (Accept_Function_Heading()) return true;
			else return false;
		}

		public bool Accept_Procedure_Parameter()
		{
			if (Accept_Procedure_Heading()) return true;
			else return false;
		}

		public bool Accept_Fixed_Point_Number()
        {
			if (Current_Lexem.type == "digit")
			{
				NextSym();

				while (Current_Lexem.type == "digit")
                {
					NextSym();
                }

				if (!Accept_Raw(".")) Raise_Error(61);

				if(!(Current_Lexem.type == "digit"))
				{
					Unhandled_Error("цифра");
                }
                else
                {
					NextSym();
					while(Current_Lexem.type == "digit")
					{
						NextSym();
					}
                }
				//чекни тут
				return true;
			}
			else return false;
        }

		public bool Accept_Floating_Point_Number()
        {
			return false;
        }
	}
}

