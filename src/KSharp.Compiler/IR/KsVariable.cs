namespace KSharp.Compiler.IR;

public record KsVariable : KsTopLevelDeclaration
{
    public required KsIdentifier Name { get; init; }
    public KsTypeReference? Type { get; init; }
    public KsExpression? Initializer { get; init; }
    public bool IsMutable { get; init; } = false;
    public KsFullName? DeclaringModule { get; init; }
}
