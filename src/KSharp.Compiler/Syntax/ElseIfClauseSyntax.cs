namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents an else-if clause in the syntax tree
/// </summary>
public record ElseIfClauseSyntax(
    ExpressionSyntax Condition,
    BlockSyntax Block,
    ElseClauseSyntax? Else
) : ElseClauseSyntax;
