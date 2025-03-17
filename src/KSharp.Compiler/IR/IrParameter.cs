namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a method or function parameter in the IR
/// </summary>
public record IrParameter : IrNode
{
    public required IrIdentifier Name { get; init; }
    public IrTypeReference? Type { get; init; }
}
