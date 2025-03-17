using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

public record KsSourceFile : KsNode
{
    public required string FilePath { get; init; }
    public ImmutableList<KsDeclaration> Declarations { get; init; } = [];
    
    public KsSourceFile()
    {
    }
    
    public KsSourceFile(string filePath)
    {
        FilePath = filePath;
    }
}
