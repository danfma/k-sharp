using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Tipo de tupla (ex: [string, number])
/// </summary>
public record TsTupleType(ImmutableArray<TsType> ElementTypes) : TsType;
