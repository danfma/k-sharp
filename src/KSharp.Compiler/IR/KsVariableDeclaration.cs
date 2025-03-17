namespace KSharp.Compiler.IR;

public record KsVariableDeclaration : KsStatement
{
    public required KsIdentifier Name { get; init; }
    public KsTypeReference? Type { get; init; }
    public KsExpression? Initializer { get; init; }
    public bool IsMutable { get; init; } = false;
}
