namespace KSharp.Compiler.IR;

public record KsIfStatement(KsExpression Condition, KsBlock ThenBlock, KsBlock? ElseBlock) : KsStatement;