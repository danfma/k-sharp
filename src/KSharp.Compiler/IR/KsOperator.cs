namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a concrete operator implementation.
/// </summary>
public record KsConcreteOperator(string Symbol) : KsOperator(Symbol);