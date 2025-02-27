namespace KSharp.Compiler.Syntax;

public record TypeAnnotation(Identifier? Identifier) : AstNode
{
    public static TypeAnnotation Void => new(new Identifier("Void"));
    public static TypeAnnotation Int => new(new Identifier("Int32"));
}
