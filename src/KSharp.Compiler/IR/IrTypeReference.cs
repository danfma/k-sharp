using System.Collections.Immutable;

namespace KSharp.Compiler.IR;

/// <summary>
/// Represents a reference to a type in the IR
/// </summary>
public record IrTypeReference
{
    public string Name { get; init; }
    
    public IrTypeReference(string name)
    {
        Name = name;
    }
    
    public IrFullName? FullName { get; init; }
    public ImmutableList<IrTypeReference> GenericArguments { get; init; } = [];
}
