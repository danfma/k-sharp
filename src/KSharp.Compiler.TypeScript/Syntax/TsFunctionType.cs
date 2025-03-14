using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Tipo de função
/// </summary>
public record TsFunctionType(ImmutableArray<TsParameter> Parameters, TsType ReturnType) : TsType;
