namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a global variable declaration in the syntax tree
/// </summary>
public record GlobalVariableDeclarationSyntax(VariableDeclarationSyntax Variable)
    : GlobalDeclarationSyntax;
