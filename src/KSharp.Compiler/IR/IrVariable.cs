namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a global variable in the IR
/// </summary>
public record IrVariable : IrTopLevelDeclaration
{
    public required IrIdentifier Name { get; init; }
    public IrTypeReference? Type { get; init; }
    public IrExpression? Initializer { get; init; }
    public bool IsMutable { get; init; } = false;
    public IrFullName? DeclaringModule { get; init; }
}
