using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Representa um bloco de código delimitado por chaves
/// </summary>
public sealed record BlockStatement(ImmutableList<Statement> Statements) : Statement;
