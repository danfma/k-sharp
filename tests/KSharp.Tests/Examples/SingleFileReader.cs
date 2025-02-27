using System;
using System.IO;
using System.Reflection;

namespace KSharp.Tests.Examples;

public static class SingleFileReader
{
    public static string Read(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var resourceStream =
            assembly.GetManifestResourceStream(typeof(SingleFileReader), resourceName)
            ?? throw new InvalidOperationException($"Embedded resource not found '{resourceName}'");

        using var reader = new StreamReader(resourceStream);

        return reader.ReadToEnd();
    }
}
