using Irony;
using Irony.Parsing;
using KSharp.Compiler.Syntax;

namespace KSharp.Compiler;

/// <summary>
/// Compilador da linguagem K# que gera a AST.
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
    /// Compila um código fonte K# em uma AST
    /// </summary>
    /// <param name="code">Código fonte K#</param>
    /// <param name="fileName">Nome do arquivo</param>
    /// <returns>AST representando o código</returns>
    /// <exception cref="CompilationException">Lançada quando houver erro de compilação</exception>
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
