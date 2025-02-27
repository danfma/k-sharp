namespace KSharp.Compiler.Ast;

/// <summary>
/// Representa um parâmetro de função
/// </summary>
public record Parameter(Identifier Identifier, TypeAnnotation Type) : AstNode;
