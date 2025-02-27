namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa uma referência a uma variável
/// </summary>
public record IdentifierExpression : Expression
{
    public string Name { get; set; }
}
