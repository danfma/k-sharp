using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Declaração de importação
/// </summary>
public record TsImportDeclaration(string ModuleName, ImmutableArray<TsImportClause> ImportClauses)
    : TsSyntaxNode;
