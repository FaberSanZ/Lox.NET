using System.Collections.Generic;
using System.Linq;

namespace Lox
{
    public abstract class ExpressionSyntax : SyntaxNode
    {


    }

    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, Token endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public Token EndOfFileToken { get; }

        //public static SyntaxTree Parse(string text)
        //{
        //    Parser parser = new Parser(text);
        //    return parser.Parse() /*new Parser(text).Parse()*/;
        //}
    }


    public class BinaryExpression : SyntaxNode
    {
        public SyntaxNode Left { get; }
        public SyntaxNode Right { get; }
        public Token Operator { get; }

        public BinaryExpression(SyntaxNode left, Token oper, SyntaxNode right)
        {
            Left = left;
            Operator = oper;
            Right = right;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return Operator;
            yield return Right;
        }
    }
}
