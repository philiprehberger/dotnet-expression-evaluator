namespace Philiprehberger.ExpressionEvaluator;

/// <summary>
/// Represents a value produced during expression evaluation, which can be either a number or a string.
/// </summary>
internal readonly struct EvalValue
{
    private readonly double _number;
    private readonly string? _str;

    /// <summary>
    /// Gets a value indicating whether this value is a number.
    /// </summary>
    public bool IsNumber { get; }

    /// <summary>
    /// Gets a value indicating whether this value is a string.
    /// </summary>
    public bool IsString => !IsNumber;

    /// <summary>
    /// Creates a numeric value.
    /// </summary>
    /// <param name="value">The numeric value.</param>
    public EvalValue(double value)
    {
        _number = value;
        _str = null;
        IsNumber = true;
    }

    /// <summary>
    /// Creates a string value.
    /// </summary>
    /// <param name="value">The string value.</param>
    public EvalValue(string value)
    {
        _number = 0;
        _str = value;
        IsNumber = false;
    }

    /// <summary>
    /// Gets the numeric value, or throws if this is a string.
    /// </summary>
    /// <returns>The numeric value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value is a string.</exception>
    public double AsNumber()
    {
        if (!IsNumber)
            throw new InvalidOperationException("Expected a numeric value but received a string.");
        return _number;
    }

    /// <summary>
    /// Gets the string value, or throws if this is a number.
    /// </summary>
    /// <returns>The string value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value is a number.</exception>
    public string AsString()
    {
        if (IsNumber)
            throw new InvalidOperationException("Expected a string value but received a number.");
        return _str!;
    }

    /// <summary>
    /// Implicitly converts a double to an <see cref="EvalValue"/>.
    /// </summary>
    /// <param name="value">The numeric value.</param>
    public static implicit operator EvalValue(double value) => new(value);

    /// <summary>
    /// Implicitly converts a string to an <see cref="EvalValue"/>.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static implicit operator EvalValue(string value) => new(value);
}
