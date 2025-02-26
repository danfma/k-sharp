namespace KSharp.Compiler.AST;

public sealed record ExpressionStatement(Expression Expression) : Statement;