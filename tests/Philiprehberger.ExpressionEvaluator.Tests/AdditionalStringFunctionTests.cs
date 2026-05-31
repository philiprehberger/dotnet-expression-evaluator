namespace Philiprehberger.ExpressionEvaluator.Tests;

public class AdditionalStringFunctionTests
{
    [Fact]
    public void IndexOf_ReturnsZeroBasedIndex()
    {
        var result = Evaluator.Eval("indexof('hello world', 'world')");
        Assert.Equal(6.0, result);
    }

    [Fact]
    public void IndexOf_NotFound_ReturnsMinusOne()
    {
        var result = Evaluator.Eval("indexof('hello', 'xyz')");
        Assert.Equal(-1.0, result);
    }

    [Fact]
    public void StartsWith_TrueCase_ReturnsOne()
    {
        var result = Evaluator.Eval("startswith('hello', 'he')");
        Assert.Equal(1.0, result);
    }

    [Fact]
    public void StartsWith_FalseCase_ReturnsZero()
    {
        var result = Evaluator.Eval("startswith('hello', 'lo')");
        Assert.Equal(0.0, result);
    }

    [Fact]
    public void EndsWith_TrueCase_ReturnsOne()
    {
        var result = Evaluator.Eval("endswith('hello', 'lo')");
        Assert.Equal(1.0, result);
    }

    [Fact]
    public void EndsWith_FalseCase_ReturnsZero()
    {
        var result = Evaluator.Eval("endswith('hello', 'he')");
        Assert.Equal(0.0, result);
    }

    [Fact]
    public void Replace_AppliedThenLen_ReflectsReplacement()
    {
        var result = Evaluator.Eval("len(replace('foo bar', 'bar', 'banana'))");
        Assert.Equal(10.0, result);
    }

    [Fact]
    public void Substring_AppliedThenLen_ReturnsLengthOfSlice()
    {
        var result = Evaluator.Eval("len(substring('hello world', 6, 5))");
        Assert.Equal(5.0, result);
    }

    [Fact]
    public void Substring_BoundsClamped()
    {
        var result = Evaluator.Eval("len(substring('hello', 3, 100))");
        Assert.Equal(2.0, result);
    }
}
