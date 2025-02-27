namespace KSharp.Compiler.Ast;

/// <summary>
/// Representa um statement if/else
/// </summary>
public record IfStatement(Expression Condition, BlockStatement BlockStatement) : Statement;
