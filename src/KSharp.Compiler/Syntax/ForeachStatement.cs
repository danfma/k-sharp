namespace KSharp.Compiler.Syntax;

public record ForeachStatement(Identifier Item, Expression Source, BlockStatement Block)
    : Statement;
