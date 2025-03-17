using Irony.Parsing;

namespace KSharp.Compiler.Syntax;

/// <summary>
/// Responsible for reading and parsing K# source files, building the corresponding syntax trees
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
    /// Parses all .ks files in a directory and its subdirectories,
    /// creating a project with the respective syntax trees
    /// </summary>
    /// <param name="projectName">Project name</param>
    /// <param name="rootDirectory">Project root directory</param>
    /// <returns>A CompilationSyntax object with the AST of all files</returns>
    public CompilationSyntax ReadProject(string projectName, string rootDirectory)
    {
        var sourceFiles = Directory.GetFiles(rootDirectory, "*.ks", SearchOption.AllDirectories);

        var parseTrees = sourceFiles
            .Select(file => (Parse(file), file))
            .Where(item => item.Item1.Status == ParseTreeStatus.Parsed)
            .ToList();

        return _transformer.ToProject(parseTrees, projectName, rootDirectory);
    }

    /// <summary>
    /// Parses a single K# source file
    /// </summary>
    /// <param name="filePath">File path</param>
    /// <returns>Syntax parse tree</returns>
    public ParseTree Parse(string filePath)
    {
        var sourceCode = File.ReadAllText(filePath);
        return _parser.Parse(sourceCode);
    }

    /// <summary>
    /// Parses a single source file and builds its K# syntax tree
    /// </summary>
    /// <param name="filePath">File path</param>
    /// <returns>The source file with its syntax tree</returns>
    public CompilationUnitSyntax ReadSourceFile(string filePath)
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
    /// Parses K# source code directly from a string
    /// </summary>
    /// <param name="sourceCode">K# source code</param>
    /// <param name="fileName">Virtual file name (optional)</param>
    /// <returns>The source file with its syntax tree</returns>
    public CompilationUnitSyntax ReadSourceFromString(
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
