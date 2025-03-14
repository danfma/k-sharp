namespace KSharp.Compiler.Syntax;

public record KsIdentifierSyntax(string Name) : KsNodeSyntax
{
    public static implicit operator string(KsIdentifierSyntax syntaxIdentifier) =>
        syntaxIdentifier.Name;
}
