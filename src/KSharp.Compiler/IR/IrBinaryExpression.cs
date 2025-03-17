namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a binary expression in the IR
/// </summary>
public record IrBinaryExpression : IrExpression
{
    public required IrExpression Left { get; init; }
    public required IrOperator Operator { get; init; }
    public required IrExpression Right { get; init; }
}
