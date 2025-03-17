namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents an identifier token in the syntax tree
/// </summary>
public record IdentifierTokenSyntax(string Text) : SyntaxNode
{
    public static implicit operator string(IdentifierTokenSyntax identifier) =>
        identifier.Text;
}
