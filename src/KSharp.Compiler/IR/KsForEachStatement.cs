namespace KSharp.Compiler.IR;

public record KsForEachStatement : KsStatement
{
    public required KsIdentifier ItemIdentifier { get; init; }
    public required KsExpression Collection { get; init; }
    public required KsBlock Body { get; init; }
}
