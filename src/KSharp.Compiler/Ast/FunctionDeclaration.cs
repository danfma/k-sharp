using System.Collections.Immutable;
using KSharp.Compiler.Visitors;

namespace KSharp.Compiler.Ast;

/// <summary>
/// Representa uma declaração de função
/// </summary>
public record FunctionDeclaration(
    Identifier Identifier,
    ImmutableList<Parameter> Parameters,
    TypeAnnotation ReturnType,
    BlockStatement Body
) : Declaration
{
    public override void Accept(AstVisitor visitor)
    {
        visitor.VisitFunctionDeclaration(this);
    }
}
