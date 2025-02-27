using System.Numerics;

namespace KSharp.Compiler.Ast;

public record NumberLiteralExpression<T>(T Value) : LiteralExpression, INumberLiteral
    where T : INumber<T>;