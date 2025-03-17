namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a string interpolation expression
/// </summary>
public record StringInterpolationExpressionSyntax : ExpressionSyntax
{
    public List<ExpressionSyntax> Parts { get; } = new();
}
