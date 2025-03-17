namespace KSharp.Compiler.IR;

public record KsBinaryExpression : KsExpression
{
    public required KsExpression Left { get; init; }
    public required KsOperator Operator { get; init; }
    public required KsExpression Right { get; init; }
}