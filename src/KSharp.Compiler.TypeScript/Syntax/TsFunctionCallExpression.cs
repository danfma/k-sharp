using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Chamada de função
/// </summary>
public record TsFunctionCallExpression(
    TsExpression Expression,
    ImmutableArray<TsExpression> Arguments
) : TsExpression;
