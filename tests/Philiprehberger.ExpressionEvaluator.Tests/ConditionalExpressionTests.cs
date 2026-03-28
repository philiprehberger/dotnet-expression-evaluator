namespace Philiprehberger.ExpressionEvaluator.Tests;

public class ConditionalExpressionTests
{
    [Fact]
    public void Ternary_TrueCondition_ReturnsTrueBranch()
    {
        var result = Evaluator.Eval("1 ? 10 : 20");
        Assert.Equal(10.0, result);
    }

    [Fact]
    public void Ternary_FalseCondition_ReturnsFalseBranch()
    {
        var result = Evaluator.Eval("0 ? 10 : 20");
        Assert.Equal(20.0, result);
    }

    [Fact]
    public void Ternary_WithComparison_SelectsCorrectBranch()
    {
        var vars = new Dictionary<string, double> { ["x"] = -5 };
        var result = Evaluator.Eval("x > 0 ? x : -x", vars);
        Assert.Equal(5.0, result);
    }

    [Fact]
    public void Ternary_NestedTernary_EvaluatesCorrectly()
    {
        var result = Evaluator.Eval("1 > 2 ? 100 : 2 > 1 ? 200 : 300");
        Assert.Equal(200.0, result);
    }

    [Fact]
    public void Ternary_WithExpressions_EvaluatesBranches()
    {
        var result = Evaluator.Eval("3 > 2 ? 10 + 5 : 10 - 5");
        Assert.Equal(15.0, result);
    }

    [Fact]
    public void Ternary_MissingColon_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => Evaluator.Eval("1 ? 10"));
    }
}
