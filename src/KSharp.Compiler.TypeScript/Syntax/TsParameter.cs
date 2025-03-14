namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Parâmetro de função
/// </summary>
public record TsParameter(TsIdentifier Name, TsType? Type = null, TsExpression? Initializer = null);
