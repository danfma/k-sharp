using System.Collections.Immutable;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Assinatura de método em uma interface ou tipo literal
/// </summary>
public record TsMethodSignature(
    TsIdentifier Name,
    ImmutableArray<TsParameter> Parameters,
    TsType? ReturnType,
    bool IsOptional = false
) : TsTypeElement;
