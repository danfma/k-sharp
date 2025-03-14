using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Declaração de função
/// </summary>
public record TsFunctionDeclaration(
    TsIdentifier Name,
    ImmutableArray<TsParameter> Parameters,
    TsType? ReturnType,
    TsBlock Body,
    bool IsAsync = false,
    bool IsGenerator = false
) : TsDeclaration;
