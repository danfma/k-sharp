namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Expressão binária
/// </summary>
public record TsBinaryExpression(TsExpression Left, TsBinaryOperator Operator, TsExpression Right)
    : TsExpression;
