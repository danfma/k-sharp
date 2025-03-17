namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a binary expression in the syntax tree
/// </summary>
public record BinaryExpressionSyntax(
    ExpressionSyntax Left,
    BinaryOperatorTokenSyntax Operator,
    ExpressionSyntax Right
) : ExpressionSyntax;
