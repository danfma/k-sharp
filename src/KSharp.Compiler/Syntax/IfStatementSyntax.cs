namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents an if statement in the syntax tree
/// </summary>
public record IfStatementSyntax(
    ExpressionSyntax Condition,
    BlockSyntax ThenBlock,
    ElseClauseSyntax? Else
) : StatementSyntax;
