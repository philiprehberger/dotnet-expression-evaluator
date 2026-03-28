namespace Philiprehberger.ExpressionEvaluator;

/// <summary>
/// Safe mathematical expression parser and evaluator with variables, custom functions, and operator precedence.
/// </summary>
public sealed class Evaluator
{
    private readonly Dictionary<string, Func<double[], double>> _customFunctions = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Registers a user-defined custom function that can be called from expressions.
    /// </summary>
    /// <param name="name">The function name (case-insensitive).</param>
    /// <param name="function">A delegate that receives an array of evaluated arguments and returns a result.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="function"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    public void RegisterFunction(string name, Func<double[], double> function)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(function);

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Function name cannot be empty or whitespace.", nameof(name));

        _customFunctions[name.ToLowerInvariant()] = function;
    }

    /// <summary>
    /// Registers a user-defined custom function with a fixed number of expected arguments.
    /// Arity is validated at evaluation time.
    /// </summary>
    /// <param name="name">The function name (case-insensitive).</param>
    /// <param name="arity">The expected number of arguments.</param>
    /// <param name="function">A delegate that receives an array of evaluated arguments and returns a result.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="function"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace, or <paramref name="arity"/> is negative.</exception>
    public void RegisterFunction(string name, int arity, Func<double[], double> function)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(function);

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Function name cannot be empty or whitespace.", nameof(name));

        if (arity < 0)
            throw new ArgumentException("Arity cannot be negative.", nameof(arity));

        var lowerName = name.ToLowerInvariant();
        _customFunctions[lowerName] = args =>
        {
            BuiltInFunctions.RequireArgs(lowerName, args, arity);
            return function(args);
        };
    }

    /// <summary>
    /// Parses and evaluates a mathematical expression, returning the computed result.
    /// </summary>
    /// <param name="expression">The expression string to evaluate (e.g. "2 + 3 * x").</param>
    /// <param name="variables">Optional dictionary of variable names and their values.</param>
    /// <returns>The result of evaluating the expression.</returns>
    /// <exception cref="FormatException">Thrown when the expression contains a syntax error.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a referenced variable is not defined.</exception>
    /// <exception cref="ArgumentException">Thrown when a function receives the wrong number of arguments.</exception>
    public double Evaluate(string expression, IDictionary<string, double>? variables = null)
    {
        var ast = ParseExpression(expression);
        return EvaluateNode(ast, variables ?? new Dictionary<string, double>());
    }

    /// <summary>
    /// Compiles an expression into a reusable delegate for repeated evaluation with different variable values.
    /// </summary>
    /// <param name="expression">The expression string to compile (e.g. "x^2 + y^2").</param>
    /// <returns>A function that accepts a variable dictionary and returns the computed result.</returns>
    /// <exception cref="FormatException">Thrown when the expression contains a syntax error.</exception>
    public Func<IDictionary<string, double>, double> Compile(string expression)
    {
        var ast = ParseExpression(expression);
        return variables => EvaluateNode(ast, variables);
    }

    /// <summary>
    /// Parses and evaluates a mathematical expression using the static API (no custom functions).
    /// </summary>
    /// <param name="expression">The expression string to evaluate.</param>
    /// <param name="variables">Optional dictionary of variable names and their values.</param>
    /// <returns>The result of evaluating the expression.</returns>
    /// <exception cref="FormatException">Thrown when the expression contains a syntax error.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a referenced variable is not defined.</exception>
    /// <exception cref="ArgumentException">Thrown when a function receives the wrong number of arguments.</exception>
    public static double Eval(string expression, IDictionary<string, double>? variables = null)
    {
        var ast = ParseExpression(expression);
        return EvaluateNodeStatic(ast, variables ?? new Dictionary<string, double>());
    }

    /// <summary>
    /// Compiles an expression into a reusable delegate using the static API (no custom functions).
    /// </summary>
    /// <param name="expression">The expression string to compile.</param>
    /// <returns>A function that accepts a variable dictionary and returns the computed result.</returns>
    /// <exception cref="FormatException">Thrown when the expression contains a syntax error.</exception>
    public static Func<IDictionary<string, double>, double> CompileStatic(string expression)
    {
        var ast = ParseExpression(expression);
        return variables => EvaluateNodeStatic(ast, variables);
    }

    private static AstNode ParseExpression(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new FormatException("Expression cannot be empty.");

        var tokenizer = new Tokenizer(expression);
        var tokens = tokenizer.Tokenize();
        var parser = new Parser(tokens);
        return parser.Parse();
    }

    private double EvaluateNode(AstNode node, IDictionary<string, double> variables)
    {
        return node switch
        {
            NumberNode n => n.Value,

            VariableNode v => ResolveVariable(v.Name, variables),

            UnaryNode u => u.Operator switch
            {
                '-' => -EvaluateNode(u.Operand, variables),
                _ => throw new FormatException($"Unknown unary operator '{u.Operator}'.")
            },

            BinaryNode b => EvaluateBinary(b, variables),

            ComparisonNode c => EvaluateComparison(c, variables),

            ConditionalNode t => EvaluateNode(t.Condition, variables) != 0.0
                ? EvaluateNode(t.TrueExpr, variables)
                : EvaluateNode(t.FalseExpr, variables),

            FunctionCallNode f => EvaluateFunction(f, variables),

            _ => throw new FormatException("Unknown AST node type.")
        };
    }

    private static double EvaluateNodeStatic(AstNode node, IDictionary<string, double> variables)
    {
        return node switch
        {
            NumberNode n => n.Value,

            VariableNode v => ResolveVariable(v.Name, variables),

            UnaryNode u => u.Operator switch
            {
                '-' => -EvaluateNodeStatic(u.Operand, variables),
                _ => throw new FormatException($"Unknown unary operator '{u.Operator}'.")
            },

            BinaryNode b => EvaluateBinaryStatic(b, variables),

            ComparisonNode c => EvaluateComparisonStatic(c, variables),

            ConditionalNode t => EvaluateNodeStatic(t.Condition, variables) != 0.0
                ? EvaluateNodeStatic(t.TrueExpr, variables)
                : EvaluateNodeStatic(t.FalseExpr, variables),

            FunctionCallNode f => EvaluateFunctionStatic(f, variables),

            _ => throw new FormatException("Unknown AST node type.")
        };
    }

    private static double ResolveVariable(string name, IDictionary<string, double> variables)
    {
        if (BuiltInFunctions.Constants.TryGetValue(name, out var constant))
            return constant;

        if (variables.TryGetValue(name, out var value))
            return value;

        throw new KeyNotFoundException($"Variable '{name}' is not defined.");
    }

    private double EvaluateBinary(BinaryNode node, IDictionary<string, double> variables)
    {
        var left = EvaluateNode(node.Left, variables);
        var right = EvaluateNode(node.Right, variables);
        return ApplyBinaryOperator(node.Operator, left, right);
    }

    private static double EvaluateBinaryStatic(BinaryNode node, IDictionary<string, double> variables)
    {
        var left = EvaluateNodeStatic(node.Left, variables);
        var right = EvaluateNodeStatic(node.Right, variables);
        return ApplyBinaryOperator(node.Operator, left, right);
    }

    private static double ApplyBinaryOperator(char op, double left, double right)
    {
        return op switch
        {
            '+' => left + right,
            '-' => left - right,
            '*' => left * right,
            '/' => right == 0
                ? throw new DivideByZeroException("Division by zero.")
                : left / right,
            '%' => right == 0
                ? throw new DivideByZeroException("Modulo by zero.")
                : left % right,
            '^' => Math.Pow(left, right),
            _ => throw new FormatException($"Unknown operator '{op}'.")
        };
    }

    private double EvaluateComparison(ComparisonNode node, IDictionary<string, double> variables)
    {
        var left = EvaluateNode(node.Left, variables);
        var right = EvaluateNode(node.Right, variables);
        return ApplyComparison(node.Operator, left, right);
    }

    private static double EvaluateComparisonStatic(ComparisonNode node, IDictionary<string, double> variables)
    {
        var left = EvaluateNodeStatic(node.Left, variables);
        var right = EvaluateNodeStatic(node.Right, variables);
        return ApplyComparison(node.Operator, left, right);
    }

    private static double ApplyComparison(string op, double left, double right)
    {
        var result = op switch
        {
            ">" => left > right,
            "<" => left < right,
            ">=" => left >= right,
            "<=" => left <= right,
            "==" => Math.Abs(left - right) < double.Epsilon,
            "!=" => Math.Abs(left - right) >= double.Epsilon,
            _ => throw new FormatException($"Unknown comparison operator '{op}'.")
        };

        return result ? 1.0 : 0.0;
    }

    private double EvaluateFunction(FunctionCallNode node, IDictionary<string, double> variables)
    {
        var args = node.Arguments
            .Select(arg => EvaluateNode(arg, variables))
            .ToArray();

        if (_customFunctions.TryGetValue(node.FunctionName, out var customFunc))
            return customFunc(args);

        if (BuiltInFunctions.Functions.TryGetValue(node.FunctionName, out var builtInFunc))
            return builtInFunc(args);

        throw new ArgumentException($"Unknown function '{node.FunctionName}'.");
    }

    private static double EvaluateFunctionStatic(FunctionCallNode node, IDictionary<string, double> variables)
    {
        if (!BuiltInFunctions.Functions.TryGetValue(node.FunctionName, out var func))
            throw new ArgumentException($"Unknown function '{node.FunctionName}'.");

        var args = node.Arguments
            .Select(arg => EvaluateNodeStatic(arg, variables))
            .ToArray();

        return func(args);
    }
}
