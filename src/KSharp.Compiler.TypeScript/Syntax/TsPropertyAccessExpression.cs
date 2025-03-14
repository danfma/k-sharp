namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Acesso de membro (ex: obj.prop)
/// </summary>
public record TsPropertyAccessExpression(TsExpression Expression, TsIdentifier Name) : TsExpression;
