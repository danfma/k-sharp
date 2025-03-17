namespace KSharp.Compiler.IR;

/// <summary>
/// Represents an expression used as a statement in the IR
/// </summary>
public record IrExpressionStatement(IrExpression Expression) : IrStatement;
