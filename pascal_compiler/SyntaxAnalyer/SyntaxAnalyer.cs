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
using SemanticAnalyzer;
using System.Collections.Generic;

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

		public Semantic Semantic_Analyzer;

		public string got;

		public Lexical.Lexem Last_Lexem;

		public IO Reader { get; }
		public Lexical.Lexem Current_Lexem;
		public Syntaxic(Lexical Lexical_Analyzer, IO Reader, Lexical.Lexem Current_Lexem, Semantic Semantic_Analyzer)
		{
			this.Lexical_Analyzer = Lexical_Analyzer;
			this.Reader = Reader;
			this.Current_Lexem = Current_Lexem;
			this.Semantic_Analyzer = Semantic_Analyzer;
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
			if (!Accept_Name().Item1) Raise_Error(2);
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
					if (Accept_Actual_Parameter().Item1)
                    {
						while (Accept_Raw(","))
                        {
							if (!Accept_Actual_Parameter().Item1)
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
			List<string> Ids = new List<string>();
			Tuple<bool, Lexical.Lexem> temp = Accept_Variable_Name();
			Tuple<bool, string> Bool_EType = new Tuple<bool, string>(temp.Item1, temp.Item2.value);
			if (!Bool_EType.Item1)
			{
				return false;
			}

			Ids.Add(Bool_EType.Item2);

			while (Accept_Raw(","))
			{
				temp = Accept_Variable_Name();
				Bool_EType = new Tuple<bool, string>(temp.Item1, temp.Item2.value); ;
				if (!Bool_EType.Item1)
                {
					Unhandled_Error("имя переменной");
				}
				Ids.Add(Bool_EType.Item2);
			} 

            if (!Accept_Raw(":")) Raise_Error(5);

			Bool_EType = Accept_Type();

			if (!Bool_EType.Item1)
            {
                Unhandled_Error("тип");
            }

			foreach(string id in Ids)
            {
				if (!Semantic_Analyzer.AddIdent(id, Bool_EType.Item2)) Raise_Error(101);
			}

			Console.WriteLine("Done Accept_Variables_Description");
			return true;
		}

        public bool Accept_Assignment_Operator()
        {
			Console.WriteLine("EE Entered Accept_Assignment_Operator");

			Tuple<bool, Lexical.Lexem> temp = Accept_Variable_Name();
			Tuple<bool, string> Id = new Tuple<bool, string>(temp.Item1, Lexical_Analyzer.GetStringTypeLexel(temp.Item2));
			if (Id.Item1)
			{
				if (!Accept_Raw(":=")) Raise_Error(51);

				Tuple<bool, string> Expression = Accept_Expression();
				if (!Expression.Item1)
				{
					Raise_Error(310);
				}

				EType ID_Type = EType.NotDerivable;
				bool TableHas = false;
				foreach(var e in Semantic_Analyzer.Types_Table)
                {
					if (e.Key == temp.Item2.value)
                    {
						Console.WriteLine("Found key in dict");
						ID_Type = e.Value;
						TableHas = true;

					}
					
				}
				if (!TableHas) Raise_Error(309);
				Console.WriteLine("Left1: " + ID_Type + " Right1: " + Expression.Item2);
				if (Semantic_Analyzer.IsAssignable(ID_Type, Semantic_Analyzer.ConvertToType(Expression.Item2)) == EType.NotDerivable)
                {
					Raise_Error(128);
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
				if (!Accept_Logical_Expression().Item1)
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
				if (!Accept_Logical_Expression().Item1) Unhandled_Error("логическое выражение");

				if (!Accept_Raw("do")) Raise_Error(54);

				if (!Accept_Compound_Operator()) Unhandled_Error("оператор");

				if (!Accept_Raw(";")) Raise_Error(14);

				Console.WriteLine("Done Accept_Loop_Operator_With_Precondition");
				return true;
			}
			else return false;
		}

		public Tuple<bool, string> Accept_Cycle_Parameter()
		{
			Console.WriteLine("EE Entered Accept_Cycle_Parameter");
			Tuple<bool, Lexical.Lexem> temp = Accept_Variable_Name();
			Tuple<bool, string> Bool_EType = new Tuple<bool, string>(temp.Item1, Lexical_Analyzer.GetStringTypeLexel(temp.Item2));
			if (!Bool_EType.Item1)
			{
				return Bool_EType;
			}

			Console.WriteLine("Done Accept_Cycle_Parameter");
			return Bool_EType;
		}

		public Tuple<bool, Lexical.Lexem> Accept_Variable_Name()
		{
			Console.WriteLine("EE Entered Accept_Variable_Name");
			Tuple<bool, Lexical.Lexem> Bool_EType = Accept_Name();
			if (!Bool_EType.Item1)
			{
				return Bool_EType;
			}
			else
            {
				Console.WriteLine("Done Accept_Variable_Name");

				return Bool_EType;
			}
		}

		public Tuple<bool, string> Accept_Type()
		{
			Console.WriteLine("EE Entered Accept_Type");
			Tuple<bool, string> Bool_EType = Accept_Type_Name();
			if (!Bool_EType.Item1)
			{
				return Bool_EType;
			}
			else
            {
				Console.WriteLine("Done Accept_Type");
				return Bool_EType;
			}
		}

        public Tuple<bool, string> Accept_Expression()
        {
			Console.WriteLine("EE Entered Accept_Expression");
			Position Backup = Save_Position();
			Tuple<bool, string> Bool_EType = Accept_Literal_Expression();
			if (!Bool_EType.Item1)
            {
				Set_Position(Backup);
				Bool_EType = Accept_Arithmetic_Expression();
				if (!Bool_EType.Item1)
                {
					Set_Position(Backup);
					Bool_EType = Accept_Logical_Expression();
					if (!Bool_EType.Item1)
                    {
						Set_Position(Backup);
						return Bool_EType;
					}
				}
			}
			Console.WriteLine("Done Accept_Expression");
			return Bool_EType;
		}

		//А ТУТ ПОСЛОЖНЕЕ ЧТО ЛМ
		public Tuple<bool, string> Accept_Arithmetic_Expression()
		{
			Tuple<bool, string> Bool_EType;
			Tuple<bool, string> Bool_EType_2;
			Console.WriteLine("EE Entered Accept_Arithmetic_Expression");
			EType derividedtype;
			if (Accept_Raw("+") || Accept_Raw("-"))
			{
				Bool_EType = Accept_Summand();
				if (Bool_EType.Item2.ToLower() != "integer" && Bool_EType.Item2.ToLower() != "real")
				{
					return new Tuple<bool, string>(false, "");

				}
				if (!Bool_EType.Item1) Unhandled_Error("слагаемое");

				while (Accept_Raw("+") || Accept_Raw("-"))
				{
					Bool_EType_2 = Accept_Summand();
					Console.WriteLine(Bool_EType_2.Item2.ToLower());
					if (Bool_EType_2.Item2.ToLower() != "integer" && Bool_EType_2.Item2.ToLower() != "real")
					{
						return new Tuple<bool, string>(false, "");

					}
					if (!Bool_EType_2.Item1) Unhandled_Error("слагаемое");
					Console.WriteLine("First: " + Bool_EType.Item2 + " Second: " + Bool_EType_2.Item2); 
					derividedtype = Semantic_Analyzer.IsArithmeticDerivided(Semantic_Analyzer.ConvertToType(Bool_EType.Item2), Semantic_Analyzer.ConvertToType(Bool_EType_2.Item2));
					if (derividedtype == EType.NotDerivable) Raise_Error(145);
					else Bool_EType = new Tuple<bool, string>(true, Semantic_Analyzer.ConvertToString(derividedtype));
				}

				Console.WriteLine("Done Accept_Arithmetic_Expression");
				return Bool_EType;
			}
			else
            {
				Bool_EType = Accept_Summand();
				Console.WriteLine(Bool_EType.Item2.ToLower());
				if (!Bool_EType.Item1) return Bool_EType;
				if (Bool_EType.Item2.ToLower() != "integer" && Bool_EType.Item2.ToLower() != "real")
				{
					return new Tuple<bool, string>(false, "");

				}
				while (Accept_Raw("+") || Accept_Raw("-"))
				{
					Bool_EType_2 = Accept_Summand();
					Console.WriteLine(Bool_EType_2.Item2.ToLower());
					if (!Bool_EType_2.Item1) Unhandled_Error("слагаемое");
					if (Bool_EType_2.Item2.ToLower() != "integer" && Bool_EType_2.Item2.ToLower() != "real")
					{
						return new Tuple<bool, string>(false, "");

					}
					
					Console.WriteLine("First: " + Bool_EType.Item2 + " Second: " + Bool_EType_2.Item2);        
					derividedtype = Semantic_Analyzer.IsArithmeticDerivided(Semantic_Analyzer.ConvertToType(Bool_EType.Item2), Semantic_Analyzer.ConvertToType(Bool_EType_2.Item2));
					if (derividedtype == EType.NotDerivable) Raise_Error(145);
					else Bool_EType = new Tuple<bool, string>(true, Semantic_Analyzer.ConvertToString(derividedtype));
				}

				Console.WriteLine("Done Accept_Arithmetic_Expression");
				return Bool_EType;
			}
		}

		public Tuple<bool, string> Accept_Summand()
		{
			Console.WriteLine("EE Entered Accept_Summand");
			Tuple<bool, string> Bool_EType = Accept_Multiplier();
			Tuple<bool, string> Bool_EType_2;
			EType derividedtype;
			if (Bool_EType.Item1)
			{
				while (Accept_Raw("*") || Accept_Raw("/")
				|| Accept_Raw("mod") || Accept_Raw("div"))
				{
					Bool_EType_2 = Accept_Multiplier();
					if (!Bool_EType.Item1) Unhandled_Error("множитель");
					derividedtype = Semantic_Analyzer.IsArithmeticDerivided(Semantic_Analyzer.ConvertToType(Bool_EType.Item2), Semantic_Analyzer.ConvertToType(Bool_EType_2.Item2));
					if (derividedtype == EType.NotDerivable) Raise_Error(145);
					else Bool_EType = new Tuple<bool, string>(true, Semantic_Analyzer.ConvertToString(derividedtype));
				}
				Console.WriteLine("Done Accept_Summand");
				return Bool_EType;
			}
			return Bool_EType;
		}

		public Tuple<bool, string> Accept_Multiplier()
		{
			Console.WriteLine("EE Entered Accept_Multiplier");
			Tuple<bool, string> Bool_EType;
			if (Accept_Raw("("))
			{
				Bool_EType = Accept_Arithmetic_Expression();
				if (!Bool_EType.Item1) Unhandled_Error("Арифметическое выражение");

				if (!Accept_Raw(")")) Raise_Error(4);
				Console.WriteLine("Done Accept_Multiplier");
				return Bool_EType;
			}
			else
			{
				Position Backup = Save_Position();

				Bool_EType = Accept_Real_Without_Sign();
				if (!Bool_EType.Item1)
				{
					Set_Position(Backup);
					Bool_EType = Accept_Unsigned_Integer();
					if (!Bool_EType.Item1)
					{
						Set_Position(Backup);
						//Проверка в семантике
						Bool_EType = Accept_Variable();
						if (!Bool_EType.Item1)
						{
							Set_Position(Backup);
							return Bool_EType;
						}
					}
				}
				Console.WriteLine("Done Accept_Multiplier");
				return Bool_EType;
			}

		}

		//ТУТ ТОЖЕ НАДО ВСТАВИТЬ МНОЖЕСТВЕННУЮ ПРОВЕРКУ
		public Tuple<bool, string> Accept_Literal_Expression()
		{

			Console.WriteLine("EE Entered Accept_Literal_Expression");

			Tuple<bool, string> Bool_EType = Accept_Literal_Summand();
			if (Bool_EType.Item2.ToLower() != "string" && Bool_EType.Item2.ToLower() != "char")
            {
				return new Tuple<bool, string>(false, "");

			}
			if (!Bool_EType.Item1) return Bool_EType;
			EType derividedtype;
			Tuple<bool, string> Bool_EType_2;

			while (Accept_Raw("+"))
			{
				Bool_EType_2 = Accept_Literal_Summand();
				if (Bool_EType_2.Item2.ToLower() != "string" && Bool_EType_2.Item2.ToLower() != "char")
				{
					return new Tuple<bool, string>(false, "");

				}
				if (!Bool_EType_2.Item1) Unhandled_Error("слагаемое");
				derividedtype = Semantic_Analyzer.IsArithmeticDerivided(Semantic_Analyzer.ConvertToType(Bool_EType.Item2), Semantic_Analyzer.ConvertToType(Bool_EType_2.Item2));
					if (derividedtype == EType.NotDerivable) Raise_Error(145);
					else Bool_EType = new Tuple<bool, string>(true, Semantic_Analyzer.ConvertToString(derividedtype));
			}
			if (Accept_Raw("-") || Accept_Raw("*") || Accept_Raw("/") || Accept_Raw("div") || Accept_Raw("mod"))
            {
				Raise_Error(41);
            }

			Console.WriteLine("Done Accept_Literal_Expression");
			return Bool_EType;
		}

		public Tuple<bool, string> Accept_Literal_Summand()
		{
			Console.WriteLine("EE Entered Accept_Literal_Summand");
			Tuple<bool, string> Bool_EType = Accept_Literal_Multiplier();
			if (Bool_EType.Item1)
			{
				Console.WriteLine("Done Accept_Literal_Summand");
				return Bool_EType;
			}
			else return Bool_EType;
		}

		public Tuple<bool, string> Accept_Literal_Multiplier()
        {
			Console.WriteLine("EE Entered Accept_Literal_Multiplier");
			Tuple<bool, string> Bool_EType;
			if (Accept_Raw("("))
			{
				Bool_EType = Accept_Literal_Expression();
				if (!Bool_EType.Item1) return Bool_EType;

				if (!Accept_Raw(")")) Raise_Error(4);
				Console.WriteLine("Done Accept_Literal_Multiplier");
				return Bool_EType;
			}
			else
			{
				Position Backup = Save_Position();

				Bool_EType = Accept_Literal_Const();
				if (!Bool_EType.Item1)
				{
					Set_Position(Backup);
					Bool_EType = Accept_String_Const();
					if (!Bool_EType.Item1)
					{
						Set_Position(Backup);
						Bool_EType = Accept_Variable();
						if (!Bool_EType.Item1)
						{
							Set_Position(Backup);
							return Bool_EType;
						}
					}
				}
				Console.WriteLine("Done Accept_Multiplier");
				return Bool_EType;
			}
		}


		public Tuple<bool, string> Accept_Actual_Parameter()
		{
			Console.WriteLine("EE Entered Accept_Actual_Parameter");
			Position Backup = Save_Position();

			Tuple<bool, string> Bool_EType = Accept_Expression();
			if (!Bool_EType.Item1)
			{
				Set_Position(Backup);
				Bool_EType = Accept_Variable();
				if (!Bool_EType.Item1)
				{
					Set_Position(Backup);
					return Bool_EType;
				}
			}
			Console.WriteLine("Done Accept_Actual_Parameter");
			return Bool_EType;
		}

		// Неправильно?
		public Tuple<bool, string> Accept_Literal_Token()
		{
			Console.WriteLine("EE Entered Accept_Literal_Token");
			Position Backup = Save_Position();

			Tuple<bool, string> Bool_EType = Accept_Literal_Const();

			if (!Bool_EType.Item1)
			{
				Set_Position(Backup);

				Bool_EType = Accept_String_Const();
				if (!Bool_EType.Item1)
                {
					Set_Position(Backup);
					Bool_EType = Accept_Variable();
					if (!Bool_EType.Item1)
					{
						Set_Position(Backup);
						return Bool_EType;
					}
				}
			}
			Console.WriteLine("Done Accept_Literal_Expression");
			return Bool_EType;
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
		public Tuple<bool, string> Accept_Logical_Expression()
        {
			Console.WriteLine("EE Entered Accept_Logical_Expression");
			Position Backup = Save_Position();

			Tuple<bool, string> Bool_EType = Accept_Relationship();


			if (!Bool_EType.Item1)
            {
                Set_Position(Backup);

				Bool_EType = Accept_Simple_Logical_Expression();

				if (!Bool_EType.Item1)
                {
                    Set_Position(Backup);
                    return Bool_EType;
                }
            }

			Console.WriteLine("Done Accept_Logical_Expression");
            return Bool_EType;
        }

        public Tuple<bool, string> Accept_Simple_Logical_Expression()
        {
			Console.WriteLine("EE Entered Accept_Simple_Logical_Expression");

			Tuple<bool, string> Bool_EType = Accept_Logical_Summand();

			if (!Bool_EType.Item1) return Bool_EType;

			while (Accept_Raw("or"))
			{
				Bool_EType = Accept_Logical_Summand();
				if (!Bool_EType.Item1)
                {
                    Unhandled_Error("логическое слагаемое");
                }
            }

			Console.WriteLine("Done Accept_Simple_Logical_Expression");
            return Bool_EType;
        }

        public Tuple<bool, string> Accept_Logical_Summand()
        {
			Console.WriteLine("EE Entered Accept_Logical_Summand");

			Tuple<bool, string> Bool_EType = Accept_Logical_Multiplier();

			if (!Bool_EType.Item1) return Bool_EType;

			while (Accept_Raw("and"))
			{
				Bool_EType = Accept_Logical_Multiplier();

				if (!Bool_EType.Item1)
                {
                    Unhandled_Error("логический множитель");
                }
            }

			Console.WriteLine("Done Accept_Logical_Summand");
            return Bool_EType;
        }

        public Tuple<bool, string> Accept_Logical_Multiplier()
        {
			Console.WriteLine("EE Entered Accept_Logical_Multiplier");
			Position Backup = Save_Position();

			Tuple<bool, string> Bool_EType = Accept_True_False();


			if (!Bool_EType.Item1)
            {
                Set_Position(Backup);

				Bool_EType = Accept_Variable();
				if (!Bool_EType.Item1)
                {
                    Set_Position(Backup);

					if (Accept_Raw("not"))
					{
						Bool_EType = Accept_Logical_Multiplier();
						if (!Bool_EType.Item1) Unhandled_Error("логический множитель");

						Console.WriteLine("Done Accept_Logical_Multiplier");
						return Bool_EType;
					}

					Set_Position(Backup);

					if (Accept_Raw("("))
					{
						Bool_EType = Accept_Logical_Expression();
						if (!Bool_EType.Item1)
						{
							Unhandled_Error("логическое выражение");
						}

						if (!Accept_Raw(")")) Raise_Error(4);
						Console.WriteLine("Done Accept_Logical_Multiplier");
						return Bool_EType;
					}
					else
					{
						Set_Position(Backup);
						return Bool_EType;
					}
				}
            }
			Console.WriteLine("Done Accept_Logical_Multiplier");
			return Bool_EType;
        }

		//Все семантика...
        public Tuple<bool, string> Accept_Relationship()
        {
			Console.WriteLine("EE Entered Accept_Relationship");
			Position Backup = Save_Position();

			Tuple<bool, string> Bool_EType = Accept_Scalar_Relationship();


			if (!Bool_EType.Item1)
            {
                Set_Position(Backup);

				Bool_EType = Accept_String_Relastionship();

				if (!Bool_EType.Item1)
                {
                    Set_Position(Backup);
					return Bool_EType;
				}
            }
			Console.WriteLine("Done Accept_Relationship");
			return Bool_EType;
        }

        public Tuple<bool, string> Accept_Scalar_Relationship()
        {
			Console.WriteLine("EE Entered Accept_Scalar_Relationship");
			Position Backup = Save_Position();

			Tuple<bool, string> Bool_EType = Accept_Arithmetic_Expression();
			Tuple<bool, string> Bool_EType_2;

			if (!Bool_EType.Item1)
            {
                Set_Position(Backup);

				Bool_EType = Accept_Literal_Token();

				if (!Bool_EType.Item1)
                {
                    Set_Position(Backup);

					Bool_EType = Accept_Simple_Logical_Expression();

					if (!Bool_EType.Item1)
					{
						Set_Position(Backup);
						return Bool_EType;
					}
					else
					{
						if (!Accept_Comparison_Operation())
						{
							Set_Position(Backup);
							return new Tuple<bool, string>(false, "");
						}

						Bool_EType_2 = Accept_Simple_Logical_Expression();
						if (!Bool_EType_2.Item1)
						{
							Unhandled_Error("простое логическое выражение");
						}
						

						if(!Semantic_Analyzer.isComparable(Semantic_Analyzer.ConvertToType(Bool_EType.Item2), Semantic_Analyzer.ConvertToType(Bool_EType_2.Item2)))
                        {
							Raise_Error(145);
							return new Tuple<bool, string>(false, "");
						}
						Console.WriteLine("Done Accept_Scalar_Relationship");
						return new Tuple<bool, string>(true, "Boolean");
					}
				}
                else
                {
                    if (!Accept_Comparison_Operation())
                    {
						Set_Position(Backup);
						return new Tuple<bool, string>(false, "");
					}

					Bool_EType_2 = Accept_Literal_Token();

					if (!Bool_EType_2.Item1)
                    {
                        Unhandled_Error("литерное выражение");
                    }

					if (!Semantic_Analyzer.isComparable(Semantic_Analyzer.ConvertToType(Bool_EType.Item2), Semantic_Analyzer.ConvertToType(Bool_EType_2.Item2)))
					{
						Raise_Error(145);
						return new Tuple<bool, string>(false, "");
					}

					Console.WriteLine("Done Accept_Scalar_Relationship");
					return new Tuple<bool, string>(true, "Boolean");
				}

            }
            else
            {
                if (!Accept_Comparison_Operation())
                {
					Set_Position(Backup);
					return new Tuple<bool, string>(false, "");
				}

				Bool_EType_2 = Accept_Arithmetic_Expression();


				if (!Bool_EType_2.Item1)
                {
                    Unhandled_Error("арифметическое выражение");
                }

				if (!Semantic_Analyzer.isComparable(Semantic_Analyzer.ConvertToType(Bool_EType.Item2), Semantic_Analyzer.ConvertToType(Bool_EType_2.Item2)))
				{
					Raise_Error(145);
					return new Tuple<bool, string>(false, "");
				}

				Console.WriteLine("Done Accept_Scalar_Relationship");
				return new Tuple<bool, string>(true, "Boolean");
			}
        }

        public Tuple<bool, string> Accept_String_Relastionship()
        {
			Console.WriteLine("EE Entered Accept_String_Relastionship");
			Position Backup = Save_Position();

			Tuple<bool, string> First_Bool_EType = Accept_String_Const();


			if (!First_Bool_EType.Item1)
            {
                Set_Position(Backup);
				First_Bool_EType = Accept_Variable();
				//Строкового типа
				if (!First_Bool_EType.Item1)
                {
                    Set_Position(Backup);
                    return First_Bool_EType;
                }
            }

            if (!Accept_Comparison_Operation())
            {
                Unhandled_Error("операция сравнения");
            }

            Position Backup_1 = Save_Position();

			Tuple<bool, string> Second_Bool_EType = Accept_String_Const();

			if (!Second_Bool_EType.Item1)
            {
                Set_Position(Backup_1);

				Second_Bool_EType = Accept_Variable();

				//Строкового типа
				if (!Second_Bool_EType.Item1)
                {
					Set_Position(Backup);
					return Second_Bool_EType;
				}
            }
			if (!Semantic_Analyzer.isComparable(Semantic_Analyzer.ConvertToType(First_Bool_EType.Item2), Semantic_Analyzer.ConvertToType(Second_Bool_EType.Item2)))
            {
				Raise_Error(145);
				return new Tuple<bool, string>(false, "");
            }
			Console.WriteLine("Done Accept_String_Relastionship");
			return new Tuple<bool, string>(true, "Boolean");
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

		public Tuple<bool, string> Accept_Variable()
		{
			Console.WriteLine("EE Entered Accept_Variable");
			Position Backup = Save_Position();
			Tuple<bool, Lexical.Lexem> temp = Accept_Variable_Name();
			Tuple<bool, String> Bool_EType;
			EType Type = EType.NotDerivable;
			bool TableHas = false;
			if (Lexical_Analyzer.GetStringTypeLexel(temp.Item2) == "id")
            {
				foreach (var e in Semantic_Analyzer.Types_Table)
				{
					if (e.Key == temp.Item2.value)
					{
						Type = e.Value;
						TableHas = true;
					}
				}
				if (!TableHas) Raise_Error(309);
;				Bool_EType = new Tuple<bool, string>(temp.Item1, Semantic_Analyzer.ConvertToString(Type));

			}
            else
            {
				Bool_EType = new Tuple<bool, string>(temp.Item1, temp.Item2.GetType().ToString().ToLower().Substring(24));
			}

			if (!temp.Item1)
			{
				Set_Position(Backup);
				return Bool_EType;
			}

			Console.WriteLine("Done Accept_Variable");
			return Bool_EType;
		}

		//Тут надо подсасывать семантический
		public Tuple<bool, string> Accept_Real_Without_Sign()
		{
			Console.WriteLine("EE Entered Accept_Real_Without_Sign");
			if (Get_Class() != "Real")
			{
				return new Tuple<bool, string>(false, "");
			}
			else
            {
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_Real_Without_Sign");
				return new Tuple<bool, string>(true, Last_Lexem.GetType().ToString().ToLower().Substring(24));
			}
		}

		//Тут надо подсасывать семантический
		public Tuple<bool, string> Accept_Unsigned_Integer()
		{
			Console.WriteLine("EE Entered Accept_Unsigned_Integer");
			if (Get_Class() != "Integer")
				{
					return new Tuple<bool, string>(false, "");
			}
				else
				{
					Last_Lexem = Current_Lexem;
					NextSym();
					Console.WriteLine("Done Accept_Unsigned_Integer");
					return new Tuple<bool, string>(true, Last_Lexem.GetType().ToString().ToLower().Substring(24));
			}
		}

		public Tuple<bool, Lexical.Lexem> Accept_Name()
		{
			Console.WriteLine("EE Entered Accept_Name");
			if (Get_Class() != "Id")
			{
				return new Tuple<bool, Lexical.Lexem>(false, Current_Lexem);
			}
			else
			{
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_Name");
				return new Tuple<bool, Lexical.Lexem>(true, Last_Lexem);
			}
		}


		public Tuple<bool, string> Accept_Type_Name()
		{
			Console.WriteLine("EE Entered Accept_Type_Name");
			if (Get_Class() != "Simple_Type")
			{
				return new Tuple<bool, string>(false, "");
			}
			else
			{
				string type = Current_Lexem.value;
				NextSym();
				Console.WriteLine("Done Accept_Type_Name");
				return new Tuple<bool, string>(true, type);
			}
		}

		public Tuple<bool, string> Accept_String_Const()
        {
			Console.WriteLine("EE Entered Accept_String_Const");
			if (Get_Class() != "String")
			{
				return new Tuple<bool, string>(false, "");
			}
			else
			{
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_String_Const");
				return new Tuple<bool, string>(true, Last_Lexem.GetType().ToString().ToLower().Substring(24));
			}
		}

		public Tuple<bool, string> Accept_Literal_Const()
		{
			Console.WriteLine("EE Entered Accept_Literal_Const");
			if (Get_Class() != "Char")
			{
				if (Get_Class() != "String")
				{
					return new Tuple<bool, string>(false, "");
				}
				else
				{
					Last_Lexem = Current_Lexem;
					NextSym();
					Console.WriteLine("Done Accept_Literal_Const");
					return new Tuple<bool, string>(true, Last_Lexem.GetType().ToString().ToLower().Substring(24));
				}
			}
			else
			{
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_Literal_Const");
				return new Tuple<bool, string>(true, Last_Lexem.GetType().ToString().ToLower().Substring(24));
			}
		}

		public Tuple<bool, string> Accept_True_False()
        {
			Console.WriteLine("EE Entered Accept_True_False");
			if (Get_Class() != "Boolean")
			{
				return new Tuple<bool, string>(false, "");
			}
			else
			{
				if (Current_Lexem.value != "true" && Current_Lexem.value != "false")
				{
					return new Tuple<bool, string>(false, "");
				}
				Last_Lexem = Current_Lexem;
				NextSym();
				Console.WriteLine("Done Accept_True_False");
				return new Tuple<bool, string>(true, Last_Lexem.GetType().ToString().ToLower().Substring(24));
			}
		}
	}
}



