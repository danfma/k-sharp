using KSharp.Compiler.TypeScript.Syntax;

namespace KSharp.Compiler.TypeScript;

/// <summary>
/// Compiles K# code to TypeScript
/// </summary>
public class KSharpTypeScriptCompiler
{
    private readonly KsCompiler _compiler;
    private readonly TypeScriptTranspiler _transpiler;

    public KSharpTypeScriptCompiler()
    {
        _compiler = new KsCompiler();
        _transpiler = new TypeScriptTranspiler();
    }

    /// <summary>
    /// Compiles K# code to a TypeScript file
    /// </summary>
    /// <param name="code">K# source code</param>
    /// <param name="fileName">File name</param>
    /// <returns>Generated TypeScript file</returns>
    public TsSourceFile Compile(string code, string fileName)
    {
        throw new NotImplementedException();
    }
}
