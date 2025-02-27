namespace KSharp.Compiler.Ast;

public record BinaryExpression(Expression Left, BinaryOperator Operator, Expression Right)
    : Expression;
