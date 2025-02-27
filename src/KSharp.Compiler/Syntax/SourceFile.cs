using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa um programa K# completo
/// </summary>
public record SourceFile : AstNode
{
    public required string FileName { get; init; }
    public ImmutableList<UsingDirective> Usings { get; init; } = [];
    public NamespaceDeclaration? Namespace { get; init; }
    public ImmutableList<TopLevelDeclaration> Declarations { get; init; } = [];
}
