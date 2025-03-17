using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

public record KsTypeReference
{
    public string Name { get; init; }
    
    public KsTypeReference(string name)
    {
        Name = name;
    }
    
    public KsFullName? FullName { get; init; }
    public ImmutableList<KsTypeReference> GenericArguments { get; init; } = [];
}
