namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Expressão de acesso a elemento (ex: arr[0])
/// </summary>
public record TsElementAccessExpression(TsExpression Expression, TsExpression ArgumentExpression)
    : TsExpression;
