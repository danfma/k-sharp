namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a string literal expression in the syntax tree
/// </summary>
public record StringLiteralExpressionSyntax(string Value) : ConstantExpressionSyntax;
