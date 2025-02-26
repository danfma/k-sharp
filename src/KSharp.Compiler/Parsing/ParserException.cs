namespace KSharp.Compiler.Parsing;

public class ParserException : Exception
{
    public ParserException(string message)
        : base(message)
    {
    }
}