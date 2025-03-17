namespace KSharp.Compiler.IR;

public readonly record struct KsAssemblyReference(string AssemblyName)
{
    public override string ToString() => AssemblyName;

    public static implicit operator string(KsAssemblyReference ar) => ar.AssemblyName;
}
