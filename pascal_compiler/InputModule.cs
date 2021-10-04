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

class IO
{
	//class Token
	public string ProgramText{ get; set; }
	
	public IO(string path)
	{
		System.Console.InputEncoding = enc1251
		using (StreamReader streamReader = new StreamReader(path))
		{
			ProgramText = streamReader.ReadToEnd();
		}
		Console.WriteLine(ProgramText);
	}


	
}
