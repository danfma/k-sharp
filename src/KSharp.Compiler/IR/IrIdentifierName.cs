namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a reference to a named entity in the IR
/// </summary>
public record IrIdentifierName(IrIdentifier Identifier) : IrExpression;
