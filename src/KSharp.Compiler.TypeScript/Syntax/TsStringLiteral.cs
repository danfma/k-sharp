namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Expressão literal de string
/// </summary>
public record TsStringLiteral(string Value) : TsLiteralExpression;
