namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a using directive in the syntax tree
/// </summary>
public record UsingDirectiveSyntax(IdentifierTokenSyntax NamespaceName) : SyntaxNode;
