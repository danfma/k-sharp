using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa um projeto K# composto por múltiplos arquivos fonte
/// </summary>
public record KsProjectSyntax : KsNodeSyntax
{
    /// <summary>
    /// Nome do projeto
    /// </summary>
    public required KsIdentifierSyntax Name { get; init; }

    /// <summary>
    /// Caminho do diretório raiz do projeto
    /// </summary>
    public required string RootDirectory { get; init; }

    /// <summary>
    /// Arquivos fonte que compõem o projeto
    /// </summary>
    public ImmutableList<KsSourceFileSyntax> SourceFiles { get; init; } = [];
}
