namespace KSharp.Compiler.IR;

public record KsReturnStatement(KsExpression? Expression) : KsStatement;