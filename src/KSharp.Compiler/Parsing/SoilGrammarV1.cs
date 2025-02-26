using Irony.Parsing;
using KSharp.Compiler.AST;

namespace KSharp.Compiler.Parsing;

[Language("Soil", "1.0", "Soil programming language")]
public sealed class SoilGrammarV1 : Grammar
{
    public SoilGrammarV1()
    {
        // comments
        var lineComment = new CommentTerminal("//", "\r", "\n", "\u2028", "\u2029");
        var blockComment = new CommentTerminal("/*", "*/");

        NonGrammarTerminals.Add(lineComment);
        NonGrammarTerminals.Add(blockComment);

        #region Terminals

        var newLine = new NewLineTerminal(nameof(Tokens.NewLine));

        var assignOperator = ToTerm("=", nameof(Tokens.AssignOperator));
        var colon = ToTerm(":", nameof(Tokens.Colon));
        var trueValue = ToTerm("true", nameof(Tokens.TrueToken));
        var falseValue = ToTerm("false", nameof(Tokens.FalseToken));

        var letKeyword = ToTerm("let", nameof(Tokens.LetKeyword));
        var varKeyword = ToTerm("var", nameof(Tokens.VarKeyword));

        // operators
        var plusOperator = ToTerm("+", nameof(Tokens.PlusOperator));
        var minusOperator = ToTerm("-", nameof(Tokens.MinusOperator));
        var multiplyOperator = ToTerm("*", nameof(Tokens.MultiplyOperator));
        var divideOperator = ToTerm("/", nameof(Tokens.DivideOperator));
        var moduloOperator = ToTerm("%", nameof(Tokens.ModuloOperator));

        var openingParenthesis = ToTerm("(", nameof(Tokens.ParenthesisOpen));
        var closingParenthesis = ToTerm(")", nameof(Tokens.ParenthesisClose));

        // pre-defined types
        var charType = ToTerm("Char", nameof(Tokens.TypeIdentifier));
        var int8Type = ToTerm("Int8", nameof(Tokens.TypeIdentifier));
        var int16Type = ToTerm("Int16", nameof(Tokens.TypeIdentifier));
        var int32Type = ToTerm("Int32", nameof(Tokens.TypeIdentifier));
        var int64Type = ToTerm("Int64", nameof(Tokens.TypeIdentifier));
        var floatType = ToTerm("Float32", nameof(Tokens.TypeIdentifier));
        var float64Type = ToTerm("Float64", nameof(Tokens.TypeIdentifier));
        var decimalType = ToTerm("Decimal", nameof(Tokens.TypeIdentifier));
        var boolType = ToTerm("Bool", nameof(Tokens.TypeIdentifier));
        var stringType = ToTerm("String", nameof(Tokens.TypeIdentifier));

        var preDefinedIdentifierTypes = new[]
        {
            boolType,
            charType,
            stringType,
            int8Type,
            int16Type,
            int32Type,
            int64Type,
            floatType,
            float64Type,
            decimalType,
        };

        MarkReservedWords(
            new List<string> { "val", "var", "true", "false" }
                .Concat(preDefinedIdentifierTypes.Select(t => t.Name))
                .ToArray()
        );

        var stringValue = new StringLiteral(nameof(Tokens.String), "\"");

        var integerValue = new NumberLiteral(
            nameof(Tokens.Integer),
            NumberOptions.IntOnly | NumberOptions.AllowSign
        );

        integerValue.AddSuffix("i", TypeCode.Int32);
        integerValue.AddSuffix("l", TypeCode.Int64);

        var floatingNumberValue = new NumberLiteral(
            nameof(Tokens.FloatingNumber),
            NumberOptions.AllowSign
        )
        {
            DefaultFloatType = TypeCode.Double,
            CaseSensitivePrefixesSuffixes = true
        };

        floatingNumberValue.AddSuffix("f", TypeCode.Single);
        floatingNumberValue.AddSuffix("d", TypeCode.Double);
        floatingNumberValue.AddSuffix("m", TypeCode.Decimal);

        var booleanValue = trueValue | falseValue;

        var nameIdentifier = TerminalFactory.CreateCSharpIdentifier(nameof(Tokens.NameIdentifier));

        #endregion

        #region Non-terminals

        var compilationUnit = new NonTerminal(nameof(Tokens.CompilationUnit));

        var statement = new NonTerminal(nameof(Tokens.Statement));
        var statements = MakeStarRule(new NonTerminal(nameof(Tokens.StatementList)), statement);
        var declarationStatement = new NonTerminal(nameof(Tokens.DeclarationStatement));
        var expressionStatement = new NonTerminal(nameof(Tokens.ExpressionStatement));

        var valueDeclaration = new NonTerminal(nameof(Tokens.ValueDeclarationStatement));
        var variableDeclaration = new NonTerminal(nameof(Tokens.VariableDeclarationStatement));
        var typeAnnotation = new NonTerminal(nameof(Tokens.TypeAnnotation));
        var expression = new NonTerminal(nameof(Tokens.Expression));
        var constantExpression = new NonTerminal(nameof(Tokens.ConstantExpression));
        var variableExpression = new NonTerminal(nameof(Tokens.VariableExpression));
        var binaryExpression = new NonTerminal(nameof(Tokens.BinaryExpression));
        var binaryOperator = new NonTerminal(nameof(Tokens.BinaryOperator));
        var groupedExpression = new NonTerminal(nameof(Tokens.GroupedExpression));

        #endregion

        #region Derivations

        compilationUnit.Rule = statements | Eof;

        statement.Rule = declarationStatement | expressionStatement;

        declarationStatement.Rule = valueDeclaration | variableDeclaration;

        typeAnnotation.Rule = colon + nameIdentifier;

        valueDeclaration.Rule =
            letKeyword + nameIdentifier + typeAnnotation + assignOperator + expression
            | letKeyword + nameIdentifier + assignOperator + expression;

        variableDeclaration.Rule =
            varKeyword + nameIdentifier + typeAnnotation + assignOperator + expression
            | varKeyword + nameIdentifier + assignOperator + expression;

        expressionStatement.Rule = expression;

        expression.Rule = constantExpression | binaryExpression | groupedExpression;

        constantExpression.Rule = booleanValue | floatingNumberValue | integerValue | stringValue;

        binaryExpression.Rule = expression + binaryOperator + expression;

        groupedExpression.Rule = openingParenthesis + expression + closingParenthesis;

        binaryOperator.Rule =
            plusOperator | minusOperator | multiplyOperator | divideOperator | moduloOperator;

        #endregion

        Root = compilationUnit;
    }
}
