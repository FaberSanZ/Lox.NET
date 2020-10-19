using System.Collections.Generic;

namespace Lox
{
    class SetExpression : SyntaxNode
    {
        public Token Name {get;}
        public SyntaxNode Object {get;}
        public SyntaxNode Value {get;}

        public override SyntaxKind Kind => SyntaxKind.SetExpression;

        public SetExpression(SyntaxNode expr, Token name, SyntaxNode value)
        {
            Name = name;
            Object = expr;
            Value = value;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
