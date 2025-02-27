namespace KSharp.Compiler.Syntax;

public record ElseIfStatement(Expression Condition, BlockStatement BlockStatement, ElseClause? Else)
    : ElseClause;
