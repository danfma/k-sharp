using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

public record KsInterface : KsType
{
    public ImmutableList<KsProperty> Properties { get; init; } = [];
    public ImmutableList<KsMethod> Methods { get; init; } = [];
}