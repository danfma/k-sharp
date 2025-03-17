using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Class method
/// </summary>
public record TsMethodDeclaration(
    TsIdentifier Name,
    ImmutableArray<TsParameter> Parameters,
    TsType? ReturnType,
    TsBlock Body,
    bool IsOptional = false,
    bool IsStatic = false
) : TsClassElement;
