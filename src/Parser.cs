namespace Philiprehberger.ExpressionEvaluator;

/// <summary>
/// Recursive descent parser that builds an AST from a list of tokens.
/// Operator precedence (lowest to highest): ternary, ||, &amp;&amp;, comparison, +/-, */%, ^, unary (-, !).
/// </summary>
internal sealed class Parser
{
    private readonly List<Token> _tokens;
    private int _pos;

    /// <summary>
    /// Initializes a new parser for the given token list.
    /// </summary>
    /// <param name="tokens">The tokens to parse.</param>
    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _pos = 0;
    }

    /// <summary>
    /// Parses the token list into an AST.
    /// </summary>
    /// <returns>The root AST node.</returns>
    /// <exception cref="FormatException">Thrown when the expression is malformed.</exception>
    public AstNode Parse()
    {
        var node = ParseTernary();

        if (Current.Kind != TokenKind.End)
            throw new FormatException($"Unexpected token '{Current.Value}' after end of expression.");

        return node;
    }

    private Token Current => _tokens[_pos];

    private Token Consume()
    {
        var token = _tokens[_pos];
        _pos++;
        return token;
    }

    private Token Expect(TokenKind kind)
    {
        if (Current.Kind != kind)
            throw new FormatException($"Expected {kind} but found '{Current.Value}'.");
        return Consume();
    }

    // Ternary conditional (lowest precedence): expr ? expr : expr
    private AstNode ParseTernary()
    {
        var condition = ParseLogicalOr();

        if (Current.Kind == TokenKind.Question)
        {
            Consume(); // consume '?'
            var trueExpr = ParseTernary();
            Expect(TokenKind.Colon);
            var falseExpr = ParseTernary();
            return new ConditionalNode(condition, trueExpr, falseExpr);
        }

        return condition;
    }

    // Logical OR (||): left-associative, lower precedence than &&
    private AstNode ParseLogicalOr()
    {
        var left = ParseLogicalAnd();

        while (Current.Kind == TokenKind.LogicalOr)
        {
            Consume();
            var right = ParseLogicalAnd();
            left = new LogicalOrNode(left, right);
        }

        return left;
    }

    // Logical AND (&&): left-associative, lower precedence than comparison
    private AstNode ParseLogicalAnd()
    {
        var left = ParseComparison();

        while (Current.Kind == TokenKind.LogicalAnd)
        {
            Consume();
            var right = ParseComparison();
            left = new LogicalAndNode(left, right);
        }

        return left;
    }

    // Comparison operators (>, <, >=, <=, ==, !=)
    private AstNode ParseComparison()
    {
        var left = ParseAddSub();

        if (Current.Kind == TokenKind.Comparison)
        {
            var op = Consume().Value;
            var right = ParseAddSub();
            return new ComparisonNode(op, left, right);
        }

        return left;
    }

    // Addition and subtraction
    private AstNode ParseAddSub()
    {
        var left = ParseMulDivMod();

        while (Current.Kind == TokenKind.Operator && Current.Value is "+" or "-")
        {
            var op = Consume().Value[0];
            var right = ParseMulDivMod();
            left = new BinaryNode(op, left, right);
        }

        return left;
    }

    // Multiplication, division, modulus
    private AstNode ParseMulDivMod()
    {
        var left = ParseExponent();

        while (Current.Kind == TokenKind.Operator && Current.Value is "*" or "/" or "%")
        {
            var op = Consume().Value[0];
            var right = ParseExponent();
            left = new BinaryNode(op, left, right);
        }

        return left;
    }

    // Exponentiation (right-associative, highest binary precedence)
    private AstNode ParseExponent()
    {
        var left = ParseUnary();

        if (Current.Kind == TokenKind.Operator && Current.Value is "^")
        {
            Consume();
            var right = ParseExponent(); // right-associative recursion
            return new BinaryNode('^', left, right);
        }

        return left;
    }

    // Unary plus/minus and logical NOT
    private AstNode ParseUnary()
    {
        if (Current.Kind == TokenKind.Operator && Current.Value is "+" or "-")
        {
            var op = Consume().Value[0];
            var operand = ParseUnary();

            if (op == '+')
                return operand;

            return new UnaryNode('-', operand);
        }

        if (Current.Kind == TokenKind.LogicalNot)
        {
            Consume();
            var operand = ParseUnary();
            return new LogicalNotNode(operand);
        }

        return ParsePrimary();
    }

    // Primary: number, string, variable, function call, parenthesized expression
    private AstNode ParsePrimary()
    {
        switch (Current.Kind)
        {
            case TokenKind.Number:
            {
                var token = Consume();
                return new NumberNode(double.Parse(token.Value, System.Globalization.CultureInfo.InvariantCulture));
            }

            case TokenKind.String:
            {
                var token = Consume();
                return new StringNode(token.Value);
            }

            case TokenKind.Identifier:
            {
                var token = Consume();

                // Function call: identifier followed by '('
                if (Current.Kind == TokenKind.LeftParen)
                {
                    Consume(); // consume '('
                    var args = new List<AstNode>();

                    if (Current.Kind != TokenKind.RightParen)
                    {
                        args.Add(ParseTernary());
                        while (Current.Kind == TokenKind.Comma)
                        {
                            Consume(); // consume ','
                            args.Add(ParseTernary());
                        }
                    }

                    Expect(TokenKind.RightParen);
                    return new FunctionCallNode(token.Value.ToLowerInvariant(), args);
                }

                // Variable reference
                return new VariableNode(token.Value);
            }

            case TokenKind.LeftParen:
            {
                Consume(); // consume '('
                var node = ParseTernary();
                Expect(TokenKind.RightParen);
                return node;
            }

            default:
                throw new FormatException($"Unexpected token '{Current.Value}'.");
        }
    }
}
