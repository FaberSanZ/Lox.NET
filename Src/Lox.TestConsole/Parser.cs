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

            if (Match(TokenType.Print))
            {
                return ParsePrintStatement();
            }

            if (Match(TokenType.Return))
            {
                return ParseReturnStatement();
            }

            if (Match(TokenType.If))
            {
                return ParseIfStatement();
            }

            if (Match(TokenType.While))
            {
                return ParseWhileStatement();
            }

            if (Match(TokenType.For))
            {
                return ParseForStatement();
            }

            if (Match(TokenType.LeftBrace))
            {
                return ParseBlockStatement();
            }

            //TODO: add more statement types
            return ParseExpressionStatement();
        }


        private SyntaxNode ParseDeclaration()
        {
            if (Match(TokenType.Class))
            {
                return ParseClassDeclaration();
            }

            if (Match(TokenType.Fun))
            {
                return ParseFunctionDeclaration("function");
            }

            if (Match(TokenType.Var))
            {
                return ParseVariableDeclaration();
            }

            return ParseStatement();
        }

        private SyntaxNode ParseClassDeclaration()
        {
            Consume(TokenType.Identifier, "Expect class name.");
            Token name = Previous();
            VariableExpression superclass = null;
            if (Match(TokenType.Less))
            {
                Consume(TokenType.Identifier, "Expect superclass name");
                superclass = new VariableExpression(Previous());
            }
            Consume(TokenType.LeftBrace, "Expect '{' before class body.");
            List<FunctionStatement> methods = new List<FunctionStatement>();
            while (!Check(TokenType.RightBrace) && !IsAtEnd())
            {
                methods.Add((FunctionStatement)ParseFunctionDeclaration("method") as FunctionStatement);
            }
            Consume(TokenType.RightBrace, "Expect '}' after class Body.");
            return new ClassStatement(name, superclass, methods);
        }

        private SyntaxNode ParseFunctionDeclaration(string kind)
        {
            Consume(TokenType.Identifier, $"Expect {kind} name");
            Token name = Previous();
            Consume(TokenType.LeftParen, $"Expect '(' after {kind} name");
            List<Token> parameters = new List<Token>();
            if (!Check(TokenType.RightParen))
            {
                do
                {
                    if (parameters.Count > 255)
                    {
                        Error(Peek(), "Cannot have morethan 255 parameters");
                    }

                    Consume(TokenType.Identifier, "Expect parameter name");
                    parameters.Add(Previous());
                } while (Match(TokenType.Comma));
            }

            Consume(TokenType.RightParen, "Expect ')' after parameters.");

            Consume(TokenType.LeftBrace, $"Expect '{{' before {kind} body");
            BlockStatement body = ParseBlockStatement() as BlockStatement;
            return new FunctionStatement(name, parameters, body.Statements);
        }

        private SyntaxNode ParseVariableDeclaration()
        {
            Consume(TokenType.Identifier, "Expect variable name.");
            Token name = Previous();

            SyntaxNode initializer = null;
            if (Match(TokenType.Equal))
            {
                initializer = ParseExpression();
            }

            Consume(TokenType.Semicolon, "Expect ; after variable declaration");
            return new VariableDeclarationStatement(name, initializer);
        }

        private SyntaxNode ParseBlockStatement()
        {
            List<SyntaxNode> statements = new List<SyntaxNode>();

            while (!Check(TokenType.RightBrace) && !IsAtEnd())
            {
                statements.Add(ParseDeclaration());
            }

            Consume(TokenType.RightBrace, "Expect '}' after block.");
            return new BlockStatement(statements);
        }

        private SyntaxNode ParseForStatement()
        {
            Consume(TokenType.LeftParen, "expect'(' after 'for'.");
            SyntaxNode initializer;
            if (Match(TokenType.Semicolon))
            {
                initializer = null;
            }
            else if (Match(TokenType.Var))
            {
                initializer = ParseVariableDeclaration();
            }
            else
            {
                initializer = ParseExpressionStatement();
            }

            SyntaxNode condition = null;
            if (!Check(TokenType.Semicolon))
            {
                condition = ParseExpression();
            }

            Consume(TokenType.Semicolon, "expect';' after loop condition.");

            SyntaxNode increment = null;
            if (!Check(TokenType.RightParen))
            {
                increment = ParseExpression();
            }

            Consume(TokenType.RightParen, "expect ')' after for clauses.");

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
            Consume(TokenType.LeftParen, "expect'(' after 'while'.");
            SyntaxNode condition = ParseExpression();
            Consume(TokenType.RightParen, "expect ')' after condition.");

            SyntaxNode body = ParseStatement();
            return new WhileStatement(condition, body);
        }

        private SyntaxNode ParseIfStatement()
        {
            Consume(TokenType.LeftParen, "expect'(' after if.");
            SyntaxNode condition = ParseExpression();
            Consume(TokenType.RightParen, "expect ')' after if condition.");

            SyntaxNode thenBranch = ParseStatement();
            SyntaxNode elseBranch = null;
            if (Match(TokenType.Else))
            {
                elseBranch = ParseStatement();
            }

            return new IfStatement(condition, thenBranch, elseBranch);
        }

        private SyntaxNode ParseReturnStatement()
        {
            Token keyword = Previous();
            SyntaxNode value = null;
            if (!Check(TokenType.Semicolon))
            {
                value = ParseExpression();
            }

            Consume(TokenType.Semicolon, "Expect ';' after return value.");
            return new ReturnStatement(keyword, value);
        }
        private SyntaxNode ParsePrintStatement()
        {
            SyntaxNode expr = ParseExpression();
            Consume(TokenType.Semicolon, "Expect ; after expression");
            return new PrintStatement(expr);
        }
        private SyntaxNode ParseExpressionStatement()
        {
            SyntaxNode expr = ParseExpression();
            Consume(TokenType.Semicolon, "Expect ; after expression");
            return new ExpressionStatement(expr);
        }

        private SyntaxNode ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private SyntaxNode ParseAssignmentExpression()
        {
            SyntaxNode expr = ParseBinaryExpression();

            if (Match(TokenType.Equal))
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

            int unaryPrecedence = Peek().Type.GetUnaryOperatorPrecendence();
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
                int binaryPrecedence = Peek().Type.GetBinaryOperatorPrecendence();
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
                if (Match(TokenType.LeftParen))
                {
                    expr = ParseFinishCall(expr);
                }
                else if (Match(TokenType.Dot))
                {
                    Consume(TokenType.Identifier, "Expect property name after '.'");
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
            if (!Check(TokenType.RightParen))
            {
                do
                {
                    if (arguments.Count > 255)
                    {
                        Error(Peek(), "cannot have more than 255 arguments.");
                    }

                    arguments.Add(ParseExpression());
                } while (Match(TokenType.Comma));
            }
            Consume(TokenType.RightParen, "Expect ')' after arguments.");
            Token paren = Previous();

            return new CallExpression(callee, paren, arguments);
        }
        private SyntaxNode ParsePrimaryExpression()
        {
            switch (Peek().Type)
            {
                case TokenType.False:
                    Match(TokenType.False);
                    return new LiteralExpression(false);
                case TokenType.True:
                    Match(TokenType.True);
                    return new LiteralExpression(true);
                case TokenType.Nil:
                    Match(TokenType.Nil);
                    return new LiteralExpression(null);
                case TokenType.Number:
                case TokenType.String:
                    Match(TokenType.Number, TokenType.String);
                    return new LiteralExpression(Previous().Literal);
                case TokenType.Identifier:
                    Match(TokenType.Identifier);
                    return new VariableExpression(Previous());
                case TokenType.This:
                    Match(TokenType.This);
                    return new ThisExpression(Previous());
                case TokenType.Super:
                    {
                        Match(TokenType.Super);
                        Token keyword = Previous();
                        Consume(TokenType.Dot, "Expect '.' after super");
                        Consume(TokenType.Identifier, "Expect superclass method name");
                        Token method = Previous();

                        return new SuperExpression(keyword, method);
                    }

                case TokenType.LeftParen:
                    SyntaxNode expr = ParseExpression();
                    Consume(TokenType.RightParen, "Expect ')' after expression");
                    return new GroupingExpression(expr);

            }

            Error(Peek(), "Expect Expression");

            //TODO: fix this
            return null;
        }

        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd())
            {
                return false;
            }

            return Peek().Type == type;
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
            return Peek().Type == TokenType.Eof;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private void Consume(TokenType type, string message)
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
            if (token.Type == TokenType.Eof)
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
                if (Previous().Type == TokenType.Semicolon)
                {
                    return;
                }

                switch (Peek().Type)
                {
                    case TokenType.Class:
                    case TokenType.Fun:
                    case TokenType.Var:
                    case TokenType.If:
                    case TokenType.While:
                    case TokenType.For:
                    case TokenType.Return:
                        return;
                }

                Advance();
            }
        }
    }
}
