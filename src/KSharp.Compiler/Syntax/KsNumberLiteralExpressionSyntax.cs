namespace KSharp.Compiler.Syntax;

public record KsNumberLiteralExpressionSyntax<T>(T Value)
    : KsLiteralExpressionSyntax,
        INumberLiteralSyntax
    where T : struct;
