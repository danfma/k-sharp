namespace KSharp.Compiler.Syntax;

public record KsTypeAnnotationSyntax(KsIdentifierSyntax? Identifier) : KsNodeSyntax
{
    public static KsTypeAnnotationSyntax Void => new(new KsIdentifierSyntax("Void"));
    public static KsTypeAnnotationSyntax Int => new(new KsIdentifierSyntax("Int32"));
}
