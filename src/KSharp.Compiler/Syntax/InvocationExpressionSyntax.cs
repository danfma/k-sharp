using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a method or function invocation in the syntax tree
/// </summary>
public record InvocationExpressionSyntax(
    IdentifierTokenSyntax MethodName,
    ImmutableArray<ExpressionSyntax> Arguments
) : ExpressionSyntax;
