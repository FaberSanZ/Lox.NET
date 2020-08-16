using System.Collections.Generic;

namespace Lox
{
    class BlockStatement : SyntaxNode
    {
        public List<SyntaxNode> Statements { get; }
        public SyntaxKind Kind => SyntaxKind.BlockStatement;

        public BlockStatement(List<SyntaxNode> statements)
        {
            Statements = statements;
        }

    }
}
