namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Expressão literal de boolean
/// </summary>
public record TsBooleanLiteral(bool Value) : TsLiteralExpression;
