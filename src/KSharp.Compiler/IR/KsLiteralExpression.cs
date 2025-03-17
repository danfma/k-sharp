namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a literal expression with a constant value.
/// </summary>
public record KsLiteralExpression(object Value) : KsExpression;