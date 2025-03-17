using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a complete compilation in the IR
/// </summary>
public record IrCompilation : IrNode
{
    public required string Name { get; init; }
    public required IrNamespace RootNamespace { get; init; }
    public ImmutableList<IrCompilationUnit> SourceFiles { get; init; } = [];
    public ImmutableList<IrModule> Modules { get; init; } = [];
    public ImmutableList<IrType> Types { get; init; } = [];

    public IrCompilation() { }

    public IrCompilation(string name)
    {
        Name = name;
    }
}
