using System;
using System.IO;

namespace Lox.TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string source = File.ReadAllText(@"Test\Program.lox");
            LoxInterpreter interpreter = new LoxInterpreter();
            interpreter.Run(source);
        }
    }

}
