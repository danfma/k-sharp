using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a class declaration in the IR
/// </summary>
public record IrClass : IrType
{
    public IrTypeReference? BaseType { get; init; }
    public ImmutableList<IrProperty> Properties { get; init; } = [];
    public ImmutableList<IrMethod> Methods { get; init; } = [];
}