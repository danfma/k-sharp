using KSharp.Compiler.Visitors;

namespace KSharp.Compiler.Ast;

/// <summary>
/// Representa uma referência a uma variável
/// </summary>
public record IdentifierExpression : Expression
{
    public string Name { get; set; }

    public override void Accept(AstVisitor visitor)
    {
        visitor.VisitIdentifierExpression(this);
    }
}
