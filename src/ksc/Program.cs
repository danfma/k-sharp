// See https://aka.ms/new-console-template for more information

using ConsoleAppFramework;
using KSharp.Compiler;

ConsoleApp.Run(args, (string input, Target target, string? output = null) =>
{
    var compiler = new KSharpCompiler();
    var program = compiler.ParseSource(input);

    throw new NotImplementedException();
});

public enum Target
{
    CSharp,
    TypeScript
}
