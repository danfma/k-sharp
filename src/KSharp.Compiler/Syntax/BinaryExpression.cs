namespace KSharp.Compiler.Syntax;

public record BinaryExpression(Expression Left, BinaryOperator Operator, Expression Right)
    : Expression;
