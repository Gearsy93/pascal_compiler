/* Получает исходный код программы и преобразует в последовательность литер
 * Вход: Исходная программа
 * Выход: Последовательность литер, листинг
 */

/*TODO:
 * Бесконечный цикл получения токена
 * ДОбавить получение файла
 * Добавить номера строк, позиции в строке
 * 
 * Добавить тип токена - латиница, кирилица, операции, другие символы
 */

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace InputOutput
{
	public class IO
	{
		public string ProgramText { get; set; }

		public bool EndOfFile { get; set; }

		public int Line_Number { get; set; }
		public int Line_Position { get; set; }
		public int Last_Line_Number { get; set; }
		public int Last_Line_Position { get; set; }
		public int Count { get; set; }

	public IO(string path)
		{
            using (StreamReader streamReader = new StreamReader(path))
            {
                ProgramText = streamReader.ReadToEnd();				
			}
            Line_Number = 0;
            Line_Position = 0;
			Count = 0;
			EndOfFile = false;
		}

		public char Nextch()
        {
			Last_Line_Number = Line_Number;
			Last_Line_Position = Line_Position;
			char symbol = ProgramText[Count];
			if (symbol == '\n')
			{
				Line_Number += 1;
				Line_Position = 0;
			}
			else Line_Position += 1;
			Count += 1;

			if (Count == ProgramText.Length) EndOfFile = true;

			return symbol;
		}

		public void Set_Start()
        {
			Line_Number = 0;
			Line_Position = 0;
			Last_Line_Number = 0;
			Last_Line_Position = 0;
			Count = 0;
			EndOfFile = false;

		}

		public void Back()
        {
			Line_Position = Last_Line_Position;
			Line_Number = Last_Line_Number;
			Count -= 1;
			EndOfFile = false;

		}

		public void Return_To_State()
        {

        }
	}
}
