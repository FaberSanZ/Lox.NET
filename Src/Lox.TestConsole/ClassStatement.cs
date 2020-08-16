using System.Collections.Generic;

namespace Lox
{
    class ClassStatement : SyntaxNode 
    {
        public Token Name {get;}
        public List<FunctionStatement> Methods {get;}

        public VariableExpression SuperClass {get;}

        public SyntaxKind Kind => SyntaxKind.ClassStatement;

        public ClassStatement(Token name, VariableExpression superclass, List<FunctionStatement> methods)
        {
            Name = name;
            Methods = methods;
            SuperClass = superclass;
        }
    }
}
