namespace KSharp.Compiler.AST;

public record Int32Literal(int Value) : Literal;

public record Int64Literal(long Value) : Literal;

public record Float32Literal(float Value) : Literal;

public record Float64Literal(double Value) : Literal;

public record DecimalLiteral(decimal Value) : Literal;