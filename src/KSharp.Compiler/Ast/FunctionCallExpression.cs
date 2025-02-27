using System.Collections.Immutable;

namespace KSharp.Compiler.Ast;

public record FunctionCallExpression(Identifier Name, ImmutableArray<Expression> Arguments)
    : ValueExpression;
