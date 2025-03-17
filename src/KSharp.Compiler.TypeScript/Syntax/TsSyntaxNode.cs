using KSharp.Compiler.TypeScript.Printing;

namespace KSharp.Compiler.TypeScript.Syntax;

/// <summary>
/// Base class for all TypeScript syntax nodes
/// </summary>
public abstract record TsSyntaxNode
{
    /// <summary>
    /// Generates TypeScript code for this node
    /// </summary>
    /// <returns>The generated TypeScript code</returns>
    public virtual string ToTypeScript()
    {
        var printer = new TypeScriptPrinter();
        
        if (this is TsSourceFile sourceFile)
            return printer.Print(sourceFile);
            
        throw new NotSupportedException($"Direct printing not supported for {GetType().Name}. Use TsSourceFile.ToTypeScript() instead.");
    }
}
