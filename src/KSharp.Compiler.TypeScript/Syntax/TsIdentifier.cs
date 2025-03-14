namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Representa um identificador TypeScript
/// </summary>
public record TsIdentifier(string Name) : TsExpression;
