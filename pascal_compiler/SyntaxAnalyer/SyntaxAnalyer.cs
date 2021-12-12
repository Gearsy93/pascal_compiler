/* Проверяет, удовлетворяет ли программа формальным правилам
* Вход: последовательность символов
 * Выход: синтаксически проверенная программа, синтаксические ошибки
 * Главная задача - найти все ошибки, не более
 */

/* Структура выражение: 
 * <S> ::= L
 * Вся программа закидывается в начальное выражение программа, затем последовательно вызываются ожидаемые последоывательности лексем. Если ожидаемые лексемы есть, идем дальше, иначе кидаем ошибку
*/

//Нейтрализация идет до ключевых слов

using System;
using InputOutput;
using LexicalAnalyzer;

namespace SyntaxAnalyzer
{
	public class Syntaxic
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

		////////////////ИНИЦИАЛИЗАЦИЯ///////////////////
		public Lexical Lexical_Analyzer;

		public string got;

		public Lexical.Lexem Last_Lexem;

		public IO Reader { get; }
		public Lexical.Lexem Current_Lexem;
		public Syntaxic(Lexical Input_Lexical_Analyzer, IO Reader_Input, Lexical.Lexem Current_Lexem_Input)
		{
			Lexical_Analyzer = Input_Lexical_Analyzer;
			Reader = Reader_Input;
			Current_Lexem = Current_Lexem_Input;
		}

		public void Unhandled_Error(string expected)
		{
			got = " Получено " + Current_Lexem.value.ToString();
			if (Current_Lexem.value.ToString() == "") got = " Встречен конец файла";
			Console.WriteLine("Col: " + Reader.Line_Position + " Ln: " + Reader.Line_Number + got + ", а ожидалось " + expected);
			System.Environment.Exit(1);
		}

		public void Raise_Error(int code)
		{
			//Добавить форматирование
			Console.WriteLine("Col: " + Reader.Line_Position + " Ln: " + Reader.Line_Number + " Description: " + Lexical.Get_Error(code));
			System.Environment.Exit(1);
		}

		//////////////////ВОЗВРАТ КАРЕТКИ///////////////////////

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
			Last_Lexem = Current_Lexem;
			Current_Lexem = Lexical_Analyzer.NextSym();
			Console.WriteLine("NN New Current Lexem: " + Current_Lexem.value);
		}

		//Проверяет совпадение полученной лексемы с текущей
		public bool Accept_Raw(string raw_value)
		{
			if (Current_Lexem.value.ToString().ToLower() == raw_value)
			{
				Console.WriteLine("SS - Accepted " + raw_value);
				if (Reader.Count < Reader.ProgramText.Length)
                {
					Last_Lexem = Current_Lexem;
					NextSym();
				}
				return true;
			}
			else
            {
				return false;
			}
		}

		//////////////////БНФ ФОРМЫ//////////////////
		///
		public void Accept_Program()
		{
			Console.WriteLine("EE Entered Accept_Program");

			//Шапка программы
			if (!Accept_Raw("program")) Raise_Error(3);
			if (!Accept_Name()) Raise_Error(2);
			if (!Accept_Raw(";")) Raise_Error(14);


			Accept_Block();

			if (!Accept_Raw(".")) Raise_Error(61);
			Console.WriteLine("Done Accept_Program");
		}

		public void Accept_Block()
		{
			Console.WriteLine("EE Entered Accept_Block");
			Accept_Description_Chapter();
			Console.WriteLine("--------------------READING OPERATORS--------------------");
			if (!Accept_Operators_Chapter()) Unhandled_Error("раздел описаний");
			Console.WriteLine("Done Accept_Block");
		}

		//Возможно нет описания переменных
		public void Accept_Description_Chapter()
		{
			Console.WriteLine("EE Entered Accept_Description_Chapter");
			Accept_Variables_Chapter();
			Console.WriteLine("Done Accept_Description_Chapter");
		}

		public bool Accept_Operators_Chapter()
		{
			Console.WriteLine("EE Entered Accept_Operators_Chapter");
			if (!Accept_Compound_Operator())
			{
				return false;
			}
			Console.WriteLine("Done Accept_Operators_Chapter");
			return true;
		}

        public void Accept_Variables_Chapter()
        {
			Console.WriteLine("EE Entered Accept_Variables_Chapter");
			if (Accept_Raw("var"))
            {
                if (!Accept_Variables_Description()) Raise_Error(2);
                if (!Accept_Raw(";")) Raise_Error(14);

                while (Accept_Variables_Description())
                {
                    if (!Accept_Raw(";")) Raise_Error(14);
                }
            }
			Console.WriteLine("Done Accept_Variables_Chapter");

		}

        public bool Accept_Compound_Operator()
        {
			Console.WriteLine("EE Entered Accept_Compound_Operator");
			if (Accept_Raw("begin"))
            {
				if (Accept_Operator())
				{
					if (!Accept_Raw(";")) Raise_Error(14);

					while (Accept_Operator())
					{
						if (!Accept_Raw(";") && Last_Lexem.value != ";")
                        {
							Raise_Error(14);
						}
					}
				}

                if (!Accept_Raw("end")) Raise_Error(13);

				Console.WriteLine("Done Accept_Compound_Operator");
				return true;
            }
            else return false;
		}

        public bool Accept_Operator()
        {
			Console.WriteLine("EE Entered Accept_Operator");
			Position Backup = Save_Position();

            if (!Accept_Main_Operator())
            {
                Set_Position(Backup);
                if (!Accept_Derivative_Operator())
                {
					Set_Position(Backup);
					return false;

				}
            }
			Console.WriteLine("Done Accept_Operator");
			return true;
        }

		public bool Accept_Main_Operator()
        {
			Console.WriteLine("EE Entered Accept_Main_Operator");
			Position Backup = Save_Position();

            if (!Accept_Void_Operator())
            {
                Set_Position(Backup);

                if (!Accept_Procedure_Operator())
                {
                    Set_Position(Backup);

					if (!Accept_Assignment_Operator())
					{
						Set_Position(Backup);
						return false;
					}
                }
            }
			Console.WriteLine("Done Accept_Main_Operator");
			return true;
		}

		public bool Accept_Procedure_Operator()
		{
			Console.WriteLine("EE Entered Accept_Procedure_Operator");
			if (Accept_Procedure_Name())
			{
				if (Accept_Raw("("))
				{
					if (Accept_Actual_Parameter())
                    {
						while (Accept_Raw(","))
                        {
							if (!Accept_Actual_Parameter())
							{
								Unhandled_Error("фактический оператор");
							}
						}
					}

					if (!Accept_Raw(")")) Raise_Error(4); //Пропустить до )
					Console.WriteLine("Done Accept_Procedure_Operator");
					return true;
				}
				else return false;
			}
			else return false;
		}

		public bool Accept_Derivative_Operator()
        {
			Console.WriteLine("EE Entered Accept_Derivative_Operator");
			Position Backup = Save_Position();
            if (!Accept_Compound_Operator())
            {
                Set_Position(Backup);

				if(!Accept_Selecting_Operator())
                {
					Set_Position(Backup);
					if (!Accept_Cycle_Operator())
                    {
						Set_Position(Backup);
						return false;
					}
					
				}
                
            }
			Console.WriteLine("Done Accept_Derivative_Operator");
			return true;
        }

		public bool Accept_Variables_Description()
        {
			Console.WriteLine("EE Entered Accept_Variables_Description");
			if (!Accept_Variable_Name())
			{
				return false;
			}

			while (Accept_Raw(","))
			{
                if (!Accept_Variable_Name())
                {
					Unhandled_Error("имя переменной");
				}
            } 

            if (!Accept_Raw(":")) Raise_Error(5);

            if (!Accept_Type())
            {
                Unhandled_Error("тип");
            }
			Console.WriteLine("Done Accept_Variables_Description");
			return true;
		}

        public bool Accept_Assignment_Operator()
        {
			Console.WriteLine("EE Entered Accept_Assignment_Operator");

			if (Accept_Variable_Name())
			{
				if (!Accept_Raw(":=")) Raise_Error(51);

				if (!Accept_Expression())
				{
					Unhandled_Error("выражение");
				}
				Console.WriteLine("Done Accept_Assignment_Operator");
				return true;
			}
			return false;
        }

        public bool Accept_Void_Operator()
        {
			Console.WriteLine("EE Entered Accept_Void_Operator");
			Position Backup = new Position();
            //По сути нужно знать, что следующий токен - ;
            if (!Accept_Raw(";")) return false;
            else
            {
                Set_Position(Backup);
				Console.WriteLine("Done Accept_Void_Operator");
				return true;
            }

        }

		public bool Accept_Selecting_Operator()
		{
			Console.WriteLine("EE Entered Accept_Selecting_Operator");
			if (!Accept_Conditional_Operator())
			{
				return false;
			}
			Console.WriteLine("Done Accept_Selecting_Operator");
			return true;
		}

		public bool Accept_Conditional_Operator()
		{
			//В случае ошибки - искать до конца программы else, если else нет, то капец...
			Console.WriteLine("EE Entered Accept_Conditional_Operator");
			if (Accept_Raw("if"))
			{
				if (!Accept_Logical_Expression())
				{
					Unhandled_Error("логическое выражение");
				}

				if (!Accept_Raw("then")) Raise_Error(52);

				if (!Accept_Compound_Operator())
				{
					if (!Accept_Operator())
                    {
						Unhandled_Error("оператор");
					}
				}

				if (Accept_Raw("else"))
				{
					if (!Accept_Compound_Operator())
					{
						if (!Accept_Operator())
						{
							Unhandled_Error("оператор");
						}
					}
				}
                else
                {
					if (!Accept_Raw(";")) Raise_Error(14);
				}

				Console.WriteLine("Done Accept_Conditional_Operator");
				return true;

			}
			else return false;
		}

		public bool Accept_Cycle_Operator()
		{
			Console.WriteLine("EE Entered Accept_Cycle_Operator");
			Position Backup = Save_Position();
			if (!Accept_Loop_Operator_With_Precondition())
			{
				Set_Position(Backup);
				return false;
			}
			Console.WriteLine("Done Accept_Cycle_Operator");
			return true;
		}

		public bool Accept_Loop_Operator_With_Precondition()
		{
			Console.WriteLine("EE Entered Accept_Loop_Operator_With_Precondition");
			if (Accept_Raw("while"))
			{
				if (!Accept_Logical_Expression()) Unhandled_Error("логическое выражение");

				if (!Accept_Raw("do")) Raise_Error(54);

				if (!Accept_Compound_Operator()) Unhandled_Error("оператор");

				if (!Accept_Raw(";")) Raise_Error(14);

				Console.WriteLine("Done Accept_Loop_Operator_With_Precondition");
				return true;
			}
			else return false;
		}

		public bool Accept_Cycle_Parameter()
		{
			Console.WriteLine("EE Entered Accept_Cycle_Parameter");
			if (!Accept_Variable_Name())
			{
				return false;
			}

			Console.WriteLine("Done Accept_Cycle_Parameter");
			return true;
		}

		public bool Accept_Variable_Name()
		{
			Console.WriteLine("EE Entered Accept_Variable_Name");
			if (!Accept_Name())
			{
				return false;
			}
			else
            {
				Console.WriteLine("Done Accept_Variable_Name");
				return true;
			}
		}

		public bool Accept_Type()
		{
			Console.WriteLine("EE Entered Accept_Type");
			if (!Accept_Type_Name())
			{
				return false;
			}
			else
            {
				Console.WriteLine("Done Accept_Type");
				return true;
			}
		}

        public bool Accept_Expression()
        {
			Console.WriteLine("EE Entered Accept_Expression");
			Position Backup = Save_Position();
            if (!Accept_Literal_Expression())
            {
				Set_Position(Backup);
				if(!Accept_Arithmetic_Expression())
                {
					Set_Position(Backup);
					if (!Accept_Logical_Expression())
                    {
						Set_Position(Backup);
						return false;
					}
				}
			}
			Console.WriteLine("Done Accept_Expression");
			return true;
		}

        public bool Accept_Variable()
        {
			Console.WriteLine("EE Entered Accept_Variable");
			Position Backup = Save_Position();

            if (!Accept_Variable_Name())
            {
                Set_Position(Backup);
				return false;
			}

			Console.WriteLine("Done Accept_Variable");
			return true;
		}

        

		public bool Accept_Arithmetic_Expression()
		{
			Console.WriteLine("EE Entered Accept_Arithmetic_Expression");
			if (Accept_Raw("+") || Accept_Raw("-"))
			{
				if (!Accept_Summand()) Unhandled_Error("слагаемое");

				while (Accept_Raw("+") || Accept_Raw("-"))
				{
					if (!Accept_Summand()) Unhandled_Error("слагаемое");
				}

				Console.WriteLine("Done Accept_Arithmetic_Expression");
				return true;
			}
			else
            {
				if (!Accept_Summand()) return false;

				while (Accept_Raw("+") || Accept_Raw("-"))
				{
					if (!Accept_Summand()) Unhandled_Error("слагаемое");
				}

				Console.WriteLine("Done Accept_Arithmetic_Expression");
				return true;
			}
		}

		public bool Accept_Summand()
		{
			Console.WriteLine("EE Entered Accept_Summand");
			if (Accept_Multiplier())
			{
				while (Accept_Raw("*") || Accept_Raw("/")
				|| Accept_Raw("mod") || Accept_Raw("div"))
				{
					if (!Accept_Multiplier()) Unhandled_Error("множитель");
				}
				Console.WriteLine("Done Accept_Summand");
				return true;
			}
			return false;
		}

		public bool Accept_Multiplier()
		{
			Console.WriteLine("EE Entered Accept_Multiplier");
			if (Accept_Raw("("))
			{
				if (!Accept_Arithmetic_Expression()) Unhandled_Error("Арифметическое выражение");

				if (!Accept_Raw(")")) Raise_Error(4);
				Console.WriteLine("Done Accept_Multiplier");
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
						//Проверка в семантике
						if (!Accept_Variable())
						{
							Set_Position(Backup);
							return false;
						}
					}
				}
				Console.WriteLine("Done Accept_Multiplier");
				return true;
			}

		}

		//Семантика на приводимость char и string
		public bool Accept_Literal_Expression()
		{
			Console.WriteLine("EE Entered Accept_Literal_Expression");
			if (!Accept_Literal_Summand()) return false;

			while (Accept_Raw("+"))
			{
				if (!Accept_Literal_Summand()) Unhandled_Error("слагаемое");
			}
			if (Accept_Raw("-") || Accept_Raw("*") || Accept_Raw("/") || Accept_Raw("div") || Accept_Raw("mod"))
            {
				Raise_Error(41);
            }

			Console.WriteLine("Done Accept_Literal_Expression");
			return true;
		}

		public bool Accept_Literal_Summand()
		{
			Console.WriteLine("EE Entered Accept_Literal_Summand");
			if (Accept_Literal_Multiplier())
			{
				Console.WriteLine("Done Accept_Literal_Summand");
				return true;
			}
			else return false;
		}

		public bool Accept_Literal_Multiplier()
        {
			Console.WriteLine("EE Entered Accept_Literal_Multiplier");
			if (Accept_Raw("("))
			{
				if (!Accept_Literal_Expression()) return false;

				if (!Accept_Raw(")")) Raise_Error(4);
				Console.WriteLine("Done Accept_Literal_Multiplier");
				return true;
			}
			else
			{
				Position Backup = Save_Position();
				if (!Accept_Literal_Const())
				{
					Set_Position(Backup);
					if (!Accept_String_Const())
					{
						Set_Position(Backup);
						//Проверка в семантике - строковая или литерная 
						if (!Accept_Variable())
						{
							Set_Position(Backup);
							return false;
						}
					}
				}
				Console.WriteLine("Done Accept_Multiplier");
				return true;
			}
		}


		public bool Accept_Actual_Parameter()
		{
			Console.WriteLine("EE Entered Accept_Actual_Parameter");
			Position Backup = Save_Position();
			if (!Accept_Expression())
			{
				Set_Position(Backup);
				if (!Accept_Variable())
				{
					Set_Position(Backup);
					return false;
				}
			}
			Console.WriteLine("Done Accept_Actual_Parameter");
			return true;
		}

		// Неправильно
		public bool Accept_Literal_Token()
		{
			Console.WriteLine("EE Entered Accept_Literal_Token");
			Position Backup = Save_Position();

			if (!Accept_Literal_Const())
			{
				Set_Position(Backup);

				if(!Accept_String_Const())
                {
					Set_Position(Backup);
					//Проверка в семантике
					if (!Accept_Variable())
					{
						Set_Position(Backup);
						return false;
					}
				}
			}
			Console.WriteLine("Done Accept_Literal_Expression");
			return true;
		}


		//Добавить проверку по таблице
		public bool Accept_Procedure_Name()
        {
			Console.WriteLine("EE Entered Accept_Procedure_Name");
			if (!Accept_Raw("writeln")) return false;
			else
			{
				Console.WriteLine("Done Accept_Procedure_Name");
				return true;
			}
		}

		//СЕМАНТИКА
		public bool Accept_Logical_Expression()
        {
			Console.WriteLine("EE Entered Accept_Logical_Expression");
			Position Backup = Save_Position();

            if (!Accept_Relationship())
            {
                Set_Position(Backup);

                if (!Accept_Simple_Logical_Expression())
                {
                    Set_Position(Backup);
                    return false;
                }
            }

			Console.WriteLine("Done Accept_Logical_Expression");
            return true;
        }

        public bool Accept_Simple_Logical_Expression()
        {
			Console.WriteLine("EE Entered Accept_Simple_Logical_Expression");
			if (!Accept_Logical_Summand()) return false;

			while (Accept_Raw("or"))
			{
                if (!Accept_Logical_Summand())
                {
                    Unhandled_Error("логическое слагаемое");
                }
            }

			Console.WriteLine("Done Accept_Simple_Logical_Expression");
            return true;
        }

        public bool Accept_Logical_Summand()
        {
			Console.WriteLine("EE Entered Accept_Logical_Summand");

			if (!Accept_Logical_Multiplier()) return false;

			while (Accept_Raw("and"))
			{
                if (!Accept_Logical_Multiplier())
                {
                    Unhandled_Error("логический множитель");
                }
            }

			Console.WriteLine("Done Accept_Logical_Summand");
            return true;
        }

        public bool Accept_Logical_Multiplier()
        {
			Console.WriteLine("EE Entered Accept_Logical_Multiplier");
			Position Backup = Save_Position();

            if (!Accept_True_False())
            {
                Set_Position(Backup);

				//Семантика
				if (!Accept_Variable())
                {
                    Set_Position(Backup);

					if (Accept_Raw("not"))
					{
						if (!Accept_Logical_Multiplier()) Unhandled_Error("логический множитель");

						Console.WriteLine("Done Accept_Logical_Multiplier");
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
			Console.WriteLine("Done Accept_Logical_Multiplier");
			return true;
        }

		//Все семантика...
        public bool Accept_Relationship()
        {
			Console.WriteLine("EE Entered Accept_Relationship");
			Position Backup = Save_Position();

            if (!Accept_Scalar_Relationship())
            {
                Set_Position(Backup);

                if (!Accept_String_Relastionship())
                {
                    Set_Position(Backup);
					return false;
				}
            }
			Console.WriteLine("Done Accept_Relationship");
			return true;
        }

        public bool Accept_Scalar_Relationship()
        {
			Console.WriteLine("EE Entered Accept_Scalar_Relationship");
			Position Backup = Save_Position();

            if (!Accept_Arithmetic_Expression())
            {
                Set_Position(Backup);

                if (!Accept_Literal_Token())
                {
                    Set_Position(Backup);

					if (!Accept_Simple_Logical_Expression())
					{
						Set_Position(Backup);
						return false;
					}
					else
					{
						if (!Accept_Comparison_Operation())
						{
							Set_Position(Backup);
							return false;
						}

						if (!Accept_Simple_Logical_Expression())
						{
							Unhandled_Error("простое логическое выражение");
						}
						Console.WriteLine("Done Accept_Scalar_Relationship");
						return true;
					}
				}
                else
                {
                    if (!Accept_Comparison_Operation())
                    {
                        Unhandled_Error("операция сравнения");
                    }

                    if (!Accept_Literal_Token())
                    {
                        Unhandled_Error("литерное выражение");
                    }
					Console.WriteLine("Done Accept_Scalar_Relationship");
					return true;
                }

            }
            else
            {
                if (!Accept_Comparison_Operation())
                {
                    Unhandled_Error("операция сравнения");
                }

                if (!Accept_Arithmetic_Expression())
                {
                    Unhandled_Error("арифметическое выражение");
                }
				Console.WriteLine("Done Accept_Scalar_Relationship");
				return true;
            }
        }

        public bool Accept_String_Relastionship()
        {
			Console.WriteLine("EE Entered Accept_String_Relastionship");
			Position Backup = Save_Position();

            if (!Accept_String_Const())
            {
                Set_Position(Backup);

				//Строкового типа
                if (!Accept_Variable())
                {
                    Set_Position(Backup);
                    return false;
                }
            }

            if (!Accept_Comparison_Operation())
            {
                Unhandled_Error("операция сравнения");
            }

            Position Backup_1 = Save_Position();

            if (!Accept_String_Const())
            {
                Set_Position(Backup_1);

				//Строкового типа
				if (!Accept_Variable())
                {
					Set_Position(Backup);
					return false;
				}
            }
			Console.WriteLine("Done Accept_String_Relastionship");
			return true;
        }

		public string Get_Class()
        {
			string raw_token_type = Current_Lexem.GetType().ToString();
			return raw_token_type.Substring(24);
		}

		public bool Accept_Comparison_Operation()
		{
			Console.WriteLine("EE Entered Accept_Comparison_Operaion");
			if (!Accept_Raw("=") && !Accept_Raw("<>") && !Accept_Raw("<=") && !Accept_Raw("<") && !Accept_Raw(">=") && !Accept_Raw(">"))
				return false;
			Console.WriteLine("Done Accept_Comparison_Operaion");
			return true;
		}

		//Тут надо подсасывать семантический
		public bool Accept_Real_Without_Sign()
		{
			Console.WriteLine("EE Entered Accept_Real_Without_Sign");
			if (Get_Class() != "Real")
			{
				return false;
			}
			else
            {
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_Real_Without_Sign");
				return true;
            }
		}

		//Тут надо подсасывать семантический
		public bool Accept_Unsigned_Integer()
		{
			Console.WriteLine("EE Entered Accept_Unsigned_Integer");
			if (Get_Class() != "Int")
				{
					return false;
				}
				else
				{
					Last_Lexem = Current_Lexem;
					NextSym();
					Console.WriteLine("Done Accept_Unsigned_Integer");
					return true;
				}
		}

		public bool Accept_Name()
		{
			Console.WriteLine("EE Entered Accept_Name");
			if (Get_Class() != "Id")
			{
				return false;
			}
			else
			{
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_Name");
				return true;
			}
		}


		public bool Accept_Type_Name()
		{
			Console.WriteLine("EE Entered Accept_Type_Name");
			if (Get_Class() != "Simple_Type")
			{
				return false;
			}
			else
			{
				NextSym();
				Console.WriteLine("Done Accept_Type_Name");
				return true;
			}
		}

		public bool Accept_String_Const()
        {
			Console.WriteLine("EE Entered Accept_String_Const");
			if (Get_Class() != "String")
			{
				return false;
			}
			else
			{
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_String_Const");
				return true;
			}
		}

		public bool Accept_Literal_Const()
		{
			Console.WriteLine("EE Entered Accept_Literal_Const");
			if (Get_Class() != "Char")
			{
				if (Get_Class() != "String")
				{
					return false;
				}
				else
				{
					Last_Lexem = Current_Lexem;
					NextSym();
					Console.WriteLine("Done Accept_Literal_Const");
					return true;
				}
			}
			else
			{
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_Literal_Const");
				return true;
			}
		}

		public bool Accept_True_False()
        {
			Console.WriteLine("EE Entered Accept_True_False");
			if (Get_Class() != "True_False")
			{
				
				return false;
			}
			else
			{
				if (Current_Lexem.value != "true" && Current_Lexem.value != "false")
				{
					return false;
				}
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_True_False");
				return true;
			}
		}
	}
}



