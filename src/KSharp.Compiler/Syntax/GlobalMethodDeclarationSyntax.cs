namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a global method declaration in the syntax tree
/// </summary>
public record GlobalMethodDeclarationSyntax(MethodDeclarationSyntax Method)
    : GlobalDeclarationSyntax;
