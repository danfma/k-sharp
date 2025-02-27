using System.IO;
using System.Threading.Tasks;
using KSharp.Compiler;
using KSharp.Compiler.Ast;
using KSharp.Tests.Examples;

namespace KSharp.Tests.Parsing;

public class ParsingTest(ITestOutputHelper output)
{
    private readonly OutputWriter _output = new(output);
    
    [Fact]
    public Task ParseSum()
    {
        const string FileName = "Sum.ks";
        
        var code = SingleFileReader.Read(FileName);
        var sourceFile = new KSharpCompiler(_output).ParseSource(code, FileName);

        sourceFile.ShouldBeOfType<SourceFile>();
        
        return Verify(sourceFile);
    }
    
    [Fact]
    public Task ParseFibonacci()
    {
        const string FileName = "Fibonacci.ks";
        
        var code = SingleFileReader.Read(FileName);
        var sourceFile = new KSharpCompiler(_output).ParseSource(code, FileName);

        sourceFile.ShouldBeOfType<SourceFile>();
        
        return Verify(sourceFile);
    }
    
    private sealed class OutputWriter(ITestOutputHelper output) : StringWriter
    {
        public override void WriteLine(string value) => output.WriteLine(value);
    }
}
