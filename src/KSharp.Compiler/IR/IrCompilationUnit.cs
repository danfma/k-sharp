using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a compilation unit (source file) in the IR
/// </summary>
public record IrCompilationUnit : IrNode
{
    public required string FilePath { get; init; }
    public ImmutableList<IrDeclaration> Declarations { get; init; } = [];
    
    public IrCompilationUnit()
    {
    }
    
    public IrCompilationUnit(string filePath)
    {
        FilePath = filePath;
    }
}
