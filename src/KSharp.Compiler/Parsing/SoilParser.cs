using Irony.Parsing;
using KSharp.Compiler.AST;

namespace KSharp.Compiler.Parsing;

public sealed class SoilParser
{
    public readonly Grammar Grammar = new SoilGrammarV1();

    public ParseTreeNode Parse(string text)
    {
        var parser = new Parser(Grammar);
        var result = parser.Parse(text);

        if (result.HasErrors())
        {
            throw new ParserException(string.Join(Environment.NewLine, result.ParserMessages));
        }

        return result.Root;
    }

    public CompilationUnit ParseCompilationUnit(string text)
    {
        var root = Parse(text);

        return (CompilationUnit)root.ToSyntaxNode();
    }
}
