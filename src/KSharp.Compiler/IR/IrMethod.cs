using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a method in a class or interface in the IR
/// </summary>
public record IrMethod : IrDeclaration
{
    public required IrIdentifier Name { get; init; }
    public ImmutableList<IrParameter> Parameters { get; init; } = [];
    public IrTypeReference? ReturnType { get; init; }
    public required IrBlock Body { get; init; }
    public bool IsAbstract { get; init; } = false;
    public bool IsVirtual { get; init; } = false;
    public bool IsOverride { get; init; } = false;
}