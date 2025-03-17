using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

public record KsFunctionCall : KsExpression
{
    public required KsIdentifier Name { get; init; }
    public ImmutableList<KsExpression> Arguments { get; init; } = [];
}
