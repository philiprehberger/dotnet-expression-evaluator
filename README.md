# Philiprehberger.ExpressionEvaluator

[![CI](https://github.com/philiprehberger/dotnet-expression-evaluator/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-expression-evaluator/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.ExpressionEvaluator.svg)](https://www.nuget.org/packages/Philiprehberger.ExpressionEvaluator)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-expression-evaluator)](LICENSE)
[![Sponsor](https://img.shields.io/badge/sponsor-GitHub%20Sponsors-ec6cb9)](https://github.com/sponsors/philiprehberger)

Safe mathematical expression parser and evaluator with variables, custom functions, and operator precedence.

## Installation

```bash
dotnet add package Philiprehberger.ExpressionEvaluator
```

## Usage

```csharp
using Philiprehberger.ExpressionEvaluator;

double result = Evaluator.Evaluate("2 + 3 * 4");
// result = 14
```

### Simple Evaluation

```csharp
using Philiprehberger.ExpressionEvaluator;

Evaluator.Evaluate("10 / 2 + 1");   // 6
Evaluator.Evaluate("2 ^ 10");        // 1024
Evaluator.Evaluate("10 % 3");        // 1
Evaluator.Evaluate("-5 + 3");        // -2
```

### Variables

```csharp
using Philiprehberger.ExpressionEvaluator;

var variables = new Dictionary<string, double>
{
    ["x"] = 3,
    ["y"] = 4
};

double result = Evaluator.Evaluate("sqrt(x^2 + y^2)", variables);
// result = 5
```

### Custom Functions

```csharp
using Philiprehberger.ExpressionEvaluator;

Evaluator.Evaluate("abs(-42)");           // 42
Evaluator.Evaluate("min(10, 20)");        // 10
Evaluator.Evaluate("max(10, 20)");        // 20
Evaluator.Evaluate("round(3.7)");         // 4
Evaluator.Evaluate("ceil(2.1)");          // 3
Evaluator.Evaluate("floor(2.9)");         // 2
Evaluator.Evaluate("sin(pi / 2)");        // 1
Evaluator.Evaluate("log(e)");             // 1
```

### Compiled Expressions

```csharp
using Philiprehberger.ExpressionEvaluator;

var formula = Evaluator.Compile("x^2 + 2*x + 1");

double r1 = formula(new Dictionary<string, double> { ["x"] = 3 });  // 16
double r2 = formula(new Dictionary<string, double> { ["x"] = 5 });  // 36
```

## API

### `Evaluator`

| Method | Description |
|--------|-------------|
| `Evaluate(string expression, IDictionary<string, double>? variables = null)` | Parses and evaluates an expression, returning the result |
| `Compile(string expression)` | Compiles an expression into a reusable delegate |

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
| `log(x)` | Natural logarithm |
| `pi` | Constant 3.14159... |
| `e` | Constant 2.71828... |

## Development

```bash
dotnet build src/Philiprehberger.ExpressionEvaluator.csproj --configuration Release
```

## License

[MIT](LICENSE)
