namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a local variable declaration in the IR
/// </summary>
public record IrVariableDeclaration : IrStatement
{
    public required IrIdentifier Name { get; init; }
    public IrTypeReference? Type { get; init; }
    public IrExpression? Initializer { get; init; }
    public bool IsMutable { get; init; } = false;
}
