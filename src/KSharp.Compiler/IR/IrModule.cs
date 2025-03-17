using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// A module represents a class that groups declarations of functions and variables defined directly
/// in the root scope of a file (not inside a class, for example).
/// </summary>
public record IrModule : IrTopLevelDeclaration
{
    public required IrFullName FullName { get; init; }
    public ImmutableList<IrVariable> Variables { get; init; } = [];
    public ImmutableList<IrFunction> Functions { get; init; } = [];
}