using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// A module represents a class that groups declarations of functions and variables defined directly
/// in the root scope of a file (not inside a class, for example).
/// </summary>
public record KsModule : KsTopLevelDeclaration
{
    public required KsFullName FullName { get; init; }
    public ImmutableList<KsVariable> Variables { get; init; } = [];
    public ImmutableList<KsFunction> Functions { get; init; } = [];
}