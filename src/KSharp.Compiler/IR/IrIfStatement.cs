namespace KSharp.Compiler.IR;

/// <summary>
/// Represents an if statement in the IR
/// </summary>
public record IrIfStatement(IrExpression Condition, IrBlock ThenBlock, IrBlock? ElseBlock)
    : IrStatement;
