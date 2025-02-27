namespace KSharp.Compiler.Ast;

/// <summary>
/// Representa um statement de retorno
/// </summary>
public record ReturnStatement(Expression? Expression) : Statement;
