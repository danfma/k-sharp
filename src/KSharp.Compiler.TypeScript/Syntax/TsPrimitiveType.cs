namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Tipo primitivo (string, number, boolean, etc)
/// </summary>
public record TsPrimitiveType(string Name) : TsType
{
    public static TsPrimitiveType String => new("string");
    public static TsPrimitiveType Number => new("number");
    public static TsPrimitiveType Boolean => new("boolean");
    public static TsPrimitiveType Any => new("any");
    public static TsPrimitiveType Void => new("void");
    public static TsPrimitiveType Undefined => new("undefined");
    public static TsPrimitiveType Null => new("null");
    public static TsPrimitiveType Never => new("never");
    public static TsPrimitiveType Object => new("object");
    public static TsPrimitiveType BigInt => new("bigint");
}
