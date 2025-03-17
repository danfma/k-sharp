using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a K# project composed of multiple source files
/// </summary>
public record CompilationSyntax : SyntaxNode
{
    /// <summary>
    /// Name of the project
    /// </summary>
    public required IdentifierTokenSyntax Name { get; init; }

    /// <summary>
    /// Root directory path of the project
    /// </summary>
    public required string RootDirectory { get; init; }

    /// <summary>
    /// Source files that compose the project
    /// </summary>
    public ImmutableList<CompilationUnitSyntax> SourceFiles { get; init; } = [];
}
