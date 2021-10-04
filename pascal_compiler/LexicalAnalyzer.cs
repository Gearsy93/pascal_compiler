/* Осуществляет лекcический анализ, строит идентификаторы, ключевые слова, разделители, числа
 * Вход: последовательность литер
 * Выход: лексически проверенная программа, лексические ошибки
 */

using System;

public class LexicalAnalyzer
{
	enum TokenState
	{
		id,
		number,
		operation,
		сonst, //int, float, string - object
		keyword, //ключевые слова if while 
		other_symbol
	}

	public int CurrentToken { get; private set; }
	public class Token
	{
		public int Position { get; private set; }
		public int Line { get; private set; }


	}
	public static void GetNextToken()
	{

	}

}
