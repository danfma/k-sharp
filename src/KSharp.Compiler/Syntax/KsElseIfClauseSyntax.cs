namespace KSharp.Compiler.Syntax;

public record KsElseIfClauseSyntax(
    KsExpressionSyntax Condition,
    KsBlockStatementSyntax Block,
    KsElseClauseSyntax? Else
) : KsElseClauseSyntax;
