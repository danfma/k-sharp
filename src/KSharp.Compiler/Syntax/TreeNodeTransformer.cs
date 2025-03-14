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
    /// Cria um KsSyntaxProject a partir de uma lista de ParseTrees
    /// </summary>
    /// <param name="parseTrees">Árvores de análise sintática para cada arquivo</param>
    /// <param name="projectName">Nome do projeto</param>
    /// <param name="rootDirectory">Diretório raiz do projeto</param>
    /// <returns>Um objeto KsSyntaxProject contendo todos os arquivos fonte</returns>
    public KsProjectSyntax ToProject(
        IEnumerable<(ParseTree Tree, string FileName)> parseTrees,
        string projectName,
        string rootDirectory
    )
    {
        var sourceFiles = parseTrees
            .Select(item => ToSourceFile(item.Tree, item.FileName))
            .ToImmutableList();

        return new KsProjectSyntax
        {
            Name = new KsIdentifierSyntax(projectName),
            RootDirectory = rootDirectory,
            SourceFiles = sourceFiles,
        };
    }

    public KsSourceFileSyntax ToSourceFile(ParseTree parseTree, string fileName)
    {
        var rootNode = parseTree.Root;

        AssertTerm(rootNode, NodeNames.SourceFile);

        var usingList = ToUsingList(rootNode.ChildNodes[0]);
        var namespaceDeclaration = ToNamespaceDeclaration(rootNode.ChildNodes[1]);
        var topLevelDeclarationList = ToTopLevelDeclarationList(rootNode.ChildNodes[2]);

        return new KsSourceFileSyntax
        {
            FileName = fileName,
            Usings = usingList,
            Namespace = namespaceDeclaration,
            Declarations = topLevelDeclarationList,
        };
    }

    private ImmutableList<KsUsingDirectiveSyntax> ToUsingList(ParseTreeNode node)
    {
        return node.ChildNodes.Select(ToUsingDirective).ToImmutableList();
    }

    private KsUsingDirectiveSyntax ToUsingDirective(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.UsingDirective);

        var namespace_ = ToIdentifier(node.ChildNodes[1]);

        return new KsUsingDirectiveSyntax(namespace_);
    }

    private KsNamespaceDeclarationSyntax? ToNamespaceDeclaration(ParseTreeNode node)
    {
        if (node.ChildNodes.Count == 0)
            return null;

        AssertTerm(node, NodeNames.NamespaceDeclaration);

        var name = ToIdentifier(node.ChildNodes[1]);

        return new KsNamespaceDeclarationSyntax(name);
    }

    private ImmutableList<KsTopLevelDeclarationSyntax> ToTopLevelDeclarationList(ParseTreeNode node)
    {
        return node.ChildNodes.Select(ToTopLevelDeclaration).ToImmutableList();
    }

    private KsTopLevelDeclarationSyntax ToTopLevelDeclaration(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.TopLevelDeclaration);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.FunctionDeclaration => new KsTopLevelFunctionDeclarationSyntax(
                ToFunctionDeclaration(node)
            ),
            NodeNames.VariableDeclaration => new KsTopLevelVariableDeclarationSyntax(
                ToVariableDeclaration(node)
            ),
            _ => throw new InvalidOperationException($"Unknown declaration type: {node.Term.Name}"),
        };
    }

    private ImmutableList<KsDeclarationSyntax> ToDeclarationList(ParseTreeNode node)
    {
        return node.ChildNodes.Select(ToDeclaration).ToImmutableList();
    }

    private KsDeclarationSyntax ToDeclaration(ParseTreeNode node)
    {
        return node.Term.Name switch
        {
            NodeNames.FunctionDeclaration => ToFunctionDeclaration(node),
            NodeNames.VariableDeclaration => ToVariableDeclaration(node),
            _ => throw new InvalidOperationException($"Unknown declaration type: {node.Term.Name}"),
        };
    }

    private KsFunctionDeclarationSyntax ToFunctionDeclaration(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.FunctionDeclaration);

        var identifier = ToIdentifier(node.ChildNodes[1]);
        var parameterList = ToParameterList(node.ChildNodes[2]);
        var returnType = ToTypeAnnotation(node.ChildNodes[3]);
        var body = ToBlockStatement(node.ChildNodes[4]);

        return new KsFunctionDeclarationSyntax(identifier, parameterList, returnType, body);
    }

    private KsBlockStatementSyntax ToBlockStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BlockStatement);

        var statements = node.ChildNodes.SelectMany(ToStatementList).ToImmutableList();

        return new KsBlockStatementSyntax(statements);
    }

    private ImmutableList<KsStatementSyntax> ToStatementList(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.StatementList);

        return node.ChildNodes.Select(ToStatement).ToImmutableList();
    }

    private KsStatementSyntax ToStatement(ParseTreeNode node)
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

    private KsExpressionStatementSyntax ToExpressionStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ExpressionStatement);

        var expression = ToExpression(node.ChildNodes[0]);

        return new KsExpressionStatementSyntax(expression);
    }

    private KsReturnStatementSyntax ToReturnStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ReturnStatement);
        AssertTerm(node.ChildNodes[0], KeyWord.Return);

        var expression = node.ChildNodes.Count == 1 ? null : ToExpression(node.ChildNodes[1]);

        return new KsReturnStatementSyntax(expression);
    }

    private KsIfStatementSyntax ToIfStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.IfStatement);
        AssertTerm(node.ChildNodes[0], KeyWord.If);

        var condition = ToExpression(node.ChildNodes[1]);
        var blockStatement = ToBlockStatement(node.ChildNodes[2]);
        var elseClause = ToElseClause(node.ChildNodes[3]);

        return new KsIfStatementSyntax(condition, blockStatement, elseClause);
    }

    private KsForeachStatementSyntax ToForeachStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ForeachStatement);
        AssertTerm(node.ChildNodes[0], KeyWord.Foreach);
        AssertTerm(node.ChildNodes[2], KeyWord.In);

        var itemIdentifier = ToIdentifier(node.ChildNodes[1]);
        var expression = ToExpression(node.ChildNodes[3]);
        var blockStatement = ToBlockStatement(node.ChildNodes[4]);

        return new KsForeachStatementSyntax(itemIdentifier, expression, blockStatement);
    }

    private KsElseClauseSyntax? ToElseClause(ParseTreeNode node)
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

    private KsElseStatementSyntax ToElseStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BlockStatement);

        return new KsElseStatementSyntax(ToBlockStatement(node));
    }

    private KsElseIfClauseSyntax ToElseIfStatement(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.IfStatement);

        var statement = ToIfStatement(node);

        return new KsElseIfClauseSyntax(statement.Condition, statement.Block, statement.Else);
    }

    private KsExpressionSyntax ToExpression(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Expression);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.ValueExpression => ToValueExpression(node),
            NodeNames.BinaryExpression => ToBinaryExpression(node),
            // NodeNames.FunctionCall => ToFunctionCall(node),
            // NodeNames.StringInterpolation => ToStringInterpolation(node),
            _ => throw new InvalidOperationException($"Unknown expression type: {node.Term.Name}"),
        };
    }

    private KsValueExpressionSyntax ToValueExpression(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ValueExpression);

        node = node.ChildNodes[0];

        return node.Term.Name switch
        {
            NodeNames.Variable => ToVariable(node),
            NodeNames.Literal => ToLiteral(node),
            NodeNames.FunctionCall => ToFunctionCall(node),
            _ => throw new InvalidOperationException(
                $"Unknown value expression type: {node.Term.Name}"
            ),
        };
    }

    private KsVariableExpressionSyntax ToVariable(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Variable);

        return new KsVariableExpressionSyntax(ToIdentifier(node.ChildNodes[0]));
    }

    private KsLiteralExpressionSyntax ToLiteral(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Literal);

        return node.ChildNodes[0].Term.Name switch
        {
            NodeNames.NumberLiteral => ToNumberLiteral(node.ChildNodes[0]),
            NodeNames.StringLiteral => ToStringLiteral(node.ChildNodes[0]),
            _ => throw new InvalidOperationException($"Unknown literal type: {node.Term.Name}"),
        };
    }

    private KsLiteralExpressionSyntax ToNumberLiteral(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.NumberLiteral);

        var tokenValue = node.Token.Value;

        return tokenValue switch
        {
            int i => new KsNumberLiteralExpressionSyntax<int>(i),
            double d => new KsNumberLiteralExpressionSyntax<double>(d),
            _ => throw new InvalidOperationException($"Unknown number literal type: {tokenValue}"),
        };
    }

    private KsStringLiteralExpressionSyntax ToStringLiteral(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.StringLiteral);

        var tokenValue = (string)node.Token.Value;

        return new KsStringLiteralExpressionSyntax(tokenValue);
    }

    private KsValueExpressionSyntax ToFunctionCall(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.FunctionCall);

        var identifier = ToIdentifier(node.ChildNodes[0]);
        var arguments = ToArgumentArray(node.ChildNodes[1]);

        return new KsFunctionCallExpressionSyntax(identifier, arguments);
    }

    private ImmutableArray<KsExpressionSyntax> ToArgumentArray(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ArgumentList);

        return node.ChildNodes.Select(ToExpression).ToImmutableArray();
    }

    private KsBinaryExpressionSyntax ToBinaryExpression(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BinaryExpression);

        var left = ToExpression(node.ChildNodes[0]);
        var op = ToBinaryOperator(node.ChildNodes[1]);
        var right = ToExpression(node.ChildNodes[2]);

        return new KsBinaryExpressionSyntax(left, op, right);
    }

    private KsBinaryOperatorSyntax ToBinaryOperator(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.BinaryOperator);

        var token = node.ChildNodes[0].Token;

        return new KsBinaryOperatorSyntax(token.Text);
    }

    private KsIdentifierSyntax ToIdentifier(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Identifier);

        return new KsIdentifierSyntax(node.Token.Text);
    }

    private ImmutableList<KsParameterSyntax> ToParameterList(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.ParameterList);

        return node.ChildNodes.Select(ToParameter).ToImmutableList();
    }

    private KsParameterSyntax ToParameter(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.Parameter);

        var identifier = ToIdentifier(node.ChildNodes[0]);
        var typeAnnotation = ToTypeAnnotation(node.ChildNodes[1]);

        return new KsParameterSyntax(identifier, typeAnnotation);
    }

    private KsTypeAnnotationSyntax ToTypeAnnotation(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.TypeAnnotation);

        var identifier = node.ChildNodes.Count > 0 ? ToIdentifier(node.ChildNodes[0]) : null;

        return new KsTypeAnnotationSyntax(identifier);
    }

    private KsVariableDeclarationSyntax ToVariableDeclaration(ParseTreeNode node)
    {
        AssertTerm(node, NodeNames.VariableDeclaration);

        var mutable = node.ChildNodes[0].Term.Name == KeyWord.Var;
        var identifier = ToIdentifier(node.ChildNodes[1]);
        var typeAnnotation = ToTypeAnnotation(node.ChildNodes[2]);
        var initializer = node.ChildNodes.Count == 4 ? ToExpression(node.ChildNodes[3]) : null;

        return new KsVariableDeclarationSyntax(mutable, identifier, typeAnnotation, initializer);
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
