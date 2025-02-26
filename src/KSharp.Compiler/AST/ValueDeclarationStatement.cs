namespace KSharp.Compiler.AST;

public record ValueDeclarationStatement(
    NameIdentifier Name,
    TypeIdentifier? TypeName = null,
    Expression? Initializer = null
) : DeclarationStatement
{
    public ValueDeclarationStatement(NameIdentifier name, Expression? initializer = null)
        : this(name, null, initializer) { }
}
