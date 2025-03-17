using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a method or function declaration in the syntax tree
/// </summary>
public record MethodDeclarationSyntax(
    IdentifierTokenSyntax Identifier,
    ImmutableList<ParameterSyntax> Parameters,
    TypeClauseSyntax ReturnType,
    BlockSyntax Body
) : DeclarationSyntax;
