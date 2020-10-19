using System.Collections.Generic;

namespace Lox
{
    internal sealed class Parser
    {
        private readonly IList<Token> _tokens;
        private readonly List<Error> _errors = new List<Error>();

        private int _current = 0;


        public Parser(IList<Token> tokens)
        {
            _tokens = tokens;
        }

        public List<SyntaxNode> Parse()
        {
            List<SyntaxNode> stmts = new List<SyntaxNode>();
            while (!IsAtEnd())
            {
                stmts.Add(ParseDeclaration());
            }

            return stmts;
        }

        public IEnumerable<Error> GetErrors()
        {
            return _errors;
        }

        private SyntaxNode ParseStatement()
        {

            if (Match(SyntaxKind.Print))
            {
                return ParsePrintStatement();
            }

            if (Match(SyntaxKind.Return))
            {
                return ParseReturnStatement();
            }

            if (Match(SyntaxKind.If))
            {
                return ParseIfStatement();
            }

            if (Match(SyntaxKind.While))
            {
                return ParseWhileStatement();
            }

            if (Match(SyntaxKind.For))
            {
                return ParseForStatement();
            }

            if (Match(SyntaxKind.LeftBrace))
            {
                return ParseBlockStatement();
            }

            //TODO: add more statement types
            return ParseExpressionStatement();
        }


        private SyntaxNode ParseDeclaration()
        {
            if (Match(SyntaxKind.Class))
            {
                return ParseClassDeclaration();
            }

            if (Match(SyntaxKind.Fun))
            {
                return ParseFunctionDeclaration("function");
            }

            if (Match(SyntaxKind.Var))
            {
                return ParseVariableDeclaration();
            }

            return ParseStatement();
        }

        private SyntaxNode ParseClassDeclaration()
        {
            Consume(SyntaxKind.Identifier, "Expect class name.");
            Token name = Previous();
            VariableExpression superclass = null;
            if (Match(SyntaxKind.Less))
            {
                Consume(SyntaxKind.Identifier, "Expect superclass name");
                superclass = new VariableExpression(Previous());
            }
            Consume(SyntaxKind.LeftBrace, "Expect '{' before class body.");
            List<FunctionStatement> methods = new List<FunctionStatement>();
            while (!Check(SyntaxKind.RightBrace) && !IsAtEnd())
            {
                methods.Add((FunctionStatement)ParseFunctionDeclaration("method") as FunctionStatement);
            }
            Consume(SyntaxKind.RightBrace, "Expect '}' after class Body.");
            return new ClassStatement(name, superclass, methods);
        }

        private SyntaxNode ParseFunctionDeclaration(string kind)
        {
            Consume(SyntaxKind.Identifier, $"Expect {kind} name");
            Token name = Previous();
            Consume(SyntaxKind.LeftParen, $"Expect '(' after {kind} name");
            List<Token> parameters = new List<Token>();
            if (!Check(SyntaxKind.RightParen))
            {
                do
                {
                    if (parameters.Count > 255)
                    {
                        Error(Peek(), "Cannot have morethan 255 parameters");
                    }

                    Consume(SyntaxKind.Identifier, "Expect parameter name");
                    parameters.Add(Previous());
                } while (Match(SyntaxKind.Comma));
            }

            Consume(SyntaxKind.RightParen, "Expect ')' after parameters.");

            Consume(SyntaxKind.LeftBrace, $"Expect '{{' before {kind} body");
            BlockStatement body = ParseBlockStatement() as BlockStatement;
            return new FunctionStatement(name, parameters, body.Statements);
        }

        private SyntaxNode ParseVariableDeclaration()
        {
            Consume(SyntaxKind.Identifier, "Expect variable name.");
            Token name = Previous();

            SyntaxNode initializer = null;
            if (Match(SyntaxKind.Equal))
            {
                initializer = ParseExpression();
            }

            Consume(SyntaxKind.Semicolon, "Expect ; after variable declaration");
            return new VariableDeclarationStatement(name, initializer);
        }

        private SyntaxNode ParseBlockStatement()
        {
            List<SyntaxNode> statements = new List<SyntaxNode>();

            while (!Check(SyntaxKind.RightBrace) && !IsAtEnd())
            {
                statements.Add(ParseDeclaration());
            }

            Consume(SyntaxKind.RightBrace, "Expect '}' after block.");
            return new BlockStatement(statements);
        }

        private SyntaxNode ParseForStatement()
        {
            Consume(SyntaxKind.LeftParen, "expect'(' after 'for'.");
            SyntaxNode initializer;
            if (Match(SyntaxKind.Semicolon))
            {
                initializer = null;
            }
            else if (Match(SyntaxKind.Var))
            {
                initializer = ParseVariableDeclaration();
            }
            else
            {
                initializer = ParseExpressionStatement();
            }

            SyntaxNode condition = null;
            if (!Check(SyntaxKind.Semicolon))
            {
                condition = ParseExpression();
            }

            Consume(SyntaxKind.Semicolon, "expect';' after loop condition.");

            SyntaxNode increment = null;
            if (!Check(SyntaxKind.RightParen))
            {
                increment = ParseExpression();
            }

            Consume(SyntaxKind.RightParen, "expect ')' after for clauses.");

            SyntaxNode body = ParseStatement();

            // desugar into a while loop
            if (increment != null)
            {
                body = new BlockStatement(new List<SyntaxNode> { body, new ExpressionStatement(increment) });
            }

            if (condition == null)
            {
                condition = new LiteralExpression(true);
            }

            body = new WhileStatement(condition, body);

            if (initializer != null)
            {
                body = new BlockStatement(new List<SyntaxNode> { new ExpressionStatement(initializer), body });
            }

            return body;

        }
        private SyntaxNode ParseWhileStatement()
        {
            Consume(SyntaxKind.LeftParen, "expect'(' after 'while'.");
            SyntaxNode condition = ParseExpression();
            Consume(SyntaxKind.RightParen, "expect ')' after condition.");

            SyntaxNode body = ParseStatement();
            return new WhileStatement(condition, body);
        }

        private SyntaxNode ParseIfStatement()
        {
            Consume(SyntaxKind.LeftParen, "expect'(' after if.");
            SyntaxNode condition = ParseExpression();
            Consume(SyntaxKind.RightParen, "expect ')' after if condition.");

            SyntaxNode thenBranch = ParseStatement();
            SyntaxNode elseBranch = null;
            if (Match(SyntaxKind.Else))
            {
                elseBranch = ParseStatement();
            }

            return new IfStatement(condition, thenBranch, elseBranch);
        }

        private SyntaxNode ParseReturnStatement()
        {
            Token keyword = Previous();
            SyntaxNode value = null;
            if (!Check(SyntaxKind.Semicolon))
            {
                value = ParseExpression();
            }

            Consume(SyntaxKind.Semicolon, "Expect ';' after return value.");
            return new ReturnStatement(keyword, value);
        }
        private SyntaxNode ParsePrintStatement()
        {
            SyntaxNode expr = ParseExpression();
            Consume(SyntaxKind.Semicolon, "Expect ; after expression");
            return new PrintStatement(expr);
        }
        private SyntaxNode ParseExpressionStatement()
        {
            SyntaxNode expr = ParseExpression();
            Consume(SyntaxKind.Semicolon, "Expect ; after expression");
            return new ExpressionStatement(expr);
        }

        private SyntaxNode ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private SyntaxNode ParseAssignmentExpression()
        {
            SyntaxNode expr = ParseBinaryExpression();

            if (Match(SyntaxKind.Equal))
            {
                Token equals = Previous();
                SyntaxNode value = ParseAssignmentExpression();

                if (expr is VariableExpression e)
                {
                    return new AssignmentExpression(e.Name, value);
                }
                else if (expr is GetExpression g)
                {
                    return new SetExpression(g.Object, g.Name, value);
                }

                Error(equals, "Invalid Assignment Target");
            }

            return expr;
        }

        private SyntaxNode ParseBinaryExpression(int parentPrecedence = 0)
        {
            SyntaxNode left;

            int unaryPrecedence = Peek().Kind.GetUnaryOperatorPrecendence();
            if (unaryPrecedence != 0 && unaryPrecedence >= parentPrecedence)
            {
                Token oper = Advance();
                SyntaxNode right = ParseBinaryExpression(unaryPrecedence);
                left = new UnaryExpression(oper, right);
            }
            else
            {
                left = ParseCallExpression();
            }

            while (true)
            {
                int binaryPrecedence = Peek().Kind.GetBinaryOperatorPrecendence();
                if (binaryPrecedence != 0 && binaryPrecedence > parentPrecedence)
                {
                    Token oper = Advance();
                    SyntaxNode right = ParseBinaryExpression(binaryPrecedence);
                    left = new BinaryExpression(left, oper, right);
                }
                else
                {
                    break;
                }
            }


            return left;
        }

        private SyntaxNode ParseCallExpression()
        {
            SyntaxNode expr = ParsePrimaryExpression();

            while (true)
            {
                if (Match(SyntaxKind.LeftParen))
                {
                    expr = ParseFinishCall(expr);
                }
                else if (Match(SyntaxKind.Dot))
                {
                    Consume(SyntaxKind.Identifier, "Expect property name after '.'");
                    Token name = Previous();
                    expr = new GetExpression(expr, name);
                }
                else
                {
                    break;
                }
            }
            return expr;
        }

        private SyntaxNode ParseFinishCall(SyntaxNode callee)
        {
            List<SyntaxNode> arguments = new List<SyntaxNode>();
            if (!Check(SyntaxKind.RightParen))
            {
                do
                {
                    if (arguments.Count > 255)
                    {
                        Error(Peek(), "cannot have more than 255 arguments.");
                    }

                    arguments.Add(ParseExpression());
                } while (Match(SyntaxKind.Comma));
            }
            Consume(SyntaxKind.RightParen, "Expect ')' after arguments.");
            Token paren = Previous();

            return new CallExpression(callee, paren, arguments);
        }
        private SyntaxNode ParsePrimaryExpression()
        {
            switch (Peek().Kind)
            {
                case SyntaxKind.False:
                    Match(SyntaxKind.False);
                    return new LiteralExpression(false);
                case SyntaxKind.True:
                    Match(SyntaxKind.True);
                    return new LiteralExpression(true);
                case SyntaxKind.Nil:
                    Match(SyntaxKind.Nil);
                    return new LiteralExpression(null);
                case SyntaxKind.Number:
                case SyntaxKind.String:
                    Match(SyntaxKind.Number, SyntaxKind.String);
                    return new LiteralExpression(Previous().Literal);
                case SyntaxKind.Identifier:
                    Match(SyntaxKind.Identifier);
                    return new VariableExpression(Previous());
                case SyntaxKind.This:
                    Match(SyntaxKind.This);
                    return new ThisExpression(Previous());
                case SyntaxKind.Super:
                    {
                        Match(SyntaxKind.Super);
                        Token keyword = Previous();
                        Consume(SyntaxKind.Dot, "Expect '.' after super");
                        Consume(SyntaxKind.Identifier, "Expect superclass method name");
                        Token method = Previous();

                        return new SuperExpression(keyword, method);
                    }

                case SyntaxKind.LeftParen:
                    SyntaxNode expr = ParseExpression();
                    Consume(SyntaxKind.RightParen, "Expect ')' after expression");
                    return new GroupingExpression(expr);

            }

            Error(Peek(), "Expect Expression");

            //TODO: fix this
            return null;
        }

        private bool Match(params SyntaxKind[] types)
        {
            foreach (SyntaxKind type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private bool Check(SyntaxKind type)
        {
            if (IsAtEnd())
            {
                return false;
            }

            return Peek().Kind == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd())
            {
                _current++;
            }

            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Kind == SyntaxKind.Eof;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private void Consume(SyntaxKind type, string message)
        {
            if (Check(type))
            {
                Advance();
            }
            else
            {
                Error(Peek(), message);
                Synchronize();
            }
        }

        private void Error(Token token, string message)
        {
            if (token.Kind == SyntaxKind.Eof)
            {
                _errors.Add(new Error(ErrorType.SyntaxError, token.Line, " at end", message));
            }
            else
            {
                _errors.Add(new Error(ErrorType.SyntaxError, token.Line, $" at '{token.Lexeme}'", message));
            }
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().Kind == SyntaxKind.Semicolon)
                {
                    return;
                }

                switch (Peek().Kind)
                {
                    case SyntaxKind.Class:
                    case SyntaxKind.Fun:
                    case SyntaxKind.Var:
                    case SyntaxKind.If:
                    case SyntaxKind.While:
                    case SyntaxKind.For:
                    case SyntaxKind.Return:
                        return;
                }

                Advance();
            }
        }
    }
}
