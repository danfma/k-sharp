using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

public abstract record KsType : KsTopLevelDeclaration
{
    public required KsFullName FullName { get; init; }
    public ImmutableList<KsTypeReference> Implements { get; init; } = [];
}