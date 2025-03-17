namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a fully qualified name of a declaration in the IR
/// </summary>
public record IrFullName
{
    public IrAssemblyReference AssemblyRef { get; }
    public IrNamespace Namespace { get; }
    public IrIdentifier Name { get; }
    public string FullName => $"{AssemblyRef}:{Namespace}:{Name}";

    public IrFullName(IrAssemblyReference assemblyRef, IrNamespace @namespace, IrIdentifier name)
    {
        AssemblyRef = assemblyRef;
        Namespace = @namespace;
        Name = name;
    }

    public override string ToString() => FullName;
}
