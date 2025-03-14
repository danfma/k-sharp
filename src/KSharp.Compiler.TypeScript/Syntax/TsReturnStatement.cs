namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Statement de return
/// </summary>
public record TsReturnStatement(TsExpression? Expression) : TsStatement;
