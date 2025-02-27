namespace KSharp.Compiler.Ast;

public record IfStatement(Expression Condition, BlockStatement BlockStatement, ElseClause? Else)
    : Statement;

public abstract record ElseClause : AstNode;

public record ElseIfStatement(Expression Condition, BlockStatement BlockStatement, ElseClause? Else)
    : ElseClause;

public record ElseStatement(BlockStatement BlockStatement) : ElseClause;
