namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a return statement in the IR
/// </summary>
public record IrReturnStatement(IrExpression? Expression) : IrStatement;