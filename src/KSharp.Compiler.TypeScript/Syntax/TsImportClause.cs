namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Cláusula de importação
/// </summary>
public record TsImportClause(string Name, string? Alias = null) : TsSyntaxNode;
