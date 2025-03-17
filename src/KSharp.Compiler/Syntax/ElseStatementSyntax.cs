namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a simple else statement in the syntax tree
/// </summary>
public record ElseStatementSyntax(BlockSyntax Block) : ElseClauseSyntax;
