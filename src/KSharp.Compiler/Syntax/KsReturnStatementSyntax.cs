namespace KSharp.Compiler.Syntax;

public record KsReturnStatementSyntax(KsExpressionSyntax? Expression) : KsStatementSyntax;
