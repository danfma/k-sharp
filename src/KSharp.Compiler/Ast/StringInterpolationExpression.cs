using KSharp.Compiler.Visitors;

namespace KSharp.Compiler.Ast;

/// <summary>
/// Representa uma expressão de interpolação de string
/// </summary>
public record StringInterpolationExpression : Expression
{
    public List<Expression> Parts { get; } = new();

    public override void Accept(AstVisitor visitor)
    {
        visitor.VisitStringInterpolationExpression(this);
    }
}
