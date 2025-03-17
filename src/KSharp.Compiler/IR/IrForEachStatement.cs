namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a foreach statement in the IR
/// </summary>
public record IrForEachStatement : IrStatement
{
    public required IrIdentifier Identifier { get; init; }
    public required IrExpression Collection { get; init; }
    public required IrBlock Body { get; init; }
}
