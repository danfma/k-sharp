using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

public record FunctionCallExpression(Identifier Name, ImmutableArray<Expression> Arguments)
    : ValueExpression;
