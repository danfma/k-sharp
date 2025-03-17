namespace KSharp.Compiler.IR;

public readonly record struct KsNamespace(string Value)
{
    public override string ToString() => Value;

    public static implicit operator string(KsNamespace ns) => ns.Value;

    public static KsNamespace operator +(KsNamespace a, KsNamespace? b)
    {
        return b is null ? a : new KsNamespace($"{a.Value}.{b.Value}");
    }
}
