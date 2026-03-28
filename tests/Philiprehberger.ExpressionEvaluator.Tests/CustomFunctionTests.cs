namespace Philiprehberger.ExpressionEvaluator.Tests;

public class CustomFunctionTests
{
    [Fact]
    public void RegisterFunction_SimpleClamp_EvaluatesCorrectly()
    {
        var evaluator = new Evaluator();
        evaluator.RegisterFunction("clamp", 3, args =>
            Math.Max(args[1], Math.Min(args[2], args[0])));

        var result = evaluator.Evaluate("clamp(15, 0, 10)");
        Assert.Equal(10.0, result);
    }

    [Fact]
    public void RegisterFunction_WithArity_ValidatesArgumentCount()
    {
        var evaluator = new Evaluator();
        evaluator.RegisterFunction("double", 1, args => args[0] * 2);

        var result = evaluator.Evaluate("double(5)");
        Assert.Equal(10.0, result);

        Assert.Throws<ArgumentException>(() => evaluator.Evaluate("double(1, 2)"));
    }

    [Fact]
    public void RegisterFunction_OverridesBuiltIn_UsesCustom()
    {
        var evaluator = new Evaluator();
        evaluator.RegisterFunction("abs", args => -999.0);

        var result = evaluator.Evaluate("abs(-5)");
        Assert.Equal(-999.0, result);
    }

    [Fact]
    public void RegisterFunction_WithVariables_WorksTogether()
    {
        var evaluator = new Evaluator();
        evaluator.RegisterFunction("scale", 2, args => args[0] * args[1]);

        var vars = new Dictionary<string, double> { ["x"] = 7 };
        var result = evaluator.Evaluate("scale(x, 3)", vars);
        Assert.Equal(21.0, result);
    }

    [Fact]
    public void RegisterFunction_NullName_ThrowsArgumentNullException()
    {
        var evaluator = new Evaluator();
        Assert.Throws<ArgumentNullException>(() => evaluator.RegisterFunction(null!, args => 0));
    }

    [Fact]
    public void RegisterFunction_EmptyName_ThrowsArgumentException()
    {
        var evaluator = new Evaluator();
        Assert.Throws<ArgumentException>(() => evaluator.RegisterFunction("  ", args => 0));
    }
}
