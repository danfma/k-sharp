namespace KSharp.Compiler.IR;

/// <summary>
/// Represents an intrinsic operator in the language.
/// These are operators that are built into the language and have special semantics.
/// </summary>
public record IrIntrinsicOperator(string Symbol) : IrOperator(Symbol), IIntrinsicOperator
{
    // Common arithmetic operators
    public static readonly IrIntrinsicOperator Plus = new("+");
    public static readonly IrIntrinsicOperator Minus = new("-");
    public static readonly IrIntrinsicOperator Multiply = new("*");
    public static readonly IrIntrinsicOperator Divide = new("/");
    public static readonly IrIntrinsicOperator Modulo = new("%");
    
    // Comparison operators
    public static readonly IrIntrinsicOperator Equal = new("==");
    public static readonly IrIntrinsicOperator NotEqual = new("!=");
    public static readonly IrIntrinsicOperator LessThan = new("<");
    public static readonly IrIntrinsicOperator LessThanOrEqual = new("<=");
    public static readonly IrIntrinsicOperator GreaterThan = new(">");
    public static readonly IrIntrinsicOperator GreaterThanOrEqual = new(">=");
    
    // Logical operators
    public static readonly IrIntrinsicOperator And = new("&&");
    public static readonly IrIntrinsicOperator Or = new("||");
    
    // Bitwise operators  
    public static readonly IrIntrinsicOperator BitwiseAnd = new("&");
    public static readonly IrIntrinsicOperator BitwiseOr = new("|");
    public static readonly IrIntrinsicOperator BitwiseXor = new("^");
}