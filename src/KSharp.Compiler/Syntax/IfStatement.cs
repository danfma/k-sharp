namespace KSharp.Compiler.Syntax;

public record IfStatement(Expression Condition, BlockStatement BlockStatement, ElseClause? Else)
    : Statement;
