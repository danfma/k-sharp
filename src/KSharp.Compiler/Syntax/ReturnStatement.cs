namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa um statement de retorno
/// </summary>
public record ReturnStatement(Expression? Expression) : Statement;
