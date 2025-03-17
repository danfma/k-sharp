namespace KSharp.Compiler.IR;

public readonly record struct KsIdentifier(string Value)
{
    public override string ToString() => Value;

    public static implicit operator string(KsIdentifier id) => id.Value;
}
