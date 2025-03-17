using Irony;
using Irony.Parsing;
using KSharp.Compiler.Syntax;

namespace KSharp.Compiler;

/// <summary>
/// K# language compiler that generates the AST.
/// </summary>
public class KsCompiler
{
    private readonly Parser _parser;
    private readonly TreeNodeTransformer _transformer;
    private readonly TextWriter _output;

    public KsCompiler(TextWriter? output = null)
    {
        var grammar = new KsGrammar();

        _parser = new Parser(grammar);
        _transformer = new TreeNodeTransformer();
        _output = output ?? Console.Out;
    }

    /// <summary>
    /// Compiles K# source code into an AST
    /// </summary>
    /// <param name="code">K# source code</param>
    /// <param name="fileName">File name</param>
    /// <returns>AST representing the code</returns>
    /// <exception cref="CompilationException">Thrown when there is a compilation error</exception>
    public KsSourceFileSyntax ParseSource(string code, string fileName)
    {
        var parseTree = _parser.Parse(code);

        if (parseTree.HasErrors())
        {
            throw new CompilationException(
                $"Error parsing {fileName}",
                parseTree.ParserMessages.Select(m => m.Message).ToArray()
            );
        }

        return _transformer.ToSourceFile(parseTree, fileName);
    }

    private void PrintErrorMessages(LogMessageList messages, string fileName)
    {
        foreach (var message in messages)
        {
            _output.WriteLine(
                $"{message.Level}: {message.Message} at {fileName} {message.Location.ToUiString()}"
            );
        }
    }
}
