namespace KSharp.Compiler.IR;

/// <summary>
/// Base type for all nodes in the KSharp Intermediate Representation (IR).
/// </summary>
public abstract record KsNode;

/// <summary>
/// Base type for all expressions in the IR.
/// </summary>
public abstract record KsExpression : KsNode;

/// <summary>
/// Base type for all statements in the IR.
/// </summary>
public abstract record KsStatement : KsNode;

/// <summary>
/// Base type for all declarations in the IR.
/// </summary>
public abstract record KsDeclaration : KsNode;

/// <summary>
/// Represents a top-level declaration in the IR.
/// This serves as a base type for declarations that exist at the root level.
/// </summary>
public abstract record KsTopLevelDeclaration : KsDeclaration;

/// <summary>
/// Base class for operators in the IR.
/// </summary>
public abstract record KsOperator(string Symbol) : KsNode
{
    public static KsOperator Plus => new KsPlusOperator();
}

/// <summary>
/// Interface for intrinsic operators.
/// </summary>
public interface IIntrinsicOperator;

/// <summary>
/// Plus operator (+) implementation.
/// </summary>
public record KsPlusOperator() : KsOperator("+"), IIntrinsicOperator;

/// <summary>
/// Represents a literal expression with a constant value.
/// </summary>
public record KsLiteralExpression(object Value) : KsExpression;
