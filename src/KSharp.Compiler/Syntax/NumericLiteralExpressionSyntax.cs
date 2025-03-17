namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a numeric literal expression in the syntax tree
/// </summary>
public record NumericLiteralExpressionSyntax<T>(T Value)
    : ConstantExpressionSyntax,
        INumberLiteralSyntax
    where T : struct;
