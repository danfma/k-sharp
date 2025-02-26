using System.Collections;
using System.Collections.Immutable;
using System.Text;

namespace KSharp.Compiler.AST;

public sealed record TokenList<T>(ImmutableList<T> Items) : IEnumerable<T>
{
    public TokenList(IEnumerable<T> list)
        : this(list.ToImmutableList())
    {
    }

    public bool Equals(TokenList<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return Items.SequenceEqual(other.Items);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        foreach (var item in Items)
        {
            hashCode.Add(item);
        }

        return hashCode.ToHashCode();
    }

    private bool PrintMembers(StringBuilder builder)
    {
        builder.AppendJoin(", ", Items);

        return Items.Count > 0;
    }
}