using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Represents an interface declaration in the IR
/// </summary>
public record IrInterface : IrType
{
    public ImmutableList<IrProperty> Properties { get; init; } = [];
    public ImmutableList<IrMethod> Methods { get; init; } = [];
}
