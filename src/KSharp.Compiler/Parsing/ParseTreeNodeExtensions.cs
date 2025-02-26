using System.Collections.Immutable;
using Irony.Parsing;
using KSharp.Compiler.AST;

namespace KSharp.Compiler.Parsing;

public static class ParseTreeNodeExtensions
{
    public static SyntaxNode ToSyntaxNode(this ParseTreeNode node)
    {
        return node.Term.Name switch
        {
            nameof(Tokens.CompilationUnit) => node.ToCompilationUnit(),

            $"{nameof(Tokens.Statement)}+" => node.ToStatementList(),
            nameof(Tokens.Statement) => node.ToStatement(),

            nameof(Tokens.DeclarationStatement) => node.ToDeclaration(),
            nameof(Tokens.ExpressionStatement) => node.ToExpressionStatement(),
            nameof(Tokens.ValueDeclarationStatement) => node.ToValueDeclarationStatement(),
            nameof(Tokens.VariableDeclarationStatement) => node.ToVariableDeclarationStatement(),

            nameof(Tokens.Expression) => node.ToExpression(),
            nameof(Tokens.ConstantExpression) => node.ToConstantExpression(),

            nameof(Tokens.TrueToken) => node.ToBooleanLiteral(true),
            nameof(Tokens.FalseToken) => node.ToBooleanLiteral(false),

            nameof(Tokens.Integer) => node.ToIntegerLiteral(),

            nameof(Tokens.FloatingNumber) => node.ToFloatingNumberLiteral(),

            _ => throw new NotImplementedException($"Unknown term name: {node.Term.Name}")
        };
    }

    private static CompilationUnit ToCompilationUnit(this ParseTreeNode node)
    {
        var statements = node.ChildNodes
            .Select(child => child.ToSyntaxNode())
            .OfType<StatementList>()
            .SelectMany(statementList => statementList.Statements)
            .ToImmutableArray();

        return new CompilationUnit(statements);
    }

    private static StatementList ToStatementList(this ParseTreeNode node)
    {
        var statements = node.ChildNodes
            .Select(child => child.ToSyntaxNode())
            .OfType<Statement>()
            .ToImmutableArray();

        return new StatementList(statements);
    }

    private static Statement ToStatement(this ParseTreeNode node)
    {
        return (Statement)node.ChildNodes[0].ToSyntaxNode();
    }

    private static DeclarationStatement ToDeclaration(this ParseTreeNode node)
    {
        return (DeclarationStatement)node.ChildNodes[0].ToSyntaxNode();
    }

    private static ExpressionStatement ToExpressionStatement(this ParseTreeNode node)
    {
        var expression = node.ChildNodes[0].ToExpression();

        return new ExpressionStatement(expression);
    }

    private static ValueDeclarationStatement ToValueDeclarationStatement(this ParseTreeNode node)
    {
        var identifier = GetDeclarationParts(node, out var typeName, out var initializer);

        return new ValueDeclarationStatement(identifier, typeName, initializer);
    }

    private static VariableDeclarationStatement ToVariableDeclarationStatement(
        this ParseTreeNode node
    )
    {
        var identifier = GetDeclarationParts(node, out var typeName, out var initializer);

        return new VariableDeclarationStatement(identifier, typeName, initializer);
    }

    private static NameIdentifier GetDeclarationParts(
        ParseTreeNode node,
        out TypeIdentifier? typeName,
        out Expression? initializer
    )
    {
        var identifier = new NameIdentifier("???");
        typeName = null;
        initializer = null;

        foreach (var child in node.ChildNodes)
        {
            switch (child.Term.Name)
            {
                case nameof(Tokens.NameIdentifier):
                    identifier = new NameIdentifier(child.Token.ValueString);
                    break;

                case nameof(Tokens.TypeAnnotation):
                    var typeNameNode = child.ChildNodes[1];
                    typeName = new TypeIdentifier(typeNameNode.Token.ValueString);
                    break;

                case nameof(Tokens.Expression):
                    initializer = child.ToExpression();
                    break;
            }
        }

        return identifier;
    }

    private static Expression ToExpression(this ParseTreeNode node)
    {
        return (Expression)node.ChildNodes[0].ToSyntaxNode();
    }

    private static ConstantExpression ToConstantExpression(this ParseTreeNode node)
    {
        var value = (Literal)node.ChildNodes[0].ToSyntaxNode();

        return new ConstantExpression(value);
    }

    private static BoolLiteral ToBooleanLiteral(this ParseTreeNode node, bool value)
    {
        return new BoolLiteral(value);
    }

    private static Literal ToIntegerLiteral(this ParseTreeNode node)
    {
        var value = node.Token.Value;

        return value switch
        {
            int int32Value => new Int32Literal(int32Value),
            long int64Value => new Int64Literal(int64Value),
            _ => throw new NotImplementedException($"Unknown integer type: {value.GetType()}")
        };
    }

    private static Literal ToFloatingNumberLiteral(this ParseTreeNode node)
    {
        var value = node.Token.Value;

        return value switch
        {
            float floatValue => new Float32Literal(floatValue),
            double doubleValue => new Float64Literal(doubleValue),
            decimal decimalValue => new DecimalLiteral(decimalValue),
            _
                => throw new NotImplementedException(
                    $"Unknown floating number type: {value.GetType()}"
                )
        };
    }

    private static Float64Literal ToFloat64Literal(this ParseTreeNode node)
    {
        var text = node.Token.ValueString;
        var value = double.Parse(text);

        return new Float64Literal(value);
    }
}
