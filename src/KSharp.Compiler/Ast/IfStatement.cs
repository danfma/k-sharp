namespace KSharp.Compiler.Ast;

public record IfStatement(Expression Condition, BlockStatement BlockStatement, ElseClause? Else)
    : Statement;
