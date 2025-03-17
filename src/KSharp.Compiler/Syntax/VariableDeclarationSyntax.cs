namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a variable declaration in the syntax tree
/// </summary>
public record VariableDeclarationSyntax(
    bool Mutable,
    IdentifierTokenSyntax Identifier,
    TypeClauseSyntax Type,
    ExpressionSyntax? Initializer
) : DeclarationSyntax;
