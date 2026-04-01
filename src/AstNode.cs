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
/// A string literal value (delimited by single quotes).
/// </summary>
/// <param name="Value">The string value.</param>
internal sealed record StringNode(string Value) : AstNode;

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
/// A comparison operation returning 1.0 for true or 0.0 for false.
/// </summary>
/// <param name="Operator">The comparison operator string (e.g. "&gt;", "&lt;=", "==", "!=").</param>
/// <param name="Left">The left operand.</param>
/// <param name="Right">The right operand.</param>
internal sealed record ComparisonNode(string Operator, AstNode Left, AstNode Right) : AstNode;

/// <summary>
/// A logical AND operation (short-circuit). Returns 1.0 if both operands are non-zero, 0.0 otherwise.
/// </summary>
/// <param name="Left">The left operand.</param>
/// <param name="Right">The right operand.</param>
internal sealed record LogicalAndNode(AstNode Left, AstNode Right) : AstNode;

/// <summary>
/// A logical OR operation (short-circuit). Returns 1.0 if either operand is non-zero, 0.0 otherwise.
/// </summary>
/// <param name="Left">The left operand.</param>
/// <param name="Right">The right operand.</param>
internal sealed record LogicalOrNode(AstNode Left, AstNode Right) : AstNode;

/// <summary>
/// A logical NOT operation. Returns 1.0 if the operand is zero, 0.0 otherwise.
/// </summary>
/// <param name="Operand">The operand.</param>
internal sealed record LogicalNotNode(AstNode Operand) : AstNode;

/// <summary>
/// A conditional (ternary) expression: condition ? trueExpr : falseExpr.
/// </summary>
/// <param name="Condition">The condition expression.</param>
/// <param name="TrueExpr">The expression to evaluate when the condition is non-zero.</param>
/// <param name="FalseExpr">The expression to evaluate when the condition is zero.</param>
internal sealed record ConditionalNode(AstNode Condition, AstNode TrueExpr, AstNode FalseExpr) : AstNode;

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
