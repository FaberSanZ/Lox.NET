using System;
using System.Collections.Generic;
using System.IO;

namespace Lox
{
    public abstract class SyntaxNode
    {
        //SyntaxKind Kind { get; }
        public abstract SyntaxKind Kind { get; }



        public abstract IEnumerable<SyntaxNode> GetChildren();


        //private static void PrettyPrint(TextWriter writer, SyntaxNode node, string indent = "", bool isLast = true)
        //{
        //    var isToConsole = writer == Console.Out;
        //    var token = node as Token;

        //    if (token != null)
        //    {
        //        foreach (var trivia in token.LeadingTrivia)
        //        {
        //            if (isToConsole)
        //                Console.ForegroundColor = ConsoleColor.DarkGray;

        //            writer.Write(indent);
        //            writer.Write("├──");

        //            if (isToConsole)
        //                Console.ForegroundColor = ConsoleColor.DarkGreen;

        //            writer.WriteLine($"L: {trivia.Kind}");
        //        }
        //    }

        //    var hasTrailingTrivia = token != null && token.TrailingTrivia.Any();
        //    var tokenMarker = !hasTrailingTrivia && isLast ? "└──" : "├──";

        //    if (isToConsole)
        //        Console.ForegroundColor = ConsoleColor.DarkGray;

        //    writer.Write(indent);
        //    writer.Write(tokenMarker);

        //    if (isToConsole)
        //        Console.ForegroundColor = node is SyntaxToken ? ConsoleColor.Blue : ConsoleColor.Cyan;

        //    writer.Write(node.Kind);

        //    if (token != null && token.Value != null)
        //    {
        //        writer.Write(" ");
        //        writer.Write(token.Value);
        //    }

        //    if (isToConsole)
        //        Console.ResetColor();

        //    writer.WriteLine();

        //    if (token != null)
        //    {
        //        foreach (var trivia in token.TrailingTrivia)
        //        {
        //            var isLastTrailingTrivia = trivia == token.TrailingTrivia.Last();
        //            var triviaMarker = isLast && isLastTrailingTrivia ? "└──" : "├──";

        //            if (isToConsole)
        //                Console.ForegroundColor = ConsoleColor.DarkGray;

        //            writer.Write(indent);
        //            writer.Write(triviaMarker);

        //            if (isToConsole)
        //                Console.ForegroundColor = ConsoleColor.DarkGreen;

        //            writer.WriteLine($"T: {trivia.Kind}");
        //        }
        //    }

        //    indent += isLast ? "   " : "│  ";

        //    var lastChild = node.GetChildren().LastOrDefault();

        //    foreach (var child in node.GetChildren())
        //        PrettyPrint(writer, child, indent, child == lastChild);
        //}
    }
}
