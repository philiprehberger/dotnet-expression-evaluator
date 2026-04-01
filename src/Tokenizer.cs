namespace Philiprehberger.ExpressionEvaluator;

/// <summary>
/// The kind of token produced by the tokenizer.
/// </summary>
internal enum TokenKind
{
    /// <summary>A numeric literal.</summary>
    Number,
    /// <summary>A string literal (single-quoted).</summary>
    String,
    /// <summary>An identifier (variable or function name).</summary>
    Identifier,
    /// <summary>An operator (+, -, *, /, ^, %).</summary>
    Operator,
    /// <summary>A comparison operator (&gt;, &lt;, &gt;=, &lt;=, ==, !=).</summary>
    Comparison,
    /// <summary>A logical AND operator (&amp;&amp;).</summary>
    LogicalAnd,
    /// <summary>A logical OR operator (||).</summary>
    LogicalOr,
    /// <summary>A logical NOT operator (!).</summary>
    LogicalNot,
    /// <summary>A question mark for ternary conditional.</summary>
    Question,
    /// <summary>A colon for ternary conditional.</summary>
    Colon,
    /// <summary>An opening parenthesis.</summary>
    LeftParen,
    /// <summary>A closing parenthesis.</summary>
    RightParen,
    /// <summary>A comma separating function arguments.</summary>
    Comma,
    /// <summary>End of input.</summary>
    End
}

/// <summary>
/// A token produced by the tokenizer.
/// </summary>
/// <param name="Kind">The token kind.</param>
/// <param name="Value">The string value of the token.</param>
internal readonly record struct Token(TokenKind Kind, string Value);

/// <summary>
/// Tokenizes a mathematical expression string into a sequence of tokens.
/// </summary>
internal sealed class Tokenizer
{
    private readonly string _input;
    private int _pos;

    /// <summary>
    /// Initializes a new tokenizer for the given input string.
    /// </summary>
    /// <param name="input">The expression string to tokenize.</param>
    public Tokenizer(string input)
    {
        _input = input;
        _pos = 0;
    }

    /// <summary>
    /// Reads all tokens from the input.
    /// </summary>
    /// <returns>A list of tokens including a trailing <see cref="TokenKind.End"/> token.</returns>
    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (_pos < _input.Length)
        {
            var ch = _input[_pos];

            if (char.IsWhiteSpace(ch))
            {
                _pos++;
                continue;
            }

            if (char.IsDigit(ch) || ch == '.')
            {
                tokens.Add(ReadNumber());
                continue;
            }

            if (ch == '\'')
            {
                tokens.Add(ReadString());
                continue;
            }

            if (char.IsLetter(ch) || ch == '_')
            {
                tokens.Add(ReadIdentifier());
                continue;
            }

            // Logical operators
            if (ch == '&')
            {
                if (_pos + 1 < _input.Length && _input[_pos + 1] == '&')
                {
                    tokens.Add(new Token(TokenKind.LogicalAnd, "&&"));
                    _pos += 2;
                    continue;
                }

                throw new FormatException($"Unexpected character '&' at position {_pos}. Did you mean '&&'?");
            }

            if (ch == '|')
            {
                if (_pos + 1 < _input.Length && _input[_pos + 1] == '|')
                {
                    tokens.Add(new Token(TokenKind.LogicalOr, "||"));
                    _pos += 2;
                    continue;
                }

                throw new FormatException($"Unexpected character '|' at position {_pos}. Did you mean '||'?");
            }

            // Two-character comparison operators (must check before '!' as standalone)
            if (ch is '>' or '<' or '=')
            {
                tokens.Add(ReadComparisonOrOperator());
                continue;
            }

            if (ch == '!')
            {
                var next = _pos + 1 < _input.Length ? _input[_pos + 1] : '\0';
                if (next == '=')
                {
                    _pos += 2;
                    tokens.Add(new Token(TokenKind.Comparison, "!="));
                    continue;
                }

                tokens.Add(new Token(TokenKind.LogicalNot, "!"));
                _pos++;
                continue;
            }

            if (ch is '+' or '-' or '*' or '/' or '^' or '%')
            {
                tokens.Add(new Token(TokenKind.Operator, ch.ToString()));
                _pos++;
                continue;
            }

            if (ch == '?')
            {
                tokens.Add(new Token(TokenKind.Question, "?"));
                _pos++;
                continue;
            }

            if (ch == ':')
            {
                tokens.Add(new Token(TokenKind.Colon, ":"));
                _pos++;
                continue;
            }

            if (ch == '(')
            {
                tokens.Add(new Token(TokenKind.LeftParen, "("));
                _pos++;
                continue;
            }

            if (ch == ')')
            {
                tokens.Add(new Token(TokenKind.RightParen, ")"));
                _pos++;
                continue;
            }

            if (ch == ',')
            {
                tokens.Add(new Token(TokenKind.Comma, ","));
                _pos++;
                continue;
            }

            throw new FormatException($"Unexpected character '{ch}' at position {_pos}.");
        }

        tokens.Add(new Token(TokenKind.End, ""));
        return tokens;
    }

    private Token ReadComparisonOrOperator()
    {
        var ch = _input[_pos];
        var next = _pos + 1 < _input.Length ? _input[_pos + 1] : '\0';

        if (ch == '>' && next == '=')
        {
            _pos += 2;
            return new Token(TokenKind.Comparison, ">=");
        }
        if (ch == '<' && next == '=')
        {
            _pos += 2;
            return new Token(TokenKind.Comparison, "<=");
        }
        if (ch == '=' && next == '=')
        {
            _pos += 2;
            return new Token(TokenKind.Comparison, "==");
        }
        if (ch == '>')
        {
            _pos++;
            return new Token(TokenKind.Comparison, ">");
        }
        if (ch == '<')
        {
            _pos++;
            return new Token(TokenKind.Comparison, "<");
        }

        throw new FormatException($"Unexpected character '{ch}' at position {_pos}.");
    }

    private Token ReadNumber()
    {
        var start = _pos;
        var hasDot = false;

        while (_pos < _input.Length && (char.IsDigit(_input[_pos]) || _input[_pos] == '.'))
        {
            if (_input[_pos] == '.')
            {
                if (hasDot)
                    throw new FormatException($"Invalid number at position {start}: multiple decimal points.");
                hasDot = true;
            }

            _pos++;
        }

        return new Token(TokenKind.Number, _input[start.._pos]);
    }

    private Token ReadString()
    {
        _pos++; // skip opening quote
        var start = _pos;

        while (_pos < _input.Length && _input[_pos] != '\'')
        {
            _pos++;
        }

        if (_pos >= _input.Length)
            throw new FormatException("Unterminated string literal.");

        var value = _input[start.._pos];
        _pos++; // skip closing quote
        return new Token(TokenKind.String, value);
    }

    private Token ReadIdentifier()
    {
        var start = _pos;

        while (_pos < _input.Length && (char.IsLetterOrDigit(_input[_pos]) || _input[_pos] == '_'))
        {
            _pos++;
        }

        return new Token(TokenKind.Identifier, _input[start.._pos]);
    }
}
