namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a top-level declaration in the IR.
/// This serves as a base type for declarations that exist at the root level.
/// </summary>
public abstract record IrTopLevelDeclaration : IrDeclaration;
