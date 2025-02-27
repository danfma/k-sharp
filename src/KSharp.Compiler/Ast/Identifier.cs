namespace KSharp.Compiler.Ast;

public record Identifier(string Name) : AstNode
{
    public static implicit operator string(Identifier identifier) => identifier.Name;
}
