using System.Numerics;

namespace KSharp.Compiler.Syntax;

public record NumberLiteralExpression<T>(T Value) : LiteralExpression, INumberLiteral
    where T : INumber<T>;
