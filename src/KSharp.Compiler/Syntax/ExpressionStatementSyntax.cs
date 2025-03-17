namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents an expression used as a statement in the syntax tree
/// </summary>
public record ExpressionStatementSyntax(ExpressionSyntax Expression) : StatementSyntax;
