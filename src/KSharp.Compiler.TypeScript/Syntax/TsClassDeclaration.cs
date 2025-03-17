using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Class declaration
/// </summary>
public record TsClassDeclaration(
    TsIdentifier Name,
    ImmutableArray<TsClassElement> Members,
    ImmutableArray<TsType> HeritageClauses = default
) : TsDeclaration;
