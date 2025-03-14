namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Statement de expressão
/// </summary>
public record TsExpressionStatement(TsExpression Expression) : TsStatement;
