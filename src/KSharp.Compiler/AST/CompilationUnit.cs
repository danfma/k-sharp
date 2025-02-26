using System.Collections.Immutable;

namespace KSharp.Compiler.AST;

public sealed record CompilationUnit(ImmutableArray<Statement> Statements) : SyntaxNode;
