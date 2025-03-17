namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a return statement in the syntax tree
/// </summary>
public record ReturnStatementSyntax(ExpressionSyntax? Expression) : StatementSyntax;
