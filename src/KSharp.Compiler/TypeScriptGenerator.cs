using System.Text;
using KSharp.Compiler.Ast;
using KSharp.Compiler.Visitors;

namespace KSharp.Compiler;

public class TypeScriptGenerator : AstVisitor
{
    private readonly StringBuilder _output = new();

    public TypeScriptGenerator()
    {
        // Adiciona imports do runtime K#
        _output.AppendLine("import { Int, Runtime } from '@k-sharp/runtime';");
        _output.AppendLine();
    }

    public override void VisitFunctionDeclaration(FunctionDeclaration function)
    {
        // Implementação da geração de código para funções
        _output.Append("export function ");
        _output.Append(function.Identifier);
        _output.Append("(");

        // Parâmetros
        for (var i = 0; i < function.Parameters.Count; i++)
        {
            var param = function.Parameters[i];
            _output.Append(param.Identifier);
            _output.Append(": ");
            _output.Append(MapType(param.Type.Name));

            if (i < function.Parameters.Count - 1)
            {
                _output.Append(", ");
            }
        }

        _output.Append("): ");
        _output.Append(MapType(function.ReturnType.Name));
        _output.AppendLine(" {");

        // Corpo da função
        base.VisitFunctionDeclaration(function);

        _output.AppendLine("}");
    }

    // Outros métodos de visitante para gerar código TypeScript...

    private string MapType(string kSharpType)
    {
        return kSharpType switch
        {
            "Int" => "Int", // Usando nosso tipo alias do runtime
            "String" => "string",
            "Bool" => "boolean",
            _ => kSharpType
        };
    }

    public string GetOutput()
    {
        return _output.ToString();
    }
}
