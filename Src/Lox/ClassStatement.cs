using System.Collections.Generic;

namespace Lox
{
    class ClassStatement : SyntaxNode 
    {
        public Token Name {get;}
        public List<FunctionStatement> Methods {get;}

        public VariableExpression SuperClass {get;}

        public override SyntaxKind Kind => SyntaxKind.ClassStatement;

        public ClassStatement(Token name, VariableExpression superclass, List<FunctionStatement> methods)
        {
            Name = name;
            Methods = methods;
            SuperClass = superclass;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
