namespace KSharp.Compiler.Syntax;

public record KsTopLevelVariableDeclarationSyntax(KsVariableDeclarationSyntax Variable)
    : KsTopLevelDeclarationSyntax;
