using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Base class for all type declarations in the IR
/// </summary>
public abstract record IrType : IrTopLevelDeclaration
{
    public required IrFullName FullName { get; init; }
    public ImmutableList<IrTypeReference> Implements { get; init; } = [];
}