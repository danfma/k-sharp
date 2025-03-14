using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Declaração de interface
/// </summary>
public record TsInterfaceDeclaration(
    TsIdentifier Name,
    ImmutableArray<TsTypeElement> Members,
    ImmutableArray<TsType> HeritageClauses = default
) : TsDeclaration;
