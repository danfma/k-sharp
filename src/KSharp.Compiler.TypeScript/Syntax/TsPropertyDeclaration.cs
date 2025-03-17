namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Class property
/// </summary>
public record TsPropertyDeclaration(
    TsIdentifier Name,
    TsType? Type,
    TsExpression? Initializer,
    bool IsOptional = false,
    bool IsReadonly = false
) : TsClassElement;
