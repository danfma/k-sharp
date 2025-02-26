namespace KSharp.Compiler.AST;

public record BinaryExpression(
    Expression Left,
    BinaryOperator Operator,
    Expression Right) : Expression;