namespace KSharp.Compiler.Syntax;

public record KsIfStatementSyntax(
    KsExpressionSyntax Condition,
    KsBlockStatementSyntax Block,
    KsElseClauseSyntax? Else
) : KsStatementSyntax;
