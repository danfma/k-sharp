using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

public record KsProject : KsNode
{
    public required string Name { get; init; }
    public required KsNamespace RootNamespace { get; init; }
    public ImmutableList<KsSourceFile> SourceFiles { get; init; } = [];
    public ImmutableList<KsModule> Modules { get; init; } = [];
    public ImmutableList<KsType> Types { get; init; } = [];
    
    public KsProject()
    {
    }
    
    public KsProject(string name)
    {
        Name = name;
    }
}
