using System.Text;
using KSharp.Compiler.Visitors;

namespace KSharp.Compiler;

public class CSharpGenerator : AstVisitor
{
    private readonly StringBuilder _output = new();

    public CSharpGenerator()
    {
        // Adiciona using statements e namespace
        _output.AppendLine("using System;");
        _output.AppendLine();
        _output.AppendLine("namespace KSharp.Generated");
        _output.AppendLine("{");
    }

    // Implementação similar ao TypeScriptGenerator, mas para C#

    public string GetOutput()
    {
        _output.AppendLine("}"); // Fecha o namespace
        return _output.ToString();
    }
}
