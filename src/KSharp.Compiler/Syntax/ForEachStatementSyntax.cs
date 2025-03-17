namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a foreach statement in the syntax tree
/// </summary>
public record ForEachStatementSyntax(
    IdentifierTokenSyntax Identifier,
    ExpressionSyntax Collection,
    BlockSyntax Body
) : StatementSyntax;
