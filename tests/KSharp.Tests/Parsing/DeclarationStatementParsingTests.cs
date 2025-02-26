using System.Linq;
using FluentAssertions;
using KSharp.Compiler.AST;
using KSharp.Compiler.Parsing;

namespace KSharp.Tests.Parsing;

public class DeclarationStatementParsingTests
{
    private static DeclarationStatement ParseStatement(string text)
    {
        var compilationUnit = new SoilParser().ParseCompilationUnit(text);
        var statement = compilationUnit.Statements.OfType<DeclarationStatement>().First();

        return statement;
    }

    [Fact]
    public void ParseValueDeclaration()
    {
        ParseStatement("let a: Bool = true")
            .Should()
            .Be(
                new ValueDeclarationStatement(
                    Name: new NameIdentifier("a"),
                    TypeName: TypeIdentifier.Bool,
                    Initializer: new ConstantExpression(new BoolLiteral(true))
                )
            );
    }

    [Fact]
    public void ParseValueDeclarationWithoutTypeAnnotation()
    {
        ParseStatement("let a = true")
            .Should()
            .Be(
                new ValueDeclarationStatement(
                    Name: new NameIdentifier("a"),
                    TypeName: null,
                    Initializer: new ConstantExpression(new BoolLiteral(true))
                )
            );
    }

    [Fact]
    public void ParseValueDeclarationIgnoringWhiteSpaces()
    {
        ParseStatement("let a:Bool=true")
            .Should()
            .Be(
                new ValueDeclarationStatement(
                    Name: new NameIdentifier("a"),
                    TypeName: TypeIdentifier.Bool,
                    Initializer: new ConstantExpression(new BoolLiteral(true))
                )
            );
    }

    [Fact]
    public void ParseVariableDeclaration()
    {
        ParseStatement("var a: Bool = true")
            .Should()
            .Be(
                new VariableDeclarationStatement(
                    Name: new NameIdentifier("a"),
                    TypeName: TypeIdentifier.Bool,
                    Initializer: new ConstantExpression(new BoolLiteral(true))
                )
            );
    }

    [Fact]
    public void ParseVariableDeclarationWithoutTypeAnnotation()
    {
        ParseStatement("var a = true")
            .Should()
            .Be(
                new VariableDeclarationStatement(
                    Name: new NameIdentifier("a"),
                    TypeName: null,
                    Initializer: new ConstantExpression(new BoolLiteral(true))
                )
            );
    }

    [Fact]
    public void ParseVariableDeclarationIgnoringWhiteSpaces()
    {
        ParseStatement("var a:Bool=true")
            .Should()
            .Be(
                new VariableDeclarationStatement(
                    Name: new NameIdentifier("a"),
                    TypeName: TypeIdentifier.Bool,
                    Initializer: new ConstantExpression(new BoolLiteral(true))
                )
            );
    }
}
