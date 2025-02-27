using Irony.Parsing;
using static KSharp.Compiler.Ast.TreeNodeTransformer;

namespace KSharp.Compiler
{
    // Definição da gramática K# usando Irony
    public class KSharpGrammar : Grammar
    {
        public KSharpGrammar() : base(caseSensitive: true)
        {
            // Terminals
            var identifier = new IdentifierTerminal(NodeNames.Identifier);
            var numberLiteral = new NumberLiteral(NodeNames.NumberLiteral);
            var stringLiteral = new StringLiteral("string", "\"", StringOptions.AllowsAllEscapes);

            // Comments
            var lineComment = new CommentTerminal(
                "lineComment",
                "//",
                "\r",
                "\n",
                "\u2085",
                "\u2028",
                "\u2029"
            );

            var blockComment = new CommentTerminal("blockComment", "/*", "*/");

            NonGrammarTerminals.Add(lineComment);
            NonGrammarTerminals.Add(blockComment);

            // Pontuation & operators
            var comma = ToTerm(",");
            var semicolon = ToTerm(";");
            var colon = ToTerm(":");
            var dot = ToTerm(".");
            var assign = ToTerm("=");

            var plus = ToTerm("+");
            var minus = ToTerm("-");
            var multiply = ToTerm("*");
            var divide = ToTerm("/");

            var braceOpen = ToTerm("{");
            var braceClose = ToTerm("}");
            var parenOpen = ToTerm("(");
            var parenClose = ToTerm(")");

            var equals = ToTerm("==");
            var lessThan = ToTerm("<");
            var lessThanOrEqual = ToTerm("<=");
            var greaterThan = ToTerm(">");
            var greaterThanOrEqual = ToTerm(">=");

            // Keywords
            var kwFun = ToTerm(KeyWord.Fun);
            var kwVal = ToTerm(KeyWord.Val);
            var kwVar = ToTerm(KeyWord.Var);
            var kwIf = ToTerm(KeyWord.If);
            var kwElse = ToTerm("else");
            var kwReturn = ToTerm(KeyWord.Return);
            var kwNamespace = ToTerm(KeyWord.Namespace);
            var kwUsing = ToTerm(KeyWord.Using);

            // Non-terminals
            var sourceFile = new NonTerminal(NodeNames.SourceFile);

            var usingList = new NonTerminal(NodeNames.UsingList);
            var usingDirective = new NonTerminal(NodeNames.UsingDirective);

            var topLevelDeclarationList = new NonTerminal(NodeNames.TopLevelDeclarationList);
            var topLevelDeclaration = new NonTerminal(NodeNames.TopLevelDeclaration);
            var namespaceDeclaration = new NonTerminal(NodeNames.NamespaceDeclaration);

            var funcDeclaration = new NonTerminal(NodeNames.FunctionDeclaration);
            var varDeclaration = new NonTerminal(NodeNames.VariableDeclaration);

            var parameterList = new NonTerminal(NodeNames.ParameterList);
            var parameter = new NonTerminal(NodeNames.Parameter);
            var typeAnnotation = new NonTerminal(NodeNames.TypeAnnotation);

            var statementList = new NonTerminal(NodeNames.StatementList);
            var statement = new NonTerminal(NodeNames.Statement);
            var blockStatement = new NonTerminal(NodeNames.BlockStatement);
            var ifStatement = new NonTerminal(NodeNames.IfStatement);
            var returnStatement = new NonTerminal(NodeNames.ReturnStatement);
            var expressionStatement = new NonTerminal(NodeNames.ExpressionStatement);

            var expression = new NonTerminal(NodeNames.Expression);
            var valueExpression = new NonTerminal(NodeNames.ValueExpression);
            var binaryExpression = new NonTerminal(NodeNames.BinaryExpression);

            var binaryOperator = new NonTerminal(NodeNames.BinaryOperator);
            var variable = new NonTerminal(NodeNames.Variable);

            var literal = new NonTerminal(NodeNames.Literal);

            var functionCall = new NonTerminal(NodeNames.FunctionCall);
            var argumentList = new NonTerminal(NodeNames.ArgumentList);
            var stringInterpolation = new NonTerminal(NodeNames.StringInterpolation);

            // Grammar rules
            sourceFile.Rule = usingList + namespaceDeclaration + topLevelDeclarationList;

            usingList.Rule = MakeStarRule(usingList, usingDirective);
            usingDirective.Rule = kwUsing + identifier;

            namespaceDeclaration.Rule = (kwNamespace + identifier) | Empty;

            topLevelDeclarationList.Rule = MakeStarRule(
                topLevelDeclarationList,
                topLevelDeclaration
            );

            topLevelDeclaration.Rule = funcDeclaration | varDeclaration;

            funcDeclaration.Rule =
                kwFun
                + identifier
                + parenOpen
                + parameterList
                + parenClose
                + typeAnnotation
                + blockStatement;

            parameterList.Rule = MakeStarRule(parameterList, comma, parameter) | Empty;
            parameter.Rule = identifier + typeAnnotation;
            typeAnnotation.Rule = colon + identifier | Empty;

            blockStatement.Rule = braceOpen + statementList + braceClose;
            statementList.Rule = MakeStarRule(statementList, statement);

            statement.Rule = varDeclaration | ifStatement | returnStatement | expressionStatement;

            varDeclaration.Rule =
                (kwVal + identifier + typeAnnotation + assign + expression)
                | (kwVar + identifier + typeAnnotation + assign + expression);

            ifStatement.Rule = kwIf + expression + blockStatement;

            returnStatement.Rule = kwReturn + expression;

            expressionStatement.Rule = expression;

            expression.Rule = valueExpression | binaryExpression;

            valueExpression.Rule =
                literal | variable | functionCall | parenOpen + expression + parenClose;

            variable.Rule = identifier;

            binaryExpression.Rule = expression + binaryOperator + expression;

            binaryOperator.Rule =
                plus
                | minus
                | multiply
                | divide
                | lessThan
                | lessThanOrEqual
                | greaterThan
                | greaterThanOrEqual
                | equals;

            literal.Rule = numberLiteral | stringLiteral | stringInterpolation;

            functionCall.Rule = identifier + parenOpen + argumentList + parenClose;

            argumentList.Rule = MakeStarRule(argumentList, comma, expression);

            stringInterpolation.Rule = ToTerm("$") + stringLiteral;

            // Operator precedence
            RegisterOperators(1, plus, minus);
            RegisterOperators(2, multiply, divide);

            // Set root, punctuation & keyword classes
            Root = sourceFile;

            MarkPunctuation(
                parenOpen,
                parenClose,
                braceOpen,
                braceClose,
                semicolon,
                comma,
                colon,
                assign
            );

            MarkReservedWords("fun", "val", "var", "if", "else", "return");
        }
    }
}
