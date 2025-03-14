namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Expressão literal de número
/// </summary>
public record TsNumberLiteral(double Value) : TsLiteralExpression;
