using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa uma declaração de função
/// </summary>
public record FunctionDeclaration(
    Identifier Identifier,
    ImmutableList<Parameter> Parameters,
    TypeAnnotation ReturnType,
    BlockStatement Body
) : Declaration;
