using System.Linq;
using FluentAssertions;
using KSharp.Compiler.AST;
using KSharp.Compiler.Parsing;

namespace KSharp.Tests.Parsing;

public class ExpressionStatementParsingTests
{
    private static Expression ParseExpression(string code)
    {
        var compilationUnit = new SoilParser().ParseCompilationUnit(code);
        var statement = compilationUnit.Statements.OfType<ExpressionStatement>().First();
        var expression = statement.Expression;

        return expression;
    }

    [Fact]
    public void ParseBoolean()
    {
        ParseExpression("true").Should().Be(new ConstantExpression(new BoolLiteral(true)));
        ParseExpression("false").Should().Be(new ConstantExpression(new BoolLiteral(false)));
    }

    [Fact]
    public void ParseInt32()
    {
        ParseExpression("1").Should().Be(new ConstantExpression(new Int32Literal(1)));
        ParseExpression("123").Should().Be(new ConstantExpression(new Int32Literal(123)));
        ParseExpression("-123").Should().Be(new ConstantExpression(new Int32Literal(-123)));
    }

    [Fact]
    public void ParseInt64()
    {
        ParseExpression("1l").Should().Be(new ConstantExpression(new Int64Literal(1)));
        ParseExpression("123l").Should().Be(new ConstantExpression(new Int64Literal(123)));
        ParseExpression("-123l").Should().Be(new ConstantExpression(new Int64Literal(-123)));
    }

    [Fact]
    public void ParseFloat32()
    {
        ParseExpression("1f").Should().Be(new ConstantExpression(new Float32Literal(1f)));
        ParseExpression("123.5f").Should().Be(new ConstantExpression(new Float32Literal(123.5f)));
        ParseExpression("-123.99999f")
            .Should()
            .Be(new ConstantExpression(new Float32Literal(-123.99999f)));
    }

    [Fact]
    public void ParseFloat64()
    {
        ParseExpression("1.0").Should().Be(new ConstantExpression(new Float64Literal(1.0)));
        ParseExpression("123.5").Should().Be(new ConstantExpression(new Float64Literal(123.5)));
        ParseExpression("-123.99999")
            .Should()
            .Be(new ConstantExpression(new Float64Literal(-123.99999)));

        ParseExpression("1.0d").Should().Be(new ConstantExpression(new Float64Literal(1.0)));
        ParseExpression("123.5d").Should().Be(new ConstantExpression(new Float64Literal(123.5)));
        ParseExpression("-123.99999d")
            .Should()
            .Be(new ConstantExpression(new Float64Literal(-123.99999)));
    }

    [Fact]
    public void ParseDecimal()
    {
        ParseExpression("1m").Should().Be(new ConstantExpression(new DecimalLiteral(1m)));
        ParseExpression("123.5m").Should().Be(new ConstantExpression(new DecimalLiteral(123.5m)));
        ParseExpression("-123.99999m")
            .Should()
            .Be(new ConstantExpression(new DecimalLiteral(-123.99999m)));
    }
}
