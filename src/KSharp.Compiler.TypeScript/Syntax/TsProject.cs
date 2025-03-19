using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

public record TsProject(string Name, ImmutableDictionary<string, TsSourceFile> SourceFiles)
    : TsSyntaxNode;
