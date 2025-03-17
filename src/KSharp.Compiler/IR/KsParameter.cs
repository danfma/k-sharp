namespace KSharp.Compiler.IR;

public record KsParameter : KsNode
{
    public required KsIdentifier Name { get; init; }
    public KsTypeReference? Type { get; init; }
}
