namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Statement de for..of (equivalente ao foreach)
/// </summary>
public record TsForOfStatement(
    TsIdentifier Identifier,
    TsExpression Expression,
    TsStatement Statement
) : TsStatement;
