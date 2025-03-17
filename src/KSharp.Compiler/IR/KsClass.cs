using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

public record KsClass : KsType
{
    public KsTypeReference? BaseType { get; init; }
    public ImmutableList<KsProperty> Properties { get; init; } = [];
    public ImmutableList<KsFunction> Methods { get; init; } = [];
}