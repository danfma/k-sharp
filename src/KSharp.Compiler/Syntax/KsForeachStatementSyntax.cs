namespace KSharp.Compiler.Syntax;

public record KsForeachStatementSyntax(
    KsIdentifierSyntax ItemIdentifier,
    KsExpressionSyntax Expression,
    KsBlockStatementSyntax Block
) : KsStatementSyntax;
