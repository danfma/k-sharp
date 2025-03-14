using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa um programa K# completo
/// </summary>
public record KsSourceFileSyntax : KsNodeSyntax
{
    public required string FileName { get; init; }
    public ImmutableList<KsUsingDirectiveSyntax> Usings { get; init; } = [];
    public KsNamespaceDeclarationSyntax? Namespace { get; init; }
    public ImmutableList<KsTopLevelDeclarationSyntax> Declarations { get; init; } = [];
}
