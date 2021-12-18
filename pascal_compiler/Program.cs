using System;
using InputOutput;
using LexicalAnalyzer;
using SyntaxAnalyzer;
using SemanticAnalyzer;
using System.Collections.Generic;

namespace pascal_compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            // Путь к тексту программы 
            string path = @"C:\Users\Pists\OneDrive\Документы\7 Трим\Транслятор\Текущая версия\pascal_compiler\pascal_compiler\input.txt";


            //инициализация ввода-вывода
            IO Reader = new IO(path);

            //while (Reader.Count < Reader.ProgramText.Length)
            //{
            //    Console.WriteLine("Value: " + Reader.Nextch() + "| Position: " + Reader.Line_Number + "| Line: " + (Reader.Line_Position + 1) + "| Count: " + Reader.Count);
            //}

            ////Инициализация лексического анализатора
            Lexical Lexical_Analyzer = new Lexical(Reader);

            ////Вывод полученных лексем
            Lexical_Analyzer.PrintLexem();


            //Переместить "указатель" в начало файла
            Reader.Set_Start();
            Lexical.Lexem Current_Lexem = Lexical_Analyzer.NextSym();

            //Инициализация семантического анализатора
            Semantic Semantic_Analyzer = new Semantic();

            ////Инициализация синтакического анализатора
            Syntaxic Syntacix_Analyer = new Syntaxic(Lexical_Analyzer, Reader, Current_Lexem, Semantic_Analyzer);

            Syntacix_Analyer.Accept_Program();


        }
    }
}
