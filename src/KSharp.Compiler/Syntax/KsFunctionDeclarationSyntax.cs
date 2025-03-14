using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

public record KsFunctionDeclarationSyntax(
    KsIdentifierSyntax Identifier,
    ImmutableList<KsParameterSyntax> Parameters,
    KsTypeAnnotationSyntax ReturnType,
    KsBlockStatementSyntax Body
) : KsDeclarationSyntax;
