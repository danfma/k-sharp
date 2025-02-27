namespace KSharp.Tests.Transpiler;

public class TypeScriptTranspilerTest(ITestOutputHelper output)
{
    [Fact]
    public void SimplePrint()
    {
        var code =
            """
            writeLine("Hello from K#");
            """;
    }
}
