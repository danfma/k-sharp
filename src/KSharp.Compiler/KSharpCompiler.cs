using Irony;
using Irony.Parsing;
using KSharp.Compiler.Ast;

namespace KSharp.Compiler;

public class KSharpCompiler
{
    private readonly TextWriter _output;
    private readonly Parser _parser;
    private readonly TreeNodeTransformer _transformer;

    public KSharpCompiler(TextWriter? output = null)
    {
        var grammar = new KSharpGrammar();

        _output = output ?? Console.Out;
        _parser = new Parser(grammar);
        _transformer = new TreeNodeTransformer();
    }

    public SourceFile ParseSource(string source, string fileName = "source.ks")
    {
        var parseTree = _parser.Parse(source, fileName);

        if (parseTree.HasErrors())
        {
            PrintErrorMessages(parseTree.ParserMessages, fileName);

            throw new Exception("Erro de sintaxe no código fonte.");
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

    public string CompileToTypeScript(Ast.SourceFile sourceFile)
    {
        var tsGenerator = new TypeScriptGenerator();
        sourceFile.Accept(tsGenerator);

        return tsGenerator.GetOutput();
    }

    public string CompileToCSharp(Ast.SourceFile sourceFile)
    {
        var csGenerator = new CSharpGenerator();
        sourceFile.Accept(csGenerator);

        return csGenerator.GetOutput();
    }
}
