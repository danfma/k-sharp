namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a function parameter
/// </summary>
public record KsParameterSyntax(KsIdentifierSyntax Name, KsTypeAnnotationSyntax Type)
    : KsNodeSyntax;
