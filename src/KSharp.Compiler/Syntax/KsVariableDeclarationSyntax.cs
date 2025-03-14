namespace KSharp.Compiler.Syntax;

public record KsVariableDeclarationSyntax(
    bool Mutable,
    KsIdentifierSyntax Name,
    KsTypeAnnotationSyntax Type,
    KsExpressionSyntax? Initializer
) : KsDeclarationSyntax;
