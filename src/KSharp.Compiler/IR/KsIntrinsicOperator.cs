namespace KSharp.Compiler.IR;

/// <summary>
/// Represents an intrinsic operator in the language.
/// These are operators that are built into the language and have special semantics.
/// </summary>
public record KsIntrinsicOperator(string Symbol) : KsOperator(Symbol), IIntrinsicOperator
{
    // Common arithmetic operators
    public static readonly KsIntrinsicOperator Plus = new("+");
    public static readonly KsIntrinsicOperator Minus = new("-");
    public static readonly KsIntrinsicOperator Multiply = new("*");
    public static readonly KsIntrinsicOperator Divide = new("/");
    public static readonly KsIntrinsicOperator Modulo = new("%");
    
    // Comparison operators
    public static readonly KsIntrinsicOperator Equal = new("==");
    public static readonly KsIntrinsicOperator NotEqual = new("!=");
    public static readonly KsIntrinsicOperator LessThan = new("<");
    public static readonly KsIntrinsicOperator LessThanOrEqual = new("<=");
    public static readonly KsIntrinsicOperator GreaterThan = new(">");
    public static readonly KsIntrinsicOperator GreaterThanOrEqual = new(">=");
    
    // Logical operators
    public static readonly KsIntrinsicOperator And = new("&&");
    public static readonly KsIntrinsicOperator Or = new("||");
    
    // Bitwise operators  
    public static readonly KsIntrinsicOperator BitwiseAnd = new("&");
    public static readonly KsIntrinsicOperator BitwiseOr = new("|");
    public static readonly KsIntrinsicOperator BitwiseXor = new("^");
}