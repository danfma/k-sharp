namespace KSharp.Compiler.Ast;

public record TypeAnnotation(Identifier? Name) : AstNode
{
    public static TypeAnnotation Void => new(new Identifier("Void"));
    public static TypeAnnotation Int => new(new Identifier("Int32"));
}
