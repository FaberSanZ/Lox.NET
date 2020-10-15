using System.IO;

namespace Lox.TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool main = true;
            string source = File.ReadAllText(@"Test\Program.lox");
            if (main) source += "var init = Program(); init.Main();";
            LoxInterpreter interpreter = new LoxInterpreter();
            interpreter.Run(source);
        }
    }
}
