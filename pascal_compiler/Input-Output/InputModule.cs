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
 * Перенести все нахуй в лексический анализатор
 */

//оbject
using System;
using System.IO;

namespace InputOutput
{
	public class IO
	{
		//class Token
		public string ProgramText { get; private set; }

		public int Line_Number { get; private set; }
		public int Line_Position { get; private set; }
		public int Last_Line_Number { get; private set; }
		public int Last_Line_Position { get; private set; }
		public int Count { get; private set; }



		public IO(string path)
		{
			using (StreamReader streamReader = new StreamReader(path))
			{
				ProgramText = streamReader.ReadToEnd();
			}
			Line_Number = 0;
			Line_Position = 0;
			Count = 0;
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
			return symbol;
		}

		public void Back()
        {
			Line_Position = Last_Line_Position;
			Line_Number = Last_Line_Number;
			Count -= 1;
        }
	}
}
