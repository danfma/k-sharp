namespace KSharp.Compiler.AST;

public record VariableDeclarationStatement(
    NameIdentifier Name,
    TypeIdentifier? TypeName,
    Expression? Initializer
) : DeclarationStatement
{
    public VariableDeclarationStatement(NameIdentifier name, Expression? initializer)
        : this(name, null, initializer) { }
}
