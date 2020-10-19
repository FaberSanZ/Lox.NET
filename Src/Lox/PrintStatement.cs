using System.Collections.Generic;

namespace Lox
{
    public class PrintStatement : SyntaxNode
    {
        public SyntaxNode Expression { get; }

        public override SyntaxKind Kind => SyntaxKind.PrintStatement;

        public PrintStatement(SyntaxNode expression)
        {
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
        }
    }
}
