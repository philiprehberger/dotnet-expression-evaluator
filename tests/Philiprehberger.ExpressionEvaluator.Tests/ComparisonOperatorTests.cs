namespace Philiprehberger.ExpressionEvaluator.Tests;

public class ComparisonOperatorTests
{
    [Theory]
    [InlineData("3 > 2", 1.0)]
    [InlineData("2 > 3", 0.0)]
    [InlineData("3 > 3", 0.0)]
    public void GreaterThan_ReturnsExpected(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Theory]
    [InlineData("2 < 3", 1.0)]
    [InlineData("3 < 2", 0.0)]
    [InlineData("3 < 3", 0.0)]
    public void LessThan_ReturnsExpected(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Theory]
    [InlineData("3 >= 3", 1.0)]
    [InlineData("4 >= 3", 1.0)]
    [InlineData("2 >= 3", 0.0)]
    public void GreaterThanOrEqual_ReturnsExpected(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Theory]
    [InlineData("3 <= 3", 1.0)]
    [InlineData("2 <= 3", 1.0)]
    [InlineData("4 <= 3", 0.0)]
    public void LessThanOrEqual_ReturnsExpected(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Theory]
    [InlineData("5 == 5", 1.0)]
    [InlineData("5 == 6", 0.0)]
    public void Equal_ReturnsExpected(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Theory]
    [InlineData("5 != 6", 1.0)]
    [InlineData("5 != 5", 0.0)]
    public void NotEqual_ReturnsExpected(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Fact]
    public void Comparison_WithArithmetic_RespectsPrecedence()
    {
        // 2 + 3 > 4 should be (2+3) > 4 = 5 > 4 = 1.0
        Assert.Equal(1.0, Evaluator.Eval("2 + 3 > 4"));
    }
}
