namespace Philiprehberger.ExpressionEvaluator.Tests;

public class LogicalOperatorTests
{
    [Theory]
    [InlineData("1 && 1", 1.0)]
    [InlineData("1 && 0", 0.0)]
    [InlineData("0 && 1", 0.0)]
    [InlineData("0 && 0", 0.0)]
    public void LogicalAnd_ReturnsExpected(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Theory]
    [InlineData("1 || 1", 1.0)]
    [InlineData("1 || 0", 1.0)]
    [InlineData("0 || 1", 1.0)]
    [InlineData("0 || 0", 0.0)]
    public void LogicalOr_ReturnsExpected(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Theory]
    [InlineData("!0", 1.0)]
    [InlineData("!1", 0.0)]
    [InlineData("!5", 0.0)]
    public void LogicalNot_ReturnsExpected(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Fact]
    public void LogicalAnd_ShortCircuits_LeftIsFalse()
    {
        // If left is false, right should not be evaluated.
        // We verify short-circuit by checking that division by zero does not throw.
        var result = Evaluator.Eval("0 && (1 / 0)");
        Assert.Equal(0.0, result);
    }

    [Fact]
    public void LogicalOr_ShortCircuits_LeftIsTrue()
    {
        // If left is true, right should not be evaluated.
        var result = Evaluator.Eval("1 || (1 / 0)");
        Assert.Equal(1.0, result);
    }

    [Fact]
    public void LogicalOperators_WithComparisons()
    {
        Assert.Equal(1.0, Evaluator.Eval("3 > 2 && 4 > 3"));
        Assert.Equal(0.0, Evaluator.Eval("3 > 2 && 4 < 3"));
        Assert.Equal(1.0, Evaluator.Eval("3 < 2 || 4 > 3"));
        Assert.Equal(0.0, Evaluator.Eval("3 < 2 || 4 < 3"));
    }

    [Fact]
    public void LogicalOperators_PrecedenceAndOverOr()
    {
        // && binds tighter than ||: 0 || 1 && 1 = 0 || (1 && 1) = 0 || 1 = 1
        Assert.Equal(1.0, Evaluator.Eval("0 || 1 && 1"));

        // 1 || 0 && 0 = 1 || (0 && 0) = 1 || 0 = 1
        Assert.Equal(1.0, Evaluator.Eval("1 || 0 && 0"));
    }

    [Fact]
    public void LogicalNot_WithComparison()
    {
        Assert.Equal(0.0, Evaluator.Eval("!(3 > 2)"));
        Assert.Equal(1.0, Evaluator.Eval("!(3 < 2)"));
    }

    [Fact]
    public void LogicalNot_DoubleNegation()
    {
        Assert.Equal(1.0, Evaluator.Eval("!!1"));
        Assert.Equal(0.0, Evaluator.Eval("!!0"));
    }

    [Fact]
    public void LogicalOperators_InTernary()
    {
        Assert.Equal(10.0, Evaluator.Eval("1 && 1 ? 10 : 20"));
        Assert.Equal(20.0, Evaluator.Eval("1 && 0 ? 10 : 20"));
    }

    [Fact]
    public void LogicalOperators_InstanceEvaluator()
    {
        var evaluator = new Evaluator();
        Assert.Equal(1.0, evaluator.Evaluate("1 && 1"));
        Assert.Equal(0.0, evaluator.Evaluate("!1"));
        Assert.Equal(1.0, evaluator.Evaluate("0 || 1"));
    }
}
