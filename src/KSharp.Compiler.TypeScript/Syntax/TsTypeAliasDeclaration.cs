namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Declaração de tipo
/// </summary>
public record TsTypeAliasDeclaration(TsIdentifier Name, TsType Type) : TsDeclaration;
