namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Propriedade de classe
/// </summary>
public record TsPropertyDeclaration(
    TsIdentifier Name,
    TsType? Type,
    TsExpression? Initializer,
    bool IsOptional = false,
    bool IsReadonly = false
) : TsClassElement;
