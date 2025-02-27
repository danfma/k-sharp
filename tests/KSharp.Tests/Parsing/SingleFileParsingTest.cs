using System.Threading.Tasks;
using KSharp.Compiler;
using KSharp.Compiler.Syntax;
using KSharp.Tests.Examples;
using KSharp.Tests.Utils;

namespace KSharp.Tests.Parsing;

public class ParsingTest(ITestOutputHelper output)
{
    private readonly TestTextWriter _output = new(output);
    
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
    public Task ParseFactorial()
    {
        const string FileName = "Factorial.ks";
        
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
    
    [Fact]
    public Task ParseFizzBuzz()
    {
        const string FileName = "FizzBuzz.ks";
        
        var code = SingleFileReader.Read(FileName);
        var sourceFile = new KSharpCompiler(_output).ParseSource(code, FileName);

        sourceFile.ShouldBeOfType<SourceFile>();
        
        return Verify(sourceFile);
    }
}
