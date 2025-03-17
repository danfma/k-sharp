using Irony.Parsing;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Responsável por ler e analisar arquivos fonte K#, construindo as árvores sintáticas correspondentes
/// </summary>
public class KsSyntaxReader
{
    private readonly KsGrammar _grammar;
    private readonly Parser _parser;
    private readonly TreeNodeTransformer _transformer;

    public KsSyntaxReader()
    {
        _grammar = new KsGrammar();
        _parser = new Parser(_grammar);
        _transformer = new TreeNodeTransformer();
    }

    /// <summary>
    /// Analisa todos os arquivos .ks em um diretório e seus subdiretórios,
    /// criando um projeto com as respectivas árvores sintáticas
    /// </summary>
    /// <param name="projectName">Nome do projeto</param>
    /// <param name="rootDirectory">Diretório raiz do projeto</param>
    /// <returns>Um objeto KsSyntaxProject com a AST de todos os arquivos</returns>
    public KsProjectSyntax ReadProject(string projectName, string rootDirectory)
    {
        var sourceFiles = Directory.GetFiles(rootDirectory, "*.ks", SearchOption.AllDirectories);

        var parseTrees = sourceFiles
            .Select(file => (Parse(file), file))
            .Where(item => item.Item1.Status == ParseTreeStatus.Parsed)
            .ToList();

        return _transformer.ToProject(parseTrees, projectName, rootDirectory);
    }

    /// <summary>
    /// Analisa um único arquivo fonte K#
    /// </summary>
    /// <param name="filePath">Caminho do arquivo</param>
    /// <returns>Árvore de análise sintática</returns>
    public ParseTree Parse(string filePath)
    {
        var sourceCode = File.ReadAllText(filePath);
        return _parser.Parse(sourceCode);
    }

    /// <summary>
    /// Analisa um único arquivo fonte e constrói sua árvore sintática K#
    /// </summary>
    /// <param name="filePath">Caminho do arquivo</param>
    /// <returns>O arquivo fonte com sua árvore sintática</returns>
    public KsSourceFileSyntax ReadSourceFile(string filePath)
    {
        var parseTree = Parse(filePath);

        if (parseTree.Status != ParseTreeStatus.Parsed)
        {
            throw new InvalidOperationException(
                $"Failed to parse file {filePath}: {parseTree.ParserMessages}"
            );
        }

        return _transformer.ToSourceFile(parseTree, filePath);
    }

    /// <summary>
    /// Analisa código fonte K# diretamente a partir de uma string
    /// </summary>
    /// <param name="sourceCode">Código fonte K#</param>
    /// <param name="fileName">Nome do arquivo virtual (opcional)</param>
    /// <returns>O arquivo fonte com sua árvore sintática</returns>
    public KsSourceFileSyntax ReadSourceFromString(
        string sourceCode,
        string fileName = "unnamed.ks"
    )
    {
        var parseTree = _parser.Parse(sourceCode);

        if (parseTree.Status != ParseTreeStatus.Parsed)
        {
            throw new InvalidOperationException(
                $"Failed to parse source code: {parseTree.ParserMessages}"
            );
        }

        return _transformer.ToSourceFile(parseTree, fileName);
    }
}
