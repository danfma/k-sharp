using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Declaração de importação
/// </summary>
public record TsImportDeclaration(
    ImmutableArray<TsExportSpecifier> ImportClauses,
    string ModuleName,
    TsIdentifier? Alias = null)
    : TsSyntaxNode;
