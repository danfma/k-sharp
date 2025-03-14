using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Bloco de código
/// </summary>
public record TsBlock(ImmutableArray<TsStatement> Statements) : TsStatement;
