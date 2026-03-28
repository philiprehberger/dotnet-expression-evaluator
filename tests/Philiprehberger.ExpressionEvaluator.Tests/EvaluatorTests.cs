namespace Philiprehberger.ExpressionEvaluator.Tests;

public class EvaluatorTests
{
    [Theory]
    [InlineData("2 + 3", 5.0)]
    [InlineData("10 - 4", 6.0)]
    [InlineData("3 * 4", 12.0)]
    [InlineData("10 / 2", 5.0)]
    [InlineData("10 % 3", 1.0)]
    [InlineData("2 ^ 10", 1024.0)]
    public void Eval_BasicArithmetic_ReturnsCorrectResult(string expression, double expected)
    {
        Assert.Equal(expected, Evaluator.Eval(expression));
    }

    [Fact]
    public void Eval_WithVariables_ResolvesCorrectly()
    {
        var vars = new Dictionary<string, double> { ["x"] = 3, ["y"] = 4 };
        var result = Evaluator.Eval("sqrt(x^2 + y^2)", vars);
        Assert.Equal(5.0, result, 10);
    }

    [Fact]
    public void Eval_EmptyExpression_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => Evaluator.Eval(""));
    }

    [Fact]
    public void Eval_UndefinedVariable_ThrowsKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => Evaluator.Eval("x + 1"));
    }

    [Fact]
    public void Eval_DivisionByZero_ThrowsDivideByZeroException()
    {
        Assert.Throws<DivideByZeroException>(() => Evaluator.Eval("1 / 0"));
    }

    [Fact]
    public void CompileStatic_ReusesExpression()
    {
        var formula = Evaluator.CompileStatic("x^2 + 1");
        Assert.Equal(10.0, formula(new Dictionary<string, double> { ["x"] = 3 }));
        Assert.Equal(26.0, formula(new Dictionary<string, double> { ["x"] = 5 }));
    }

    [Fact]
    public void Instance_Compile_WithCustomFunctions()
    {
        var evaluator = new Evaluator();
        evaluator.RegisterFunction("double", 1, args => args[0] * 2);

        var formula = evaluator.Compile("double(x)");
        Assert.Equal(10.0, formula(new Dictionary<string, double> { ["x"] = 5 }));
    }

    [Fact]
    public void Eval_BuiltInConstants_ResolveCorrectly()
    {
        Assert.Equal(Math.PI, Evaluator.Eval("pi"), 10);
        Assert.Equal(Math.E, Evaluator.Eval("e"), 10);
    }
}
