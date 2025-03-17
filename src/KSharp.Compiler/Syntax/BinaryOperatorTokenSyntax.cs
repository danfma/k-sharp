namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a binary operator token in the syntax tree
/// </summary>
public record BinaryOperatorTokenSyntax(string Symbol) : SyntaxNode;
