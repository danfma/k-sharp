namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Tipo de referência (ex: string, number, etc)
/// </summary>
public record TsTypeReference(TsIdentifier TypeName) : TsType;
