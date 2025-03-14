namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa uma expressão de interpolação de string
/// </summary>
public record KsStringInterpolationExpressionSyntax : KsExpressionSyntax
{
    public List<KsExpressionSyntax> Parts { get; } = new();
}
