namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Especificador de exportação
/// </summary>
public record TsExportSpecifier(TsIdentifier Name, TsIdentifier? Alias = null) : TsSyntaxNode;
