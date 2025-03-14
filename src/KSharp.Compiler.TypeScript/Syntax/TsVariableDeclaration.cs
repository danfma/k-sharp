namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Declaração de variável
/// </summary>
public record TsVariableDeclaration(TsIdentifier Name, TsType? Type, TsExpression? Initializer)
    : TsDeclaration;
