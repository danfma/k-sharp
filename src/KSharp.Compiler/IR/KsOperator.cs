namespace KSharp.Compiler.IR;

/// <summary>
/// Base class for operators in the IR.
/// </summary>
public abstract record KsOperator(string Symbol) : KsNode;
