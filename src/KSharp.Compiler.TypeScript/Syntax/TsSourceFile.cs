using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Representa um arquivo fonte TypeScript
/// </summary>
public record TsSourceFile(
    string FileName,
    ImmutableArray<TsImportDeclaration> Imports,
    ImmutableArray<TsStatement> Statements
) : TsSyntaxNode;
