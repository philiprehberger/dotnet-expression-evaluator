namespace Philiprehberger.ExpressionEvaluator;

/// <summary>
/// Safe mathematical expression parser and evaluator with variables, custom functions, and operator precedence.
/// </summary>
public static class Evaluator
{
    /// <summary>
    /// Parses and evaluates a mathematical expression, returning the computed result.
    /// </summary>
    /// <param name="expression">The expression string to evaluate (e.g. "2 + 3 * x").</param>
    /// <param name="variables">Optional dictionary of variable names and their values.</param>
    /// <returns>The result of evaluating the expression.</returns>
    /// <exception cref="FormatException">Thrown when the expression contains a syntax error.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a referenced variable is not defined.</exception>
    /// <exception cref="ArgumentException">Thrown when a function receives the wrong number of arguments.</exception>
    public static double Evaluate(string expression, IDictionary<string, double>? variables = null)
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
    public static Func<IDictionary<string, double>, double> Compile(string expression)
    {
        var ast = ParseExpression(expression);
        return variables => EvaluateNode(ast, variables);
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

    private static double EvaluateNode(AstNode node, IDictionary<string, double> variables)
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

            FunctionCallNode f => EvaluateFunction(f, variables),

            _ => throw new FormatException("Unknown AST node type.")
        };
    }

    private static double ResolveVariable(string name, IDictionary<string, double> variables)
    {
        // Check built-in constants first
        if (BuiltInFunctions.Constants.TryGetValue(name, out var constant))
            return constant;

        if (variables.TryGetValue(name, out var value))
            return value;

        throw new KeyNotFoundException($"Variable '{name}' is not defined.");
    }

    private static double EvaluateBinary(BinaryNode node, IDictionary<string, double> variables)
    {
        var left = EvaluateNode(node.Left, variables);
        var right = EvaluateNode(node.Right, variables);

        return node.Operator switch
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
            _ => throw new FormatException($"Unknown operator '{node.Operator}'.")
        };
    }

    private static double EvaluateFunction(FunctionCallNode node, IDictionary<string, double> variables)
    {
        if (!BuiltInFunctions.Functions.TryGetValue(node.FunctionName, out var func))
            throw new ArgumentException($"Unknown function '{node.FunctionName}'.");

        var args = node.Arguments
            .Select(arg => EvaluateNode(arg, variables))
            .ToArray();

        return func(args);
    }
}
