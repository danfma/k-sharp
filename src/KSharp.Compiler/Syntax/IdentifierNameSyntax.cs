namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a reference to a named entity in the syntax tree
/// </summary>
public record IdentifierNameSyntax(IdentifierTokenSyntax Identifier) : ExpressionSyntax;
