using System.Threading.Tasks;
using KSharp.Compiler;
using KSharp.Compiler.Syntax;
using KSharp.Tests.Examples;

namespace KSharp.Tests.Parsing;

public class KsSyntaxReaderTest
{
    [Fact]
    public Task ParseSum()
    {
        const string FileName = "Sum.ks";

        var code = SingleFileReader.Read(FileName);
        var sourceFile = new KsSyntaxReader().ReadSourceFromString(code, FileName);

        sourceFile.ShouldBeOfType<KsSourceFileSyntax>();

        return Verify(sourceFile);
    }

    [Fact]
    public Task ParseFactorial()
    {
        const string FileName = "Factorial.ks";

        var code = SingleFileReader.Read(FileName);
        var sourceFile = new KsSyntaxReader().ReadSourceFromString(code, FileName);

        sourceFile.ShouldBeOfType<KsSourceFileSyntax>();

        return Verify(sourceFile);
    }

    [Fact]
    public Task ParseFibonacci()
    {
        const string FileName = "Fibonacci.ks";

        var code = SingleFileReader.Read(FileName);
        var sourceFile = new KsSyntaxReader().ReadSourceFromString(code, FileName);

        sourceFile.ShouldBeOfType<KsSourceFileSyntax>();

        return Verify(sourceFile);
    }

    [Fact]
    public Task ParseFizzBuzz()
    {
        const string FileName = "FizzBuzz.ks";

        var code = SingleFileReader.Read(FileName);
        var sourceFile = new KsSyntaxReader().ReadSourceFromString(code, FileName);

        sourceFile.ShouldBeOfType<KsSourceFileSyntax>();

        return Verify(sourceFile);
    }
}
