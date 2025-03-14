using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

public record KsFunctionCallExpressionSyntax(
    KsIdentifierSyntax Name,
    ImmutableArray<KsExpressionSyntax> Arguments
) : KsValueExpressionSyntax;
