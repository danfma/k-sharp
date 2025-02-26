namespace KSharp.Compiler.AST;

public record TypeIdentifier(string Name) : Identifier
{
    public static TypeIdentifier Unknown => new("Unknown");
    public static TypeIdentifier Bool => new("Bool");
    public static TypeIdentifier Int => new("Int");
}
