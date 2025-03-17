namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a namespace declaration in the syntax tree
/// </summary>
public record NamespaceDeclarationSyntax(IdentifierTokenSyntax Name) : SyntaxNode;
