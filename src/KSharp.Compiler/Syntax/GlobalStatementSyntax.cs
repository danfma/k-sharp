namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a global statement declaration in the syntax tree (top-level statement)
/// </summary>
public record GlobalStatementSyntax(StatementSyntax Statement)
    : GlobalDeclarationSyntax;