using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a block of statements in the IR
/// </summary>
public record IrBlock(ImmutableList<IrStatement> Statements) : IrNode;