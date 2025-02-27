using System.IO;

namespace KSharp.Tests.Utils;

internal sealed class TestTextWriter(ITestOutputHelper output) : StringWriter
{
    public override void WriteLine(string value) => output.WriteLine(value);
}
