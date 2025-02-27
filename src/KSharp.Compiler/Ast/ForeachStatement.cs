namespace KSharp.Compiler.Ast;

public record ForeachStatement(Identifier Item, Expression Source, BlockStatement Block)
    : Statement;
