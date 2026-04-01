# Changelog

## 0.3.0 (2026-03-31)

- Add logical operators `&&`, `||`, `!` with short-circuit evaluation and proper precedence
- Add string literal support using single quotes (e.g. `'hello'`)
- Add built-in string functions: `len(s)`, `upper(s)`, `lower(s)`, `trim(s)`, `concat(a,b)`, `contains(s,sub)`
- Add statistical functions: `mean(...)`, `median(...)`, `stdev(...)`, `variance(...)` with variable-length arguments

## 0.2.1 (2026-03-31)

- Standardize README to 3-badge format with emoji Support section
- Update CI actions to v5 for Node.js 24 compatibility

## 0.2.0 (2026-03-28)

- Add user-defined custom functions via `RegisterFunction` with optional arity validation
- Add conditional (ternary) expressions with `condition ? trueExpr : falseExpr` syntax
- Add comparison operators: `>`, `<`, `>=`, `<=`, `==`, `!=` returning 1.0/0.0
- Add math functions: `asin`, `acos`, `atan`, `atan2`, `exp`, `log10`, `pow`, `sign`, `truncate`
- Change `Evaluator` from static class to instance class for custom function support
- Add static `Eval` and `CompileStatic` methods for stateless usage
- Add unit test project with xUnit
- Add GitHub issue templates, dependabot, and PR template

## 0.1.1 (2026-03-26)

- Add Sponsor badge and fix License link format in README

## 0.1.0 (2026-03-21)

- Initial release
- Parse and evaluate mathematical expressions with operator precedence
- Support for variables and compiled expressions
- Built-in functions: abs, min, max, sqrt, round, ceil, floor, sin, cos, tan, log
- Built-in constants: pi, e
