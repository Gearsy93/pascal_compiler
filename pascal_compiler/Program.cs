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
            string path = @"C:\Users\Pists\OneDrive\Документы\7 Трим\Транслятор\Текущая версия\pascal_compiler\pascal_compiler\input.txt";

            //инициализация ввода-вывода
            IO Reader = new IO(path);

            //Инициализация лексического анализатора
            Lexical Lexical_Analyzer = new Lexical(Reader);

            //Вывод полученных лексем
            Lexical_Analyzer.PrintLexem();
            //Инициализация текущей лексемы
            

            //Переместить "указатель" в начало файла
            Reader.Set_Start();
            Lexical.Lexem Current_Lexem = Lexical_Analyzer.NextSym();

            //Инициализация синтакического анализатора
            Syntaxic Syntacix_Analyer = new Syntaxic(Lexical_Analyzer, Reader, Current_Lexem);


            
            

            Syntacix_Analyer.Accept_Program();

            
        }
    }
}
