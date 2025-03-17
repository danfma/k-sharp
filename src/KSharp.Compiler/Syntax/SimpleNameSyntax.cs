namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a reference to a variable
/// </summary>
public record SimpleNameSyntax(string Name) : ExpressionSyntax;
