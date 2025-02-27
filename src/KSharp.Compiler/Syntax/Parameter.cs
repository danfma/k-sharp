namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa um parâmetro de função
/// </summary>
public record Parameter(Identifier Identifier, TypeAnnotation Type) : AstNode;
