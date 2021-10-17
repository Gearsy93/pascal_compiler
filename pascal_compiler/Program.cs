using System;
using InputOutput;
using LexicalAnalyzer;
using System.Collections.Generic;

namespace pascal_compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string path = @"C:\Users\Gearsy\Source\Repos\Gearsy93\pascal_compiler\pascal_compiler\input.txt";
            IO Reader = new IO(path);
            Lexical Lexical_Analyzer = new Lexical(Reader);
            Lexical_Analyzer.Errors = new List<string>();
            Lexical_Analyzer.PrintLexem();
            //Вывод ошибок
            for (int i = 0; i < Lexical_Analyzer.Errors.Count; i++)
            {
                Console.WriteLine(Lexical_Analyzer.Errors[i]);
            }
        }
    }
}
