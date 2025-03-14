using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Tipo de objeto literal (ex: { name: string, age: number })
/// </summary>
public record TsTypeLiteral(ImmutableArray<TsTypeElement> Members) : TsType;
