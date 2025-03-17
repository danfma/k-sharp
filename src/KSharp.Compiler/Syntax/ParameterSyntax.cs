namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a method or function parameter in the syntax tree
/// </summary>
public record ParameterSyntax(IdentifierTokenSyntax Identifier, TypeClauseSyntax Type)
    : SyntaxNode;
