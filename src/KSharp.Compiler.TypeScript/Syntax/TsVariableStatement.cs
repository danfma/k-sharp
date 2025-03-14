using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Statement de declaração de variável
/// </summary>
public record TsVariableStatement(
    TsVariableDeclarationKind Kind,
    ImmutableArray<TsVariableDeclaration> Declarations
) : TsDeclaration;
