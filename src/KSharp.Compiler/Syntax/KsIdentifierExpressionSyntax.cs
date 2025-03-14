namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa uma referência a uma variável
/// </summary>
public record KsIdentifierExpressionSyntax : KsExpressionSyntax
{
    public string Name { get; set; }
}
