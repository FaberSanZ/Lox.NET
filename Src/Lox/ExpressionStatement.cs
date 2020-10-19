using System.Collections.Generic;

namespace Lox
{
    class ExpressionStatement : SyntaxNode
    {
        public SyntaxNode Expression { get; }
        public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;

        public ExpressionStatement(SyntaxNode expression)
        {
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
