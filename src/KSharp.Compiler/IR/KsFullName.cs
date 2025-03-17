namespace KSharp.Compiler.IR;

public record KsFullName
{
    public KsAssemblyReference AssemblyRef { get; }
    public KsNamespace Namespace { get; }
    public KsIdentifier Name { get; }
    public string FullName => $"{AssemblyRef}:{Namespace}:{Name}";

    public KsFullName(KsAssemblyReference assemblyRef, KsNamespace @namespace, KsIdentifier name)
    {
        AssemblyRef = assemblyRef;
        Namespace = @namespace;
        Name = name;
    }
    
    public override string ToString() => FullName;
}
