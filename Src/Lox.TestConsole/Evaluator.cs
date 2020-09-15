using System;
using System.Collections.Generic;
using static Lox.Functional;

namespace Lox
{
    internal class Return : Exception
    {
        public object Value { get; }
        public Return(object value)
        {
            Value = value;
        }
    }

    internal class RuntimeError : Exception
    {
        public Token Token { get; }
        public RuntimeError(Token token, string message) : base(message)
        {
            Token = token;
        }
    }

    internal class Environment
    {
        private readonly Dictionary<string, object?> _values = new Dictionary<string, object?>();

        public Environment? Enclosing { get; private set; }

        public Environment(Environment? enclosing = null)
        {
            Enclosing = enclosing;
        }


        public void Define(string name, object? value)
        {
            _values[name] = value;
        }

        public object Get(Token name)
        {
            if (_values.TryGetValue(name.Lexeme, out object? value))
            {
                return value ?? None;
            }

            if (Enclosing != null)
            {
                return Enclosing.Get(name);
            }

            throw new RuntimeError(name, $"Undefined variable {name.Lexeme}.");
        }

        public object GetAt(int distance, Token name)
        {
            return Ancestor(distance).Get(name);
        }

        private Environment Ancestor(int distance)
        {
            Environment env = this;
            for (int i = 0; i < distance; i++)
            {
                env = env.Enclosing;
            }

            return env;
        }


        public void Assign(Token name, object? value)
        {
            if (_values.ContainsKey(name.Lexeme))
            {
                _values[name.Lexeme] = value;
            }
            else if (Enclosing != null)
            {
                Enclosing.Assign(name, value);
            }
            else
            {
                throw new RuntimeError(name, $"Undefined variable {name.Lexeme}.");
            }
        }

        public void AssignAt(int distance, Token name, object? value)
        {
            Ancestor(distance).Assign(name, value);
        }
    }

    internal class Evaluator
    {
        public Environment Globals { get; }
        private Environment _environment;
        private readonly Dictionary<SyntaxNode, int> _locals = new Dictionary<SyntaxNode, int>();

        public Evaluator()
        {
            Globals = new Environment();
            _environment = Globals;
        }

        public void Resolve(SyntaxNode expr, int depth)
        {
            _locals[expr] = depth;
        }

        private object LookupVariable(Token name, SyntaxNode expr)
        {
            bool res = _locals.TryGetValue(expr, out int distance);
            if (res)
            {
                return _environment.GetAt(distance, name);
            }

            return Globals.Get(name);
        }
        public void Evaluate(List<SyntaxNode> expressions)
        {
            foreach (SyntaxNode expression in expressions)
            {
                Evaluate(expression);
            }
        }

        private object Evaluate(SyntaxNode expression)
        {
            switch (expression.Kind)
            {
                case SyntaxKind.PrintStatement:
                    var e = (expression as PrintStatement).Expression.Kind; 
 
                    object result = Evaluate((expression as PrintStatement).Expression);
                    Console.WriteLine(result);
                    return System.ValueTuple.Create();

                case SyntaxKind.ExpressionStatement:
                    Evaluate(((ExpressionStatement)expression).Expression);
                    return System.ValueTuple.Create();

                case SyntaxKind.VariableDeclarationStatement:
                    return EvaluateVariableDeclartionStatement(expression as VariableDeclarationStatement);

                case SyntaxKind.IfStatement:
                    return EvaluateIfStatement((IfStatement)expression);

                case SyntaxKind.WhileStatement:
                    return EvaluateWhileStatement((WhileStatement)expression);

                case SyntaxKind.BlockStatement:
                    return EvaluateBlockStatement((BlockStatement)expression);

                case SyntaxKind.FunctionStatement:
                    return EvaluateFunctionStatement((FunctionStatement)expression);

                case SyntaxKind.ClassStatement:
                    return EvaluateClassStatement((ClassStatement)expression);

                case SyntaxKind.ReturnStatement:
                    return EvaluateReturnStatement((ReturnStatement)expression);

                case SyntaxKind.CallExpression:
                    return EvaluateCallExpression((CallExpression)expression);

                case SyntaxKind.VariableExpression:
                    return EvaluateVariableExpression((VariableExpression)expression);

                case SyntaxKind.AssignmentExpression:
                    return EvaluateAssignmentExpression((AssignmentExpression)expression);

                case SyntaxKind.BinaryExpression:
                    return EvaluateBinaryExpression((BinaryExpression)expression);

                case SyntaxKind.GroupingExpression:
                    return EvaluateGroupingExpression((GroupingExpression)expression);

                case SyntaxKind.UnaryExpression:
                    return EvaluateUnaryExpression((UnaryExpression)expression);

                case SyntaxKind.LiteralExpression:
                    return EvaluateLiteralExpression((LiteralExpression)expression);

                case SyntaxKind.GetExpression:
                    return EvaluateGetExpression((GetExpression)expression);

                case SyntaxKind.SetExpression:
                    return EvaluateSetExpression((SetExpression)expression);

                case SyntaxKind.ThisExpression:
                    return EvaluateThisExpression((ThisExpression)expression);

                case SyntaxKind.SuperExpression:
                    return EvaluateSuperExpression((SuperExpression)expression);

                default:
                    throw new NotSupportedException();
            }
        }

        private object EvaluateSuperExpression(SuperExpression expr)
        {
            int distance = _locals[expr];
            LoxClass superclass = (LoxClass)_environment.GetAt(distance, new Token(TokenType.Super, "super", null, 0));
            LoxInstance obj = (LoxInstance)_environment.GetAt(distance - 1, new Token(TokenType.This, "this", null, 0));

            LoxFunction method = superclass.FindMethod(expr.Method.Lexeme);
            if (method == null)
            {
                throw new RuntimeError(expr.Method, $"Undefine property {expr.Method.Lexeme}");
            }
            return method.Bind(obj);
        }
        private object EvaluateThisExpression(ThisExpression expr)
        {
            return LookupVariable(expr.Keyword, expr);
        }
        private object EvaluateSetExpression(SetExpression expr)
        {
            object obj = Evaluate(expr.Object);

            LoxInstance instance = obj as LoxInstance;
            if (instance == null)
            {
                throw new RuntimeError(expr.Name, "Only instances have fields");
            }

            object value = Evaluate(expr.Value);
            instance.Set(expr.Name, value);
            return value;
        }
        private object EvaluateGetExpression(GetExpression expr)
        {
            object obj = Evaluate(expr.Object);
            if (obj is LoxInstance o)
            {
                return o.Get(expr.Name);
            }

            throw new RuntimeError(expr.Name, "Only instances have properties");
        }
        private object EvaluateClassStatement(ClassStatement expr)
        {
            object superclass = null;
            if (expr.SuperClass != null)
            {
                superclass = Evaluate(expr.SuperClass);
                if (!(superclass is LoxClass))
                {
                    throw new RuntimeError(expr.SuperClass.Name, "Superclass must be a class");
                }
            }
            _environment.Define(expr.Name.Lexeme, null);

            if (expr.SuperClass != null)
            {
                _environment = new Environment(_environment);
                _environment.Define("super", superclass);
            }

            Dictionary<string, LoxFunction> methods = new Dictionary<string, LoxFunction>();
            foreach (FunctionStatement method in expr.Methods)
            {
                LoxFunction function = new LoxFunction(method, _environment, method.Name.Lexeme.Equals("constructor"));
                methods.Add(method.Name.Lexeme, function);
            }
            LoxClass klass = new LoxClass(expr.Name.Lexeme, (LoxClass)superclass, methods);
            if (superclass != null)
            {
                _environment = _environment.Enclosing;
            }
            _environment.Assign(expr.Name, klass);
            return null;
        }
        private object EvaluateReturnStatement(ReturnStatement expr)
        {

            object value = null;
            if (expr.Value != null)
            {
                value = Evaluate(expr.Value);
            }

            throw new Return(value);
        }
        private object EvaluateFunctionStatement(FunctionStatement expr)
        {
            LoxFunction function = new LoxFunction(expr, _environment, false);
            _environment.Define(expr.Name.Lexeme, function);
            return null;
        }
        private object EvaluateCallExpression(CallExpression expr)
        {
            object callee = Evaluate(expr.Callee);
            List<object> arguments = new List<object>();
            foreach (SyntaxNode argument in expr.Arguments)
            {
                arguments.Add(Evaluate(argument));
            }
            LoxCallable function = callee as LoxCallable;
            if (function is null)
            {
                throw new RuntimeError(expr.Paren, "Can olny call functions and classes.");
            }

            // check arity
            if (arguments.Count != function.Arity)
            {
                throw new RuntimeError(expr.Paren, $"Expected {function.Arity} arguments but got {arguments.Count}.");
            }
            return function.Call(this, arguments);
        }
        private object EvaluateWhileStatement(WhileStatement expr)
        {
            while (IsTruthy(Evaluate(expr.Condition)))
            {
                Evaluate(expr.Body);
            }

            return null;
        }

        private object EvaluateIfStatement(IfStatement expr)
        {
            if (IsTruthy(Evaluate(expr.Condition)))
            {
                Evaluate(expr.ThenBranch);
            }
            else if (expr.ElseBranch != null)
            {
                Evaluate(expr.ElseBranch);
            }

            return null;
        }

        private object EvaluateBlockStatement(BlockStatement expr)
        {
            EvaluateBlock(expr.Statements, new Environment(_environment));
            return null;
        }

        public void EvaluateBlock(List<SyntaxNode> statements, Environment environment)
        {
            Environment previous = _environment;
            try
            {
                _environment = environment;
                foreach (SyntaxNode statement in statements)
                {
                    Evaluate(statement);
                }
            }
            finally
            {
                _environment = previous;
            }
        }

        private object EvaluateAssignmentExpression(AssignmentExpression expr)
        {
            object value = Evaluate(expr.Value);
            bool res = _locals.TryGetValue(expr, out int distance);
            if (res)
            {
                _environment.AssignAt(distance, expr.Name, value);
            }
            else
            {
                Globals.Assign(expr.Name, value);
            }

            return value;
        }
        private object EvaluateVariableDeclartionStatement(VariableDeclarationStatement expr)
        {
            object value = null;
            if (expr.Initializer != null)
            {
                value = Evaluate(expr.Initializer);
            }

            _environment.Define(expr.Name.Lexeme, value);
            return null;
        }

        private object EvaluateVariableExpression(VariableExpression expr)
        {
            return LookupVariable(expr.Name, expr);
        }

        private object EvaluateLiteralExpression(LiteralExpression expr)
        {
            return expr.Value.Or(null);
        }

        private object EvaluateGroupingExpression(GroupingExpression expr)
        {
            return Evaluate(expr.Expression);
        }

        private object EvaluateUnaryExpression(UnaryExpression expr)
        {
            object right = Evaluate(expr.Right);
            switch (expr.Operator.Type)
            {
                case TokenType.Minus:
                    CheckNumberOperand(expr.Operator, right);
                    return -(double)right;
                case TokenType.Bang:
                    return !IsTruthy(right);
            }
            throw new NotSupportedException($"Unexpected unary operator {expr.Operator}");
        }

        private bool IsTruthy(object obj)
        {
            if (obj is null || obj == None)
            {
                return false;
            }

            if (obj is bool b)
            {
                return b;
            }

            return true;
        }

        private object EvaluateBinaryExpression(BinaryExpression expr)
        {
            object left = Evaluate(expr.Left);
            object right = null;
            if (!(expr.Operator.Type == TokenType.AndAnd || expr.Operator.Type == TokenType.OrOr))
            {
                right = Evaluate(expr.Right);
            }

            switch (expr.Operator.Type)
            {
                case TokenType.Minus:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left - (double)right;

                case TokenType.Star:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;

                case TokenType.Slash:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;

                case TokenType.Plus:
                    if (left is double l && right is double r)
                    {
                        return l + r;
                    }
                    else if (left is double && right is double)
                    {
                        return (string)left + (string)right;
                    }
                    else
                    {
                        throw new RuntimeError(expr.Operator, "Operands must be two numbers or two strings.");
                    }

                case TokenType.Greater:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case TokenType.GreaterEqual:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left >= (double)right;
                case TokenType.Less:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left < (double)right;
                case TokenType.LessEqual:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left <= (double)right;
                case TokenType.EqualEqual:
                    return Equals(left, right);
                case TokenType.BangEqual:
                    return !Equals(left, right);
                case TokenType.AndAnd:
                    if (!IsTruthy(left))
                    {
                        return left;
                    }
                    else
                    {
                        return Evaluate(expr.Right);
                    }

                case TokenType.OrOr:
                    if (IsTruthy(left))
                    {
                        return left;
                    }
                    else
                    {
                        return Evaluate(expr.Right);
                    }
            }

            throw new NotSupportedException($"Unexpected binary operator {expr.Operator}");
        }

        private void CheckNumberOperand(Token oper, object operand)
        {
            if (operand is double d)
            {
                return;
            }

            throw new RuntimeError(oper, $"Operand must be a number.");
        }

        private void checkNumberOperands(Token oper, object left, object right)
        {
            if (left is double && right is double)
            {
                return;
            }

            throw new RuntimeError(oper, "Operands must be numbers.");
        }

    }
}
