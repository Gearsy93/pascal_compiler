using System;
using InputOutput;
using LexicalAnalyzer;

namespace pascal_compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string path = @"C:\Users\Gearsy\Source\Repos\Gearsy93\pascal_compiler\pascal_compiler\input.txt";
            IO Reader = new IO(path);
            Lexical Lexical_Analyzer = new Lexical(Reader);
            Lexical_Analyzer.PrintLexem();
        }
    }
}
