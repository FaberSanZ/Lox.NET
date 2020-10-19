using System;
using System.IO;
using System.Linq;
using Lox;

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


        private static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            string marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if (node is Token t)
            {
                Console.Write(" ");
                Console.Write(t);
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            SyntaxNode lastChild = node.GetChildren().LastOrDefault();

            foreach (SyntaxNode child in node.GetChildren())
            {
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }
}
