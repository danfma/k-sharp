using System.Collections.Immutable;

namespace KSharp.Compiler.IR.High;

public abstract record KsNode;

public abstract record KsExpression : KsNode;

public abstract record KsStatement : KsNode;

public record KsBlock(ImmutableList<KsStatement> Statements) : KsNode;

// Exemplos de statements
public record KsReturnStatement(KsExpression? Expression) : KsStatement;

public record KsIfStatement(KsExpression Condition, KsBlock ThenBlock, KsBlock? ElseBlock)
    : KsStatement;

// Exemplos de expressões
public record KsBinaryExpression(KsExpression Left, string Operator, KsExpression Right)
    : KsExpression;

public record KsLiteralExpression(object Value) : KsExpression;

public abstract record KsDeclaration : KsNode
{
    public required string Name { get; init; }
    public required string Namespace { get; init; }
}

/// <summary>
/// Represents a top-level declaration in the KSharp Intermediate Representation (IR).
/// This serves as a base type for declarations that exist at the root level, such as modules.
/// </summary>
public abstract record KsTopLevelDeclaration : KsDeclaration;

/// <summary>
/// A module represents a class that groups declarations of functions and variables defined directly
/// in the root scope of a file (not inside a class, for example).
/// </summary>
public record KsModule : KsTopLevelDeclaration
{
    public ImmutableList<KsVariable> Variables { get; init; } = [];
    public ImmutableList<KsFunction> Functions { get; init; } = [];
}

public abstract record KsType : KsTopLevelDeclaration
{
    public ImmutableList<KsTypeReference> Implements { get; init; } = [];
}

public record KsClass : KsType
{
    public KsTypeReference? BaseType { get; init; }
    public ImmutableList<KsProperty> Properties { get; init; } = [];
    public ImmutableList<KsFunction> Methods { get; init; } = [];
}

public record KsInterface : KsType
{
    public ImmutableList<KsProperty> Properties { get; init; } = [];
    public ImmutableList<KsMethod> Methods { get; init; } = [];
}

public record KsProperty : KsNode;

public record KsMethod : KsNode;

public record KsVariable : KsTopLevelDeclaration
{
    public required KsTypeReference Type { get; init; }
}

public record KsTypeReference
{
    public required string Name { get; init; }
    public required string Namespace { get; init; }
    public required string? Assembly { get; init; }
    public ImmutableList<KsTypeReference> GenericArguments { get; init; } = [];
}

public record KsParameter(string Name, KsTypeReference Type) : KsNode;

public record KsFunction : KsTopLevelDeclaration
{
    public required KsTypeReference ReturnType { get; init; }
    public ImmutableList<KsParameter> Parameters { get; init; } = [];
    public required KsBlock Body { get; init; }
}

public record KsProject(string Name) : KsNode
{
    public ImmutableList<KsModule> Modules { get; init; } = [];
    public ImmutableList<KsType> Types { get; init; } = [];
}
