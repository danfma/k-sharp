using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

public record KsBlock(ImmutableList<KsStatement> Statements) : KsNode;