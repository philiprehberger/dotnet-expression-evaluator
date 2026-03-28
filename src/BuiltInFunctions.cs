namespace Philiprehberger.ExpressionEvaluator;

/// <summary>
/// Provides built-in mathematical functions and constants for the expression evaluator.
/// </summary>
internal static class BuiltInFunctions
{
    /// <summary>
    /// Registry of built-in functions keyed by lowercase name.
    /// Each entry maps to a delegate that accepts an array of arguments and returns a result.
    /// </summary>
    internal static readonly Dictionary<string, Func<double[], double>> Functions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["abs"] = args => { RequireArgs("abs", args, 1); return Math.Abs(args[0]); },
        ["min"] = args => { RequireArgs("min", args, 2); return Math.Min(args[0], args[1]); },
        ["max"] = args => { RequireArgs("max", args, 2); return Math.Max(args[0], args[1]); },
        ["sqrt"] = args => { RequireArgs("sqrt", args, 1); return Math.Sqrt(args[0]); },
        ["round"] = args => { RequireArgs("round", args, 1); return Math.Round(args[0]); },
        ["ceil"] = args => { RequireArgs("ceil", args, 1); return Math.Ceiling(args[0]); },
        ["floor"] = args => { RequireArgs("floor", args, 1); return Math.Floor(args[0]); },
        ["sin"] = args => { RequireArgs("sin", args, 1); return Math.Sin(args[0]); },
        ["cos"] = args => { RequireArgs("cos", args, 1); return Math.Cos(args[0]); },
        ["tan"] = args => { RequireArgs("tan", args, 1); return Math.Tan(args[0]); },
        ["log"] = args => { RequireArgs("log", args, 1); return Math.Log(args[0]); },
        ["asin"] = args => { RequireArgs("asin", args, 1); return Math.Asin(args[0]); },
        ["acos"] = args => { RequireArgs("acos", args, 1); return Math.Acos(args[0]); },
        ["atan"] = args => { RequireArgs("atan", args, 1); return Math.Atan(args[0]); },
        ["atan2"] = args => { RequireArgs("atan2", args, 2); return Math.Atan2(args[0], args[1]); },
        ["exp"] = args => { RequireArgs("exp", args, 1); return Math.Exp(args[0]); },
        ["log10"] = args => { RequireArgs("log10", args, 1); return Math.Log10(args[0]); },
        ["pow"] = args => { RequireArgs("pow", args, 2); return Math.Pow(args[0], args[1]); },
        ["sign"] = args => { RequireArgs("sign", args, 1); return Math.Sign(args[0]); },
        ["truncate"] = args => { RequireArgs("truncate", args, 1); return Math.Truncate(args[0]); },
    };

    /// <summary>
    /// Registry of built-in constants keyed by lowercase name.
    /// </summary>
    internal static readonly Dictionary<string, double> Constants = new(StringComparer.OrdinalIgnoreCase)
    {
        ["pi"] = Math.PI,
        ["e"] = Math.E,
    };

    internal static void RequireArgs(string name, double[] args, int expected)
    {
        if (args.Length != expected)
            throw new ArgumentException($"Function '{name}' expects {expected} argument(s) but received {args.Length}.");
    }
}
