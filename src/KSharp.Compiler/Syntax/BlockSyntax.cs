using System.Collections.Immutable;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Represents a code block delimited by braces
/// </summary>
public sealed record BlockSyntax(ImmutableList<StatementSyntax> Statements) : StatementSyntax;
