namespace KSharp.Compiler.Syntax;

public record VariableDeclaration(
    bool Mutable,
    Identifier Name,
    TypeAnnotation Type,
    Expression? Initializer
) : Declaration;
