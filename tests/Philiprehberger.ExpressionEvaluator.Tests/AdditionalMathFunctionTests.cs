namespace Philiprehberger.ExpressionEvaluator.Tests;

public class AdditionalMathFunctionTests
{
    [Fact]
    public void Asin_ReturnsCorrectValue()
    {
        var result = Evaluator.Eval("asin(1)");
        Assert.Equal(Math.Asin(1), result, 10);
    }

    [Fact]
    public void Acos_ReturnsCorrectValue()
    {
        var result = Evaluator.Eval("acos(0)");
        Assert.Equal(Math.Acos(0), result, 10);
    }

    [Fact]
    public void Atan_ReturnsCorrectValue()
    {
        var result = Evaluator.Eval("atan(1)");
        Assert.Equal(Math.Atan(1), result, 10);
    }

    [Fact]
    public void Atan2_ReturnsCorrectValue()
    {
        var result = Evaluator.Eval("atan2(1, 1)");
        Assert.Equal(Math.Atan2(1, 1), result, 10);
    }

    [Fact]
    public void Exp_ReturnsCorrectValue()
    {
        var result = Evaluator.Eval("exp(1)");
        Assert.Equal(Math.E, result, 10);
    }

    [Fact]
    public void Log10_ReturnsCorrectValue()
    {
        var result = Evaluator.Eval("log10(100)");
        Assert.Equal(2.0, result, 10);
    }

    [Fact]
    public void Pow_ReturnsCorrectValue()
    {
        var result = Evaluator.Eval("pow(2, 10)");
        Assert.Equal(1024.0, result);
    }

    [Fact]
    public void Sign_ReturnsCorrectValues()
    {
        Assert.Equal(1.0, Evaluator.Eval("sign(42)"));
        Assert.Equal(-1.0, Evaluator.Eval("sign(-7)"));
        Assert.Equal(0.0, Evaluator.Eval("sign(0)"));
    }

    [Fact]
    public void Truncate_ReturnsCorrectValue()
    {
        Assert.Equal(3.0, Evaluator.Eval("truncate(3.9)"));
        Assert.Equal(-3.0, Evaluator.Eval("truncate(-3.9)"));
    }

    [Fact]
    public void Atan2_WrongArity_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Evaluator.Eval("atan2(1)"));
    }

    [Fact]
    public void Pow_WrongArity_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Evaluator.Eval("pow(2)"));
    }
}
