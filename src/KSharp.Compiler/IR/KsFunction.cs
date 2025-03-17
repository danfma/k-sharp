using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

public record KsFunction : KsTopLevelDeclaration
{
    public required KsIdentifier Name { get; init; }
    public ImmutableList<KsParameter> Parameters { get; init; } = [];
    public KsTypeReference? ReturnType { get; init; }
    public required KsBlock Body { get; init; }
    public KsFullName? DeclaringModule { get; init; }
}
