namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a concrete operator implementation in the IR.
/// </summary>
public record IrConcreteOperator(string Symbol) : IrOperator(Symbol);
