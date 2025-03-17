namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a namespace in the IR
/// </summary>
public readonly record struct IrNamespace(string Value)
{
    public override string ToString() => Value;

    public static implicit operator string(IrNamespace ns) => ns.Value;

    public static IrNamespace operator +(IrNamespace a, IrNamespace? b)
    {
        return b is null ? a : new IrNamespace($"{a.Value}.{b.Value}");
    }
}
