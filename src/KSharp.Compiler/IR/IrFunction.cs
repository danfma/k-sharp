using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a top-level function in the IR (not associated with a class)
/// </summary>
public record IrFunction : IrTopLevelDeclaration
{
    public required IrIdentifier Name { get; init; }
    public ImmutableList<IrParameter> Parameters { get; init; } = [];
    public IrTypeReference? ReturnType { get; init; }
    public required IrBlock Body { get; init; }
    public IrFullName? DeclaringModule { get; init; }
}