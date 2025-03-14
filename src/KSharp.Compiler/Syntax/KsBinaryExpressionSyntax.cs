namespace KSharp.Compiler.Syntax;

public record KsBinaryExpressionSyntax(
    KsExpressionSyntax Left,
    KsBinaryOperatorSyntax Operator,
    KsExpressionSyntax Right
) : KsExpressionSyntax;
