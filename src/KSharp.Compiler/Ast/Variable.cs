namespace KSharp.Compiler.Ast;

public record Variable(Identifier Name) : ValueExpression;