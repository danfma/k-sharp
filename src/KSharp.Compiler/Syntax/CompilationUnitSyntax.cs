using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a complete K# compilation unit (source file)
/// </summary>
public record CompilationUnitSyntax : SyntaxNode
{
    public required string FileName { get; init; }
    public ImmutableList<UsingDirectiveSyntax> Usings { get; init; } = [];
    public NamespaceDeclarationSyntax? Namespace { get; init; }
    public ImmutableList<GlobalDeclarationSyntax> Declarations { get; init; } = [];
}
