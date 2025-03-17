namespace KSharp.Compiler.IR;

/// <summary>
/// Represents an identifier in the IR
/// </summary>
public readonly record struct IrIdentifier(string Value)
{
    public override string ToString() => Value;

    public static implicit operator string(IrIdentifier id) => id.Value;
}
