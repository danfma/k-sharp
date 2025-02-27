namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa uma expressão de interpolação de string
/// </summary>
public record StringInterpolationExpression : Expression
{
    public List<Expression> Parts { get; } = new();
}
