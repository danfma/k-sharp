namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Propriedade em uma interface ou tipo literal
/// </summary>
public record TsPropertySignature(
    TsIdentifier Name,
    TsType Type,
    bool IsOptional = false,
    bool IsReadonly = false
) : TsTypeElement;
