namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Tipo de array (ex: string[])
/// </summary>
public record TsArrayType(TsType ElementType) : TsType;
