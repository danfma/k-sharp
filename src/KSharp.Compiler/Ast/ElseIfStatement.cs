namespace KSharp.Compiler.Ast;

public record ElseIfStatement(Expression Condition, BlockStatement BlockStatement, ElseClause? Else)
    : ElseClause;