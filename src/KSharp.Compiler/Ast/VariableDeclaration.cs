namespace KSharp.Compiler.Ast;

public record VariableDeclaration(
    bool Mutable,
    Identifier Name,
    TypeAnnotation Type,
    Expression? Initializer
) : Declaration;
