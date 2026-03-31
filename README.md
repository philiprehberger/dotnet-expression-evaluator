# Philiprehberger.ExpressionEvaluator

[![CI](https://github.com/philiprehberger/dotnet-expression-evaluator/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-expression-evaluator/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.ExpressionEvaluator.svg)](https://www.nuget.org/packages/Philiprehberger.ExpressionEvaluator)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-expression-evaluator)](https://github.com/philiprehberger/dotnet-expression-evaluator/commits/main)

Safe mathematical expression parser and evaluator with variables, custom functions, and operator precedence.

## Installation

```bash
dotnet add package Philiprehberger.ExpressionEvaluator
```

## Usage

```csharp
using Philiprehberger.ExpressionEvaluator;

double result = Evaluator.Eval("2 + 3 * 4");
// result = 14
```

### Simple Evaluation

```csharp
using Philiprehberger.ExpressionEvaluator;

Evaluator.Eval("10 / 2 + 1");   // 6
Evaluator.Eval("2 ^ 10");        // 1024
Evaluator.Eval("10 % 3");        // 1
Evaluator.Eval("-5 + 3");        // -2
```

### Variables

```csharp
using Philiprehberger.ExpressionEvaluator;

var variables = new Dictionary<string, double>
{
    ["x"] = 3,
    ["y"] = 4
};

double result = Evaluator.Eval("sqrt(x^2 + y^2)", variables);
// result = 5
```

### Comparison Operators

```csharp
using Philiprehberger.ExpressionEvaluator;

Evaluator.Eval("3 > 2");    // 1 (true)
Evaluator.Eval("3 < 2");    // 0 (false)
Evaluator.Eval("5 >= 5");   // 1
Evaluator.Eval("4 <= 3");   // 0
Evaluator.Eval("5 == 5");   // 1
Evaluator.Eval("5 != 3");   // 1
```

### Conditional Expressions

```csharp
using Philiprehberger.ExpressionEvaluator;

var vars = new Dictionary<string, double> { ["x"] = -5 };

Evaluator.Eval("x > 0 ? x : -x", vars);   // 5 (absolute value via ternary)
Evaluator.Eval("1 > 2 ? 100 : 200");       // 200
```

### Custom Functions

```csharp
using Philiprehberger.ExpressionEvaluator;

var evaluator = new Evaluator();
evaluator.RegisterFunction("clamp", 3, args =>
    Math.Max(args[1], Math.Min(args[2], args[0])));

evaluator.Evaluate("clamp(15, 0, 10)");  // 10
evaluator.Evaluate("clamp(-5, 0, 10)");  // 0
```

### Built-in Functions

```csharp
using Philiprehberger.ExpressionEvaluator;

Evaluator.Eval("abs(-42)");           // 42
Evaluator.Eval("min(10, 20)");        // 10
Evaluator.Eval("max(10, 20)");        // 20
Evaluator.Eval("round(3.7)");         // 4
Evaluator.Eval("ceil(2.1)");          // 3
Evaluator.Eval("floor(2.9)");         // 2
Evaluator.Eval("sin(pi / 2)");        // 1
Evaluator.Eval("log(e)");             // 1
Evaluator.Eval("pow(2, 10)");         // 1024
Evaluator.Eval("log10(100)");         // 2
```

### Compiled Expressions

```csharp
using Philiprehberger.ExpressionEvaluator;

var formula = Evaluator.CompileStatic("x^2 + 2*x + 1");

double r1 = formula(new Dictionary<string, double> { ["x"] = 3 });  // 16
double r2 = formula(new Dictionary<string, double> { ["x"] = 5 });  // 36
```

## API

### `Evaluator` (static methods)

| Method | Description |
|--------|-------------|
| `Eval(string expression, IDictionary<string, double>? variables = null)` | Parses and evaluates an expression, returning the result |
| `CompileStatic(string expression)` | Compiles an expression into a reusable delegate |

### `Evaluator` (instance methods)

| Method | Description |
|--------|-------------|
| `RegisterFunction(string name, Func<double[], double> function)` | Registers a custom function by name |
| `RegisterFunction(string name, int arity, Func<double[], double> function)` | Registers a custom function with arity validation |
| `Evaluate(string expression, IDictionary<string, double>? variables = null)` | Evaluates an expression with access to registered custom functions |
| `Compile(string expression)` | Compiles an expression with access to registered custom functions |

### Comparison Operators

| Operator | Description |
|----------|-------------|
| `>` | Greater than (returns 1.0 or 0.0) |
| `<` | Less than |
| `>=` | Greater than or equal |
| `<=` | Less than or equal |
| `==` | Equal |
| `!=` | Not equal |

### Conditional Expression

| Syntax | Description |
|--------|-------------|
| `condition ? trueExpr : falseExpr` | Evaluates trueExpr if condition is non-zero, falseExpr otherwise |

### Built-in Functions

| Function | Description |
|----------|-------------|
| `abs(x)` | Absolute value |
| `min(x, y)` | Minimum of two values |
| `max(x, y)` | Maximum of two values |
| `sqrt(x)` | Square root |
| `round(x)` | Round to nearest integer |
| `ceil(x)` | Round up to nearest integer |
| `floor(x)` | Round down to nearest integer |
| `sin(x)` | Sine (radians) |
| `cos(x)` | Cosine (radians) |
| `tan(x)` | Tangent (radians) |
| `asin(x)` | Inverse sine |
| `acos(x)` | Inverse cosine |
| `atan(x)` | Inverse tangent |
| `atan2(y, x)` | Two-argument inverse tangent |
| `log(x)` | Natural logarithm |
| `log10(x)` | Base-10 logarithm |
| `exp(x)` | Exponential (e^x) |
| `pow(x, y)` | Power (x^y) |
| `sign(x)` | Sign (-1, 0, or 1) |
| `truncate(x)` | Truncate toward zero |
| `pi` | Constant 3.14159... |
| `e` | Constant 2.71828... |

## Development

```bash
dotnet build src/Philiprehberger.ExpressionEvaluator.csproj --configuration Release
```

## Support

If you find this project useful:

⭐ [Star the repo](https://github.com/philiprehberger/dotnet-expression-evaluator)

🐛 [Report issues](https://github.com/philiprehberger/dotnet-expression-evaluator/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

💡 [Suggest features](https://github.com/philiprehberger/dotnet-expression-evaluator/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

❤️ [Sponsor development](https://github.com/sponsors/philiprehberger)

🌐 [All Open Source Projects](https://philiprehberger.com/open-source-packages)

💻 [GitHub Profile](https://github.com/philiprehberger)

🔗 [LinkedIn Profile](https://www.linkedin.com/in/philiprehberger)

## License

[MIT](LICENSE)
