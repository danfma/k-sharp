using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Tipo união (ex: string | number)
/// </summary>
public record TsUnionType(ImmutableArray<TsType> Types) : TsType;
