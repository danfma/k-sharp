using System.Collections.Immutable;

namespace KSharp.Compiler.AST;

internal sealed record StatementList(ImmutableArray<Statement> Statements) : Statement;
