using KSharp.Compiler.Visitors;

namespace KSharp.Compiler.Ast;

/// <summary>
/// Representa um statement if/else
/// </summary>
public record IfStatement : Statement
{
    public Expression Condition { get; set; }
    public BlockStatement ThenBlock { get; set; }
    public BlockStatement ElseBlock { get; set; }

    public override void Accept(AstVisitor visitor)
    {
        visitor.VisitIfStatement(this);
    }
}
