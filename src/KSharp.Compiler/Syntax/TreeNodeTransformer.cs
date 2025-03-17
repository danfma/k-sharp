using System.Collections.Immutable;
using Irony.Parsing;

namespace KSharp.Compiler.Syntax;

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

    /// <summary>
    /// Creates a CompilationSyntax from a list of parse trees
    /// </summary>
    /// <param name="parseTrees">Parse trees for each source file</param>
    /// <param name="projectName">Name of the project</param>
    /// <param name="rootDirectory">Root directory of the project</param>
    /// <returns>A CompilationSyntax object containing all source files</returns>
    public CompilationSyntax ToProject(
        IEnumerable<(ParseTree Tree, string FileName)> parseTrees,
        string projectName,
        string rootDirectory
    )
    {
        var sourceFiles = parseTrees
            .Select(item => ToSourceFile(item.Tree, item.FileName))
            .ToImmutableList();

        return new CompilationSyntax
        {
            Name = new IdentifierTokenSyntax(projectName),
            RootDirectory = rootDirectory,
            SourceFiles = sourceFiles,
        };
    }

    public CompilationUnitSyntax ToSourceFile(ParseTree parseTree, string fileName)
    {
        var rootNode = parseTree.Root;

        AssertTerm(rootNode, NodeNames.SourceFile);

        var usingList = ToUsingList(rootNode.ChildNodes[0]);
        var namespaceDeclaration = ToNamespaceDeclaration(rootNode.ChildNodes[1]);
        var topLevelDeclarationList = ToTopLevelDeclarationList(rootNode.ChildNodes[2]);

        return new CompilationUnitSyntax
        {
            FileName = fileName,
            Usings = usingList,
            Namespace = namespaceDeclaration,
            Declarations = topLevelDeclarationList,
        };
    }

    private ImmutableList<UsingDirectiveSyntax> ToUsingList(ParseTreeNode node)
    {
        return node.ChildNodes.Select(ToUsingDirective).ToImmutableList();
    }

    private UsingDirectiveSyntax ToUsingDirective(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.UsingDirective);

        var namespace_ = ToIdentifier(node.ChildNodes[1]);

        return new UsingDirectiveSyntax(namespace_);
    }

    private NamespaceDeclarationSyntax? ToNamespaceDeclaration(ParseTreeNode node)
    {
        if (node.ChildNodes.Count == 0)
            return null;

        AssertTerm(node, NodeNames.NamespaceDeclaration);

        var name = ToIdentifier(node.ChildNodes[1]);

        return new NamespaceDeclarationSyntax(name);
    }

    private ImmutableList<GlobalDeclarationSyntax> ToTopLevelDeclarationList(ParseTreeNode node)
    {
        return node.ChildNodes.Select(ToTopLevelDeclaration).ToImmutableList();
    }

    private GlobalDeclarationSyntax ToTopLevelDeclaration(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.TopLevelDeclaration);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.FunctionDeclaration => new GlobalMethodDeclarationSyntax(
                ToFunctionDeclaration(node)
            ),
            NodeNames.VariableDeclaration => new GlobalVariableDeclarationSyntax(
                ToVariableDeclaration(node)
            ),
            NodeNames.TopLevelStatement => new GlobalStatementSyntax(
                ToTopLevelStatement(node)
            ),
            _ => throw new InvalidOperationException($"Unknown declaration type: {node.Term.Name}"),
        };
    }
    
    private StatementSyntax ToTopLevelStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.TopLevelStatement);
        
        return ToStatement(node.ChildNodes[0]);
    }

    private ImmutableList<DeclarationSyntax> ToDeclarationList(ParseTreeNode node)
    {
        return node.ChildNodes.Select(ToDeclaration).ToImmutableList();
    }

    private DeclarationSyntax ToDeclaration(ParseTreeNode node)
    {
        return node.Term.Name switch
        {
            NodeNames.FunctionDeclaration => ToFunctionDeclaration(node),
            NodeNames.VariableDeclaration => ToVariableDeclaration(node),
            _ => throw new InvalidOperationException($"Unknown declaration type: {node.Term.Name}"),
        };
    }

    private MethodDeclarationSyntax ToFunctionDeclaration(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.FunctionDeclaration);

        var identifier = ToIdentifier(node.ChildNodes[1]);
        var parameterList = ToParameterList(node.ChildNodes[2]);
        var returnType = ToTypeAnnotation(node.ChildNodes[3]);
        var body = ToBlockStatement(node.ChildNodes[4]);

        return new MethodDeclarationSyntax(identifier, parameterList, returnType, body);
    }

    private BlockSyntax ToBlockStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BlockStatement);

        var statements = node.ChildNodes.SelectMany(ToStatementList).ToImmutableList();

        return new BlockSyntax(statements);
    }

    private ImmutableList<StatementSyntax> ToStatementList(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.StatementList);

        return node.ChildNodes.Select(ToStatement).ToImmutableList();
    }

    private StatementSyntax ToStatement(ParseTreeNode node)
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
            _ => throw new InvalidOperationException($"Unknown statement type: {node.Term.Name}"),
        };
    }

    private ExpressionStatementSyntax ToExpressionStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ExpressionStatement);

        var expression = ToExpression(node.ChildNodes[0]);

        return new ExpressionStatementSyntax(expression);
    }

    private ReturnStatementSyntax ToReturnStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ReturnStatement);
        AssertTerm(node.ChildNodes[0], KeyWord.Return);

        var expression = node.ChildNodes.Count == 1 ? null : ToExpression(node.ChildNodes[1]);

        return new ReturnStatementSyntax(expression);
    }

    private IfStatementSyntax ToIfStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.IfStatement);
        AssertTerm(node.ChildNodes[0], KeyWord.If);

        var condition = ToExpression(node.ChildNodes[1]);
        var blockStatement = ToBlockStatement(node.ChildNodes[2]);
        var elseClause = ToElseClause(node.ChildNodes[3]);

        return new IfStatementSyntax(condition, blockStatement, elseClause);
    }

    private ForEachStatementSyntax ToForeachStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ForeachStatement);
        AssertTerm(node.ChildNodes[0], KeyWord.Foreach);
        AssertTerm(node.ChildNodes[2], KeyWord.In);

        var itemIdentifier = ToIdentifier(node.ChildNodes[1]);
        var expression = ToExpression(node.ChildNodes[3]);
        var blockStatement = ToBlockStatement(node.ChildNodes[4]);

        return new ForEachStatementSyntax(itemIdentifier, expression, blockStatement);
    }

    private ElseClauseSyntax? ToElseClause(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ElseClause);

        if (node.ChildNodes.Count == 0)
            return null;

        node = node.ChildNodes[1];

        return node.Term.Name switch
        {
            NodeNames.BlockStatement => ToElseStatement(node),
            NodeNames.IfStatement => ToElseIfStatement(node),
            _ => throw new InvalidOperationException($"Unknown else clause type: {node.Term.Name}"),
        };
    }

    private ElseStatementSyntax ToElseStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BlockStatement);

        return new ElseStatementSyntax(ToBlockStatement(node));
    }

    private ElseIfClauseSyntax ToElseIfStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.IfStatement);

        var statement = ToIfStatement(node);

        return new ElseIfClauseSyntax(statement.Condition, statement.ThenBlock, statement.Else);
    }

    private ExpressionSyntax ToExpression(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Expression);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.ValueExpression => ToValueExpression(node),
            NodeNames.BinaryExpression => ToBinaryExpression(node),
            // NodeNames.StringInterpolation => ToStringInterpolation(node),
            _ => throw new InvalidOperationException($"Unknown expression type: {node.Term.Name}"),
        };
    }

    private ExpressionSyntax ToValueExpression(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ValueExpression);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.Variable => ToVariable(node),
            NodeNames.Literal => ToLiteral(node),
            NodeNames.FunctionCall => ToInvocation(node),
            NodeNames.Expression => ToExpression(node), // Handle expressions in parentheses
            _ => throw new InvalidOperationException(
                $"Unknown value expression type: {node.Term.Name}"
            ),
        };
    }

    private IdentifierNameSyntax ToVariable(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Variable);

        return new IdentifierNameSyntax(ToIdentifier(node.ChildNodes[0]));
    }

    private LiteralExpressionSyntax ToLiteral(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Literal);

        return node.ChildNodes[0].Term.Name switch
        {
            NodeNames.NumberLiteral => ToNumberLiteral(node.ChildNodes[0]),
            NodeNames.StringLiteral => ToStringLiteral(node.ChildNodes[0]),
            _ => throw new InvalidOperationException($"Unknown literal type: {node.Term.Name}"),
        };
    }

    private LiteralExpressionSyntax ToNumberLiteral(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.NumberLiteral);

        var tokenValue = node.Token.Value;

        return tokenValue switch
        {
            int i => new NumericLiteralExpressionSyntax<int>(i),
            double d => new NumericLiteralExpressionSyntax<double>(d),
            _ => throw new InvalidOperationException($"Unknown number literal type: {tokenValue}"),
        };
    }

    private StringLiteralExpressionSyntax ToStringLiteral(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.StringLiteral);

        var tokenValue = (string)node.Token.Value;

        return new StringLiteralExpressionSyntax(tokenValue);
    }

    private InvocationExpressionSyntax ToInvocation(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.FunctionCall);

        var identifier = ToIdentifier(node.ChildNodes[0]);
        var arguments = ToArgumentArray(node.ChildNodes[1]);

        return new InvocationExpressionSyntax(identifier, arguments);
    }

    private ImmutableArray<ExpressionSyntax> ToArgumentArray(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ArgumentList);

        return node.ChildNodes.Select(ToExpression).ToImmutableArray();
    }

    private BinaryExpressionSyntax ToBinaryExpression(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BinaryExpression);

        var left = ToExpression(node.ChildNodes[0]);
        var op = ToBinaryOperator(node.ChildNodes[1]);
        var right = ToExpression(node.ChildNodes[2]);

        return new BinaryExpressionSyntax(left, op, right);
    }

    private BinaryOperatorTokenSyntax ToBinaryOperator(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BinaryOperator);

        var token = node.ChildNodes[0].Token;

        return new BinaryOperatorTokenSyntax(token.Text);
    }

    private IdentifierTokenSyntax ToIdentifier(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Identifier);

        return new IdentifierTokenSyntax(node.Token.Text);
    }

    private ImmutableList<ParameterSyntax> ToParameterList(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ParameterList);

        return node.ChildNodes.Select(ToParameter).ToImmutableList();
    }

    private ParameterSyntax ToParameter(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Parameter);

        var identifier = ToIdentifier(node.ChildNodes[0]);
        var typeAnnotation = ToTypeAnnotation(node.ChildNodes[1]);

        return new ParameterSyntax(identifier, typeAnnotation);
    }

    private TypeClauseSyntax ToTypeAnnotation(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.TypeAnnotation);

        var identifier = node.ChildNodes.Count > 0 ? ToIdentifier(node.ChildNodes[0]) : null;

        return new TypeClauseSyntax(identifier);
    }

    private VariableDeclarationSyntax ToVariableDeclaration(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.VariableDeclaration);

        var mutable = node.ChildNodes[0].Term.Name == KeyWord.Var;
        var identifier = ToIdentifier(node.ChildNodes[1]);
        var typeAnnotation = ToTypeAnnotation(node.ChildNodes[2]);
        var initializer = node.ChildNodes.Count == 4 ? ToExpression(node.ChildNodes[3]) : null;

        return new VariableDeclarationSyntax(mutable, identifier, typeAnnotation, initializer);
    }

    public static class NodeNames
    {
        public const string SourceFile = nameof(SourceFile);
        public const string UsingList = nameof(UsingList);
        public const string UsingDirective = nameof(UsingDirective);
        public const string TopLevelDeclarationList = nameof(TopLevelDeclarationList);
        public const string TopLevelDeclaration = nameof(TopLevelDeclaration);
        public const string TopLevelStatement = nameof(TopLevelStatement);
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
