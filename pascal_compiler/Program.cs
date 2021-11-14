using System;
using InputOutput;
using LexicalAnalyzer;
using SyntaxAnalyzer;
using System.Collections.Generic;

namespace pascal_compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            // Путь к тексту программы 
            string path = @"C:\Users\Gearsy\Desktop\pascal_compiler\pascal_compiler\input.txt";

            //инициализация ввода-вывода
            IO Reader = new IO(path);

            //Инициализация лексического анализатора
            Lexical Lexical_Analyzer = new Lexical(Reader);
            Lexical_Analyzer.Errors = new List<string>();

            //Вывод полученных лексем
            Lexical_Analyzer.PrintLexem();

            //Вывод ошибок
            for (int i = 0; i < Lexical_Analyzer.Errors.Count; i++)
            {
                Console.WriteLine(Lexical_Analyzer.Errors[i]);
            }

            //Инициализация текущей лексемы
            Lexical.Lexem Current_Lexem = Lexical_Analyzer.NextSym();

            //Инициализация синтакического анализатора
            Syntaxic Syntacix_Analyer = new Syntaxic(Lexical_Analyzer, Reader, Current_Lexem);

            Console.WriteLine(Reader.Count);
            Console.WriteLine(Reader.ProgramText.Length);

            //Переместить "указатель" в начало файла
            Reader.Set_Start();
            

            Syntacix_Analyer.Accept_Program();

            
        }
    }
}
