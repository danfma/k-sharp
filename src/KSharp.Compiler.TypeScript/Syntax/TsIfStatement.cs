namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Statement condicional if
/// </summary>
public record TsIfStatement(
    TsExpression Condition,
    TsStatement ThenStatement,
    TsStatement? ElseStatement
) : TsStatement;
