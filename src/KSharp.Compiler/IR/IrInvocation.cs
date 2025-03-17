using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a method or function invocation in the IR
/// </summary>
public record IrInvocation : IrExpression
{
    public required IrIdentifier MethodName { get; init; }
    public ImmutableList<IrExpression> Arguments { get; init; } = [];
}
