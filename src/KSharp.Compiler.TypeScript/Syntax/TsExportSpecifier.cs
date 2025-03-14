namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Especificador de exportação
/// </summary>
public record TsExportSpecifier(string Name, string? Alias = null) : TsSyntaxNode;
