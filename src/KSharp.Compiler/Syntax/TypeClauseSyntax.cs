namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a type clause in the syntax tree
/// </summary>
public record TypeClauseSyntax(IdentifierTokenSyntax? TypeName) : SyntaxNode
{
    public static TypeClauseSyntax Void => new(new IdentifierTokenSyntax("Void"));
    public static TypeClauseSyntax Int => new(new IdentifierTokenSyntax("Int32"));
}
