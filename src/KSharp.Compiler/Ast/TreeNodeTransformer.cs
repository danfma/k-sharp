using System.Collections.Immutable;
using Irony.Parsing;

namespace KSharp.Compiler.Ast;

public class TreeNodeTransformer
{
    private static void AssertTerm(ParseTreeNode node, string name)
    {
        if (node.Term.Name != name)
        {
            throw new InvalidOperationException(
                $"Expected node of type {name}, but got {node.Term.Name}"
            );
        }
    }

    public SourceFile ToSourceFile(ParseTree parseTree, string fileName)
    {
        var rootNode = parseTree.Root;

        AssertTerm(rootNode, NodeNames.SourceFile);

        var usingList = ToUsingList(rootNode.ChildNodes[0]);
        var namespaceDeclaration = ToNamespaceDeclaration(rootNode.ChildNodes[1]);
        var topLevelDeclarationList = ToTopLevelDeclarationList(rootNode.ChildNodes[2]);

        return new SourceFile
        {
            FileName = fileName,
            Usings = usingList,
            Namespace = namespaceDeclaration,
            Declarations = topLevelDeclarationList
        };
    }

    private ImmutableList<UsingDirective> ToUsingList(ParseTreeNode node)
    {
        return node.ChildNodes.Select(ToUsingDirective).ToImmutableList();
    }

    private UsingDirective ToUsingDirective(ParseTreeNode node)
    {
        throw new NotImplementedException();
    }

    private NamespaceDeclaration? ToNamespaceDeclaration(ParseTreeNode node)
    {
        if (node.ChildNodes.Count == 0)
            return null;

        throw new NotImplementedException();
    }

    private ImmutableList<TopLevelDeclaration> ToTopLevelDeclarationList(ParseTreeNode node)
    {
        return node.ChildNodes.Select(ToTopLevelDeclaration).ToImmutableList();
    }

    private TopLevelDeclaration ToTopLevelDeclaration(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.TopLevelDeclaration);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.FunctionDeclaration
                => new TopLevelFunctionDeclaration(ToFunctionDeclaration(node)),
            NodeNames.VariableDeclaration
                => new TopLevelVariableDeclaration(ToVariableDeclaration(node)),
            _ => throw new InvalidOperationException($"Unknown declaration type: {node.Term.Name}")
        };
    }

    private ImmutableList<Declaration> ToDeclarationList(ParseTreeNode node)
    {
        return node.ChildNodes.Select(ToDeclaration).ToImmutableList();
    }

    private Declaration ToDeclaration(ParseTreeNode node)
    {
        return node.Term.Name switch
        {
            NodeNames.FunctionDeclaration => ToFunctionDeclaration(node),
            NodeNames.VariableDeclaration => ToVariableDeclaration(node),
            _ => throw new InvalidOperationException($"Unknown declaration type: {node.Term.Name}")
        };
    }

    private FunctionDeclaration ToFunctionDeclaration(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.FunctionDeclaration);

        var identifier = ToIdentifier(node.ChildNodes[1]);
        var parameterList = ToParameterList(node.ChildNodes[2]);
        var returnType = ToTypeAnnotation(node.ChildNodes[3]);
        var body = ToBlockStatement(node.ChildNodes[4]);

        return new FunctionDeclaration(identifier, parameterList, returnType, body);
    }

    private BlockStatement ToBlockStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BlockStatement);

        var statements = node.ChildNodes.SelectMany(ToStatementList).ToImmutableList();

        return new BlockStatement(statements);
    }

    private ImmutableList<Statement> ToStatementList(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.StatementList);

        return node.ChildNodes.Select(ToStatement).ToImmutableList();
    }

    private Statement ToStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Statement);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.VariableDeclaration => ToVariableDeclaration(node),
            NodeNames.ReturnStatement => ToReturnStatement(node),
            NodeNames.ExpressionStatement => ToExpressionStatement(node),
            NodeNames.IfStatement => ToIfStatement(node),
            NodeNames.ForeachStatement => ToForeachStatement(node),
            _ => throw new InvalidOperationException($"Unknown statement type: {node.Term.Name}")
        };
    }

    private ExpressionStatement ToExpressionStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ExpressionStatement);

        var expression = ToExpression(node.ChildNodes[0]);

        return new ExpressionStatement(expression);
    }

    private ReturnStatement ToReturnStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ReturnStatement);
        AssertTerm(node.ChildNodes[0], KeyWord.Return);

        var expression = node.ChildNodes.Count == 1 ? null : ToExpression(node.ChildNodes[1]);

        return new ReturnStatement(expression);
    }

    private IfStatement ToIfStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.IfStatement);
        AssertTerm(node.ChildNodes[0], KeyWord.If);

        var condition = ToExpression(node.ChildNodes[1]);
        var blockStatement = ToBlockStatement(node.ChildNodes[2]);
        var elseClause = ToElseClause(node.ChildNodes[3]);

        return new IfStatement(condition, blockStatement, elseClause);
    }

    private ForeachStatement ToForeachStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ForeachStatement);
        AssertTerm(node.ChildNodes[0], KeyWord.Foreach);
        AssertTerm(node.ChildNodes[2], KeyWord.In);

        var itemIdentifier = ToIdentifier(node.ChildNodes[1]);
        var expression = ToExpression(node.ChildNodes[3]);
        var blockStatement = ToBlockStatement(node.ChildNodes[4]);

        return new ForeachStatement(itemIdentifier, expression, blockStatement);
    }

    private ElseClause? ToElseClause(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ElseClause);

        if (node.ChildNodes.Count == 0)
            return null;

        node = node.ChildNodes[1];

        return node.Term.Name switch
        {
            NodeNames.BlockStatement => ToElseStatement(node),
            NodeNames.IfStatement => ToElseIfStatement(node),
            _ => throw new InvalidOperationException($"Unknown else clause type: {node.Term.Name}")
        };
    }

    private ElseStatement ToElseStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BlockStatement);

        return new ElseStatement(ToBlockStatement(node));
    }

    private ElseIfStatement ToElseIfStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.IfStatement);

        var statement = ToIfStatement(node);

        return new ElseIfStatement(statement.Condition, statement.BlockStatement, statement.Else);
    }

    private Expression ToExpression(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Expression);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.ValueExpression => ToValueExpression(node),
            NodeNames.BinaryExpression => ToBinaryExpression(node),
            // NodeNames.FunctionCall => ToFunctionCall(node),
            // NodeNames.StringInterpolation => ToStringInterpolation(node),
            _ => throw new InvalidOperationException($"Unknown expression type: {node.Term.Name}")
        };
    }

    private ValueExpression ToValueExpression(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ValueExpression);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.Variable => ToVariable(node),
            NodeNames.Literal => ToLiteral(node),
            NodeNames.FunctionCall => ToFunctionCall(node),
            _
                => throw new InvalidOperationException(
                    $"Unknown value expression type: {node.Term.Name}"
                )
        };
    }

    private Variable ToVariable(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Variable);

        return new Variable(ToIdentifier(node.ChildNodes[0]));
    }

    private LiteralExpression ToLiteral(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Literal);

        return node.ChildNodes[0].Term.Name switch
        {
            NodeNames.NumberLiteral => ToNumberLiteral(node.ChildNodes[0]),
            NodeNames.StringLiteral => ToStringLiteral(node.ChildNodes[0]),
            _ => throw new InvalidOperationException($"Unknown literal type: {node.Term.Name}")
        };
    }

    private LiteralExpression ToNumberLiteral(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.NumberLiteral);

        var tokenValue = node.Token.Value;

        return tokenValue switch
        {
            int i => new NumberLiteralExpression<int>(i),
            double d => new NumberLiteralExpression<double>(d),
            _ => throw new InvalidOperationException($"Unknown number literal type: {tokenValue}")
        };
    }

    private LiteralExpression ToStringLiteral(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.StringLiteral);

        var tokenValue = (string)node.Token.Value;

        return new StringLiteralExpression(tokenValue);
    }

    private ValueExpression ToFunctionCall(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.FunctionCall);

        var identifier = ToIdentifier(node.ChildNodes[0]);
        var arguments = ToArgumentArray(node.ChildNodes[1]);

        return new FunctionCallExpression(identifier, arguments);
    }

    private ImmutableArray<Expression> ToArgumentArray(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ArgumentList);

        return node.ChildNodes.Select(ToExpression).ToImmutableArray();
    }

    private BinaryExpression ToBinaryExpression(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BinaryExpression);

        var left = ToExpression(node.ChildNodes[0]);
        var op = ToBinaryOperator(node.ChildNodes[1]);
        var right = ToExpression(node.ChildNodes[2]);

        return new BinaryExpression(left, op, right);
    }

    private BinaryOperator ToBinaryOperator(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BinaryOperator);

        var token = node.ChildNodes[0].Token;

        return new BinaryOperator(token.Text);
    }

    private Identifier ToIdentifier(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Identifier);

        return new Identifier(node.Token.Text);
    }

    private ImmutableList<Parameter> ToParameterList(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ParameterList);

        return node.ChildNodes.Select(ToParameter).ToImmutableList();
    }

    private Parameter ToParameter(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Parameter);

        var identifier = ToIdentifier(node.ChildNodes[0]);
        var typeAnnotation = ToTypeAnnotation(node.ChildNodes[1]);

        return new Parameter(identifier, typeAnnotation);
    }

    private TypeAnnotation ToTypeAnnotation(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.TypeAnnotation);

        var identifier = node.ChildNodes.Count > 0 ? ToIdentifier(node.ChildNodes[0]) : null;

        return new TypeAnnotation(identifier);
    }

    private VariableDeclaration ToVariableDeclaration(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.VariableDeclaration);

        var mutable = node.ChildNodes[0].Term.Name == KeyWord.Var;
        var identifier = ToIdentifier(node.ChildNodes[1]);
        var typeAnnotation = ToTypeAnnotation(node.ChildNodes[2]);
        var initializer = node.ChildNodes.Count == 4 ? ToExpression(node.ChildNodes[3]) : null;

        return new VariableDeclaration(mutable, identifier, typeAnnotation, initializer);
    }

    public static class NodeNames
    {
        public const string SourceFile = nameof(SourceFile);
        public const string UsingList = nameof(UsingList);
        public const string UsingDirective = nameof(UsingDirective);
        public const string TopLevelDeclarationList = nameof(TopLevelDeclarationList);
        public const string TopLevelDeclaration = nameof(TopLevelDeclaration);
        public const string NamespaceDeclaration = nameof(NamespaceDeclaration);
        public const string FunctionDeclaration = nameof(FunctionDeclaration);
        public const string VariableDeclaration = nameof(VariableDeclaration);
        public const string ParameterList = nameof(ParameterList);
        public const string Parameter = nameof(Parameter);
        public const string TypeAnnotation = nameof(TypeAnnotation);
        public const string StatementList = nameof(StatementList);
        public const string Statement = nameof(Statement);
        public const string BlockStatement = nameof(BlockStatement);
        public const string IfStatement = nameof(IfStatement);
        public const string ElseClause = nameof(ElseClause);
        public const string ReturnStatement = nameof(ReturnStatement);
        public const string ForeachStatement = nameof(ForeachStatement);
        public const string ExpressionStatement = nameof(ExpressionStatement);
        public const string Expression = nameof(Expression);
        public const string ValueExpression = nameof(ValueExpression);
        public const string BinaryExpression = nameof(BinaryExpression);
        public const string BinaryOperator = nameof(BinaryOperator);
        public const string Variable = nameof(Variable);
        public const string Literal = nameof(Literal);
        public const string NumberLiteral = nameof(NumberLiteral);
        public const string StringLiteral = nameof(StringLiteral);
        public const string FunctionCall = nameof(FunctionCall);
        public const string ArgumentList = nameof(ArgumentList);
        public const string StringInterpolation = nameof(StringInterpolation);
        public const string Identifier = nameof(Identifier);
    }

    public static class KeyWord
    {
        public const string Fun = "fun";
        public const string Using = "using";
        public const string Namespace = "namespace";
        public const string Return = "return";
        public const string Var = "var";
        public const string Val = "val";
        public const string If = "if";
        public const string Else = "else";
        public const string Foreach = "foreach";
        public const string In = "in";
    }

    public static class Operator
    {
        public const string Add = "+";
        public const string Subtract = "-";
        public const string Multiply = "*";
        public const string Divide = "/";
        public const string Modulo = "%";

        public const string Range = "..";

        public const string Assign = "=";
        public const string AddAndAssign = "+=";
        public const string SubtractAndAssign = "-=";
        public const string MultiplyAndAssign = "*=";
        public const string DivideAndAssign = "/=";
        public const string ModuloAndAssign = "%=";
    }
}
