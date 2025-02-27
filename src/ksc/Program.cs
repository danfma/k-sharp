// See https://aka.ms/new-console-template for more information

using ConsoleAppFramework;
using KSharp.Compiler;

ConsoleApp.Run(args, (string input, Target target, string? output = null) =>
{
    var compiler = new KSharpCompiler();
    var program = compiler.ParseSource(input);
    
    var outputCode = target switch
    {
        Target.CSharp => compiler.CompileToCSharp(program),
        Target.TypeScript => compiler.CompileToTypeScript(program),
        _ => throw new ArgumentException("Invalid target.")
    };
    
    output ??= Path.Combine(Environment.CurrentDirectory, "output" + (target == Target.CSharp ? ".cs" : ".ts"));
    
    File.WriteAllText(output, outputCode);
});

public enum Target
{
    CSharp,
    TypeScript
}
