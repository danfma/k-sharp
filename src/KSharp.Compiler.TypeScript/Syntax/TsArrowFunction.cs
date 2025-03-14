using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Expressão de arrow function
/// </summary>
public record TsArrowFunction(
    ImmutableArray<TsParameter> Parameters,
    TsType? ReturnType,
    TsExpression Body
) : TsExpression;
