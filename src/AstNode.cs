namespace Philiprehberger.ExpressionEvaluator;

/// <summary>
/// Abstract base for all AST nodes produced by the parser.
/// </summary>
internal abstract record AstNode;

/// <summary>
/// A numeric literal value.
/// </summary>
/// <param name="Value">The numeric value.</param>
internal sealed record NumberNode(double Value) : AstNode;

/// <summary>
/// A variable reference resolved at evaluation time.
/// </summary>
/// <param name="Name">The variable name.</param>
internal sealed record VariableNode(string Name) : AstNode;

/// <summary>
/// A binary operation (e.g. addition, multiplication).
/// </summary>
/// <param name="Operator">The operator character.</param>
/// <param name="Left">The left operand.</param>
/// <param name="Right">The right operand.</param>
internal sealed record BinaryNode(char Operator, AstNode Left, AstNode Right) : AstNode;

/// <summary>
/// A unary operation (e.g. negation).
/// </summary>
/// <param name="Operator">The operator character.</param>
/// <param name="Operand">The operand.</param>
internal sealed record UnaryNode(char Operator, AstNode Operand) : AstNode;

/// <summary>
/// A function call with one or more arguments.
/// </summary>
/// <param name="FunctionName">The function name.</param>
/// <param name="Arguments">The list of argument expressions.</param>
internal sealed record FunctionCallNode(string FunctionName, IReadOnlyList<AstNode> Arguments) : AstNode;
