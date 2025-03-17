namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a literal expression with a constant value in the IR
/// </summary>
public record IrLiteralExpression(object Value) : IrExpression;
