namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a reference to an assembly in the IR
/// </summary>
public readonly record struct IrAssemblyReference(string AssemblyName)
{
    public override string ToString() => AssemblyName;

    public static implicit operator string(IrAssemblyReference ar) => ar.AssemblyName;
}
