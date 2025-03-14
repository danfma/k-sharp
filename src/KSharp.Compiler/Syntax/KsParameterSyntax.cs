namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa um parâmetro de função
/// </summary>
public record KsParameterSyntax(KsIdentifierSyntax Name, KsTypeAnnotationSyntax Type)
    : KsNodeSyntax;
