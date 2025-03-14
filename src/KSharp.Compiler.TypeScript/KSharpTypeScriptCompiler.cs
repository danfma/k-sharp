using KSharp.Compiler.TypeScript.Syntax;

namespace KSharp.Compiler.TypeScript;

/// <summary>
/// Compila código K# para TypeScript
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
    /// Compila o código K# para um arquivo TypeScript
    /// </summary>
    /// <param name="code">Código fonte K#</param>
    /// <param name="fileName">Nome do arquivo</param>
    /// <returns>Arquivo TypeScript gerado</returns>
    public TsSourceFile Compile(string code, string fileName)
    {
        throw new NotImplementedException();
    }
}
