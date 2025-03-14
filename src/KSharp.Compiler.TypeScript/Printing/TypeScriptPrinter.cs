using System.Text;
using KSharp.Compiler.TypeScript.Syntax;

namespace KSharp.Compiler.TypeScript.Printing;

/// <summary>
/// Converte árvores de sintaxe TypeScript em código
/// </summary>
public class TypeScriptPrinter
{
    private readonly StringBuilder _builder = new();
    private int _indentLevel = 0;

    /// <summary>
    /// Gera o código TypeScript para um arquivo fonte
    /// </summary>
    /// <param name="sourceFile">Arquivo fonte TypeScript</param>
    /// <returns>Código TypeScript gerado</returns>
    public string Print(TsSourceFile sourceFile)
    {
        _builder.Clear();
        _indentLevel = 0;

        // Processar imports
        foreach (var import in sourceFile.Imports)
        {
            PrintImport(import);
            _builder.AppendLine();
        }

        if (sourceFile.Imports.Length > 0)
            _builder.AppendLine();

        // Processar statements
        foreach (var statement in sourceFile.Statements)
        {
            PrintStatement(statement);
            _builder.AppendLine();
        }

        return _builder.ToString();
    }

    private void PrintImport(TsImportDeclaration import)
    {
        _builder.Append("import ");

        if (import.ImportClauses.Length == 1 && import.ImportClauses[0].Alias == null)
        {
            _builder.Append($"{{ {import.ImportClauses[0].Name} }}");
        }
        else
        {
            _builder.Append("{ ");

            for (var i = 0; i < import.ImportClauses.Length; i++)
            {
                var clause = import.ImportClauses[i];
                _builder.Append(clause.Name);

                if (clause.Alias != null)
                    _builder.Append($" as {clause.Alias}");

                if (i < import.ImportClauses.Length - 1)
                    _builder.Append(", ");
            }

            _builder.Append(" }");
        }

        _builder.Append($" from '{import.ModuleName}';");
    }

    private void PrintStatement(TsStatement statement)
    {
        switch (statement)
        {
            case TsBlock block:
                PrintBlock(block);
                break;

            case TsExpressionStatement expressionStatement:
                PrintIndent();
                PrintExpression(expressionStatement.Expression);
                _builder.AppendLine(";");
                break;

            case TsReturnStatement returnStatement:
                PrintIndent();
                _builder.Append("return");
                if (returnStatement.Expression != null)
                {
                    _builder.Append(" ");
                    PrintExpression(returnStatement.Expression);
                }
                _builder.AppendLine(";");
                break;

            case TsIfStatement ifStatement:
                PrintIfStatement(ifStatement);
                break;

            case TsForOfStatement forOfStatement:
                PrintForOfStatement(forOfStatement);
                break;

            case TsVariableStatement variableStatement:
                PrintVariableStatement(variableStatement);
                break;

            case TsFunctionDeclaration functionDeclaration:
                PrintFunctionDeclaration(functionDeclaration);
                break;

            case TsInterfaceDeclaration interfaceDeclaration:
                PrintInterfaceDeclaration(interfaceDeclaration);
                break;

            case TsClassDeclaration classDeclaration:
                PrintClassDeclaration(classDeclaration);
                break;

            case TsTypeAliasDeclaration typeAliasDeclaration:
                PrintTypeAliasDeclaration(typeAliasDeclaration);
                break;

            default:
                _builder.AppendLine($"/* Unsupported statement: {statement.GetType()} */");
                break;
        }
    }

    private void PrintBlock(TsBlock block)
    {
        PrintIndent();
        _builder.AppendLine("{");
        _indentLevel++;

        foreach (var statement in block.Statements)
        {
            PrintStatement(statement);
        }

        _indentLevel--;
        PrintIndent();
        _builder.Append("}");
    }

    private void PrintIfStatement(TsIfStatement ifStatement)
    {
        PrintIndent();
        _builder.Append("if (");
        PrintExpression(ifStatement.Condition);
        _builder.Append(") ");

        if (ifStatement.ThenStatement is TsBlock block)
        {
            PrintBlock(block);
        }
        else
        {
            _builder.AppendLine();
            _indentLevel++;
            PrintStatement(ifStatement.ThenStatement);
            _indentLevel--;
        }

        if (ifStatement.ElseStatement != null)
        {
            _builder.Append(" else ");

            if (ifStatement.ElseStatement is TsIfStatement nestedIf)
            {
                // Use the compact form for else-if
                _builder.Append("if (");
                PrintExpression(nestedIf.Condition);
                _builder.Append(") ");

                if (nestedIf.ThenStatement is TsBlock nestedBlock)
                {
                    PrintBlock(nestedBlock);
                }
                else
                {
                    _builder.AppendLine();
                    _indentLevel++;
                    PrintStatement(nestedIf.ThenStatement);
                    _indentLevel--;
                }

                if (nestedIf.ElseStatement != null)
                {
                    _builder.Append(" else ");

                    if (nestedIf.ElseStatement is TsBlock elseBlock)
                    {
                        PrintBlock(elseBlock);
                    }
                    else
                    {
                        _builder.AppendLine();
                        _indentLevel++;
                        PrintStatement(nestedIf.ElseStatement);
                        _indentLevel--;
                    }
                }
            }
            else if (ifStatement.ElseStatement is TsBlock elseBlock)
            {
                PrintBlock(elseBlock);
            }
            else
            {
                _builder.AppendLine();
                _indentLevel++;
                PrintStatement(ifStatement.ElseStatement);
                _indentLevel--;
            }
        }
    }

    private void PrintForOfStatement(TsForOfStatement forOfStatement)
    {
        PrintIndent();
        _builder.Append("for (const ");
        _builder.Append(forOfStatement.Identifier.Name);
        _builder.Append(" of ");
        PrintExpression(forOfStatement.Expression);
        _builder.Append(") ");

        if (forOfStatement.Statement is TsBlock block)
        {
            PrintBlock(block);
        }
        else
        {
            _builder.AppendLine();
            _indentLevel++;
            PrintStatement(forOfStatement.Statement);
            _indentLevel--;
        }
    }

    private void PrintVariableStatement(TsVariableStatement variableStatement)
    {
        PrintIndent();

        var keyword = variableStatement.Kind switch
        {
            TsVariableDeclarationKind.Var => "var",
            TsVariableDeclarationKind.Let => "let",
            TsVariableDeclarationKind.Const => "const",
            _ => "let", // Fallback
        };

        _builder.Append(keyword);
        _builder.Append(" ");

        for (var i = 0; i < variableStatement.Declarations.Length; i++)
        {
            var declaration = variableStatement.Declarations[i];

            _builder.Append(declaration.Name.Name);

            if (declaration.Type != null)
            {
                _builder.Append(": ");
                PrintType(declaration.Type);
            }

            if (declaration.Initializer != null)
            {
                _builder.Append(" = ");
                PrintExpression(declaration.Initializer);
            }

            if (i < variableStatement.Declarations.Length - 1)
                _builder.Append(", ");
        }

        _builder.AppendLine(";");
    }

    private void PrintFunctionDeclaration(TsFunctionDeclaration functionDeclaration)
    {
        PrintIndent();

        if (functionDeclaration.IsAsync)
            _builder.Append("async ");

        _builder.Append("function");

        if (functionDeclaration.IsGenerator)
            _builder.Append("*");

        _builder.Append(" ");
        _builder.Append(functionDeclaration.Name.Name);
        _builder.Append("(");

        for (var i = 0; i < functionDeclaration.Parameters.Length; i++)
        {
            var parameter = functionDeclaration.Parameters[i];

            _builder.Append(parameter.Name.Name);

            if (parameter.Type != null)
            {
                _builder.Append(": ");
                PrintType(parameter.Type);
            }

            if (parameter.Initializer != null)
            {
                _builder.Append(" = ");
                PrintExpression(parameter.Initializer);
            }

            if (i < functionDeclaration.Parameters.Length - 1)
                _builder.Append(", ");
        }

        _builder.Append(")");

        if (functionDeclaration.ReturnType != null)
        {
            _builder.Append(": ");
            PrintType(functionDeclaration.ReturnType);
        }

        _builder.Append(" ");
        PrintBlock(functionDeclaration.Body);
        _builder.AppendLine();
    }

    private void PrintInterfaceDeclaration(TsInterfaceDeclaration interfaceDeclaration)
    {
        PrintIndent();
        _builder.Append("interface ");
        _builder.Append(interfaceDeclaration.Name.Name);

        if (interfaceDeclaration.HeritageClauses.Length > 0)
        {
            _builder.Append(" extends ");

            for (var i = 0; i < interfaceDeclaration.HeritageClauses.Length; i++)
            {
                PrintType(interfaceDeclaration.HeritageClauses[i]);

                if (i < interfaceDeclaration.HeritageClauses.Length - 1)
                    _builder.Append(", ");
            }
        }

        _builder.AppendLine(" {");
        _indentLevel++;

        foreach (var member in interfaceDeclaration.Members)
        {
            PrintTypeElement(member);
        }

        _indentLevel--;
        PrintIndent();
        _builder.AppendLine("}");
    }

    private void PrintTypeElement(TsTypeElement element)
    {
        PrintIndent();

        switch (element)
        {
            case TsPropertySignature propertySignature:
                if (propertySignature.IsReadonly)
                    _builder.Append("readonly ");

                _builder.Append(propertySignature.Name.Name);

                if (propertySignature.IsOptional)
                    _builder.Append("?");

                _builder.Append(": ");
                PrintType(propertySignature.Type);
                _builder.AppendLine(";");
                break;

            case TsMethodSignature methodSignature:
                _builder.Append(methodSignature.Name.Name);

                if (methodSignature.IsOptional)
                    _builder.Append("?");

                _builder.Append("(");

                for (var i = 0; i < methodSignature.Parameters.Length; i++)
                {
                    var parameter = methodSignature.Parameters[i];

                    _builder.Append(parameter.Name.Name);

                    if (parameter.Type != null)
                    {
                        _builder.Append(": ");
                        PrintType(parameter.Type);
                    }

                    if (i < methodSignature.Parameters.Length - 1)
                        _builder.Append(", ");
                }

                _builder.Append(")");

                if (methodSignature.ReturnType != null)
                {
                    _builder.Append(": ");
                    PrintType(methodSignature.ReturnType);
                }

                _builder.AppendLine(";");
                break;
        }
    }

    private void PrintClassDeclaration(TsClassDeclaration classDeclaration)
    {
        PrintIndent();
        _builder.Append("class ");
        _builder.Append(classDeclaration.Name.Name);

        if (classDeclaration.HeritageClauses.Length > 0)
        {
            _builder.Append(" extends ");

            for (var i = 0; i < classDeclaration.HeritageClauses.Length; i++)
            {
                PrintType(classDeclaration.HeritageClauses[i]);

                if (i < classDeclaration.HeritageClauses.Length - 1)
                    _builder.Append(", ");
            }
        }

        _builder.AppendLine(" {");
        _indentLevel++;

        foreach (var member in classDeclaration.Members)
        {
            PrintClassElement(member);
        }

        _indentLevel--;
        PrintIndent();
        _builder.AppendLine("}");
    }

    private void PrintClassElement(TsClassElement element)
    {
        PrintIndent();

        switch (element)
        {
            case TsPropertyDeclaration propertyDeclaration:
                if (propertyDeclaration.IsReadonly)
                    _builder.Append("readonly ");

                _builder.Append(propertyDeclaration.Name.Name);

                if (propertyDeclaration.IsOptional)
                    _builder.Append("?");

                if (propertyDeclaration.Type != null)
                {
                    _builder.Append(": ");
                    PrintType(propertyDeclaration.Type);
                }

                if (propertyDeclaration.Initializer != null)
                {
                    _builder.Append(" = ");
                    PrintExpression(propertyDeclaration.Initializer);
                }

                _builder.AppendLine(";");
                break;

            case TsMethodDeclaration methodDeclaration:
                if (methodDeclaration.IsStatic)
                    _builder.Append("static ");

                _builder.Append(methodDeclaration.Name.Name);

                if (methodDeclaration.IsOptional)
                    _builder.Append("?");

                _builder.Append("(");

                for (var i = 0; i < methodDeclaration.Parameters.Length; i++)
                {
                    var parameter = methodDeclaration.Parameters[i];

                    _builder.Append(parameter.Name.Name);

                    if (parameter.Type != null)
                    {
                        _builder.Append(": ");
                        PrintType(parameter.Type);
                    }

                    if (parameter.Initializer != null)
                    {
                        _builder.Append(" = ");
                        PrintExpression(parameter.Initializer);
                    }

                    if (i < methodDeclaration.Parameters.Length - 1)
                        _builder.Append(", ");
                }

                _builder.Append(")");

                if (methodDeclaration.ReturnType != null)
                {
                    _builder.Append(": ");
                    PrintType(methodDeclaration.ReturnType);
                }

                _builder.Append(" ");
                PrintBlock(methodDeclaration.Body);
                _builder.AppendLine();
                break;
        }
    }

    private void PrintTypeAliasDeclaration(TsTypeAliasDeclaration typeAliasDeclaration)
    {
        PrintIndent();
        _builder.Append("type ");
        _builder.Append(typeAliasDeclaration.Name.Name);
        _builder.Append(" = ");
        PrintType(typeAliasDeclaration.Type);
        _builder.AppendLine(";");
    }

    private void PrintExpression(TsExpression expression)
    {
        switch (expression)
        {
            case TsIdentifier identifier:
                _builder.Append(identifier.Name);
                break;

            case TsStringLiteral stringLiteral:
                _builder.Append($"\"{EscapeString(stringLiteral.Value)}\"");
                break;

            case TsNumberLiteral numberLiteral:
                _builder.Append(numberLiteral.Value);
                break;

            case TsBooleanLiteral booleanLiteral:
                _builder.Append(booleanLiteral.Value ? "true" : "false");
                break;

            case TsNullLiteral:
                _builder.Append("null");
                break;

            case TsUndefinedLiteral:
                _builder.Append("undefined");
                break;

            case TsBinaryExpression binaryExpression:
                PrintExpression(binaryExpression.Left);
                _builder.Append($" {binaryExpression.Operator.OperatorToken} ");
                PrintExpression(binaryExpression.Right);
                break;

            case TsFunctionCallExpression functionCallExpression:
                PrintExpression(functionCallExpression.Expression);
                _builder.Append("(");

                for (var i = 0; i < functionCallExpression.Arguments.Length; i++)
                {
                    PrintExpression(functionCallExpression.Arguments[i]);

                    if (i < functionCallExpression.Arguments.Length - 1)
                        _builder.Append(", ");
                }

                _builder.Append(")");
                break;

            case TsPropertyAccessExpression propertyAccessExpression:
                PrintExpression(propertyAccessExpression.Expression);
                _builder.Append(".");
                _builder.Append(propertyAccessExpression.Name.Name);
                break;

            case TsElementAccessExpression elementAccessExpression:
                PrintExpression(elementAccessExpression.Expression);
                _builder.Append("[");
                PrintExpression(elementAccessExpression.ArgumentExpression);
                _builder.Append("]");
                break;

            case TsArrowFunction arrowFunction:
                _builder.Append("(");

                for (var i = 0; i < arrowFunction.Parameters.Length; i++)
                {
                    var parameter = arrowFunction.Parameters[i];

                    _builder.Append(parameter.Name.Name);

                    if (parameter.Type != null)
                    {
                        _builder.Append(": ");
                        PrintType(parameter.Type);
                    }

                    if (i < arrowFunction.Parameters.Length - 1)
                        _builder.Append(", ");
                }

                _builder.Append(")");

                if (arrowFunction.ReturnType != null)
                {
                    _builder.Append(": ");
                    PrintType(arrowFunction.ReturnType);
                }

                _builder.Append(" => ");

                PrintExpression(arrowFunction.Body);
                break;
        }
    }

    private void PrintType(TsType type)
    {
        switch (type)
        {
            case TsTypeReference typeReference:
                _builder.Append(typeReference.TypeName.Name);
                break;

            case TsPrimitiveType primitiveType:
                _builder.Append(primitiveType.Name);
                break;

            case TsUnionType unionType:
                for (var i = 0; i < unionType.Types.Length; i++)
                {
                    PrintType(unionType.Types[i]);

                    if (i < unionType.Types.Length - 1)
                        _builder.Append(" | ");
                }
                break;

            case TsArrayType arrayType:
                PrintType(arrayType.ElementType);
                _builder.Append("[]");
                break;

            case TsTypeLiteral typeLiteral:
                _builder.AppendLine("{");
                _indentLevel++;

                foreach (var member in typeLiteral.Members)
                {
                    PrintTypeElement(member);
                }

                _indentLevel--;
                PrintIndent();
                _builder.Append("}");
                break;

            case TsFunctionType functionType:
                _builder.Append("(");

                for (var i = 0; i < functionType.Parameters.Length; i++)
                {
                    var parameter = functionType.Parameters[i];

                    _builder.Append(parameter.Name.Name);

                    if (parameter.Type != null)
                    {
                        _builder.Append(": ");
                        PrintType(parameter.Type);
                    }

                    if (i < functionType.Parameters.Length - 1)
                        _builder.Append(", ");
                }

                _builder.Append(") => ");
                PrintType(functionType.ReturnType);
                break;

            case TsTupleType tupleType:
                _builder.Append("[");

                for (var i = 0; i < tupleType.ElementTypes.Length; i++)
                {
                    PrintType(tupleType.ElementTypes[i]);

                    if (i < tupleType.ElementTypes.Length - 1)
                        _builder.Append(", ");
                }

                _builder.Append("]");
                break;
        }
    }

    private void PrintIndent()
    {
        for (var i = 0; i < _indentLevel; i++)
        {
            _builder.Append("    ");
        }
    }

    private string EscapeString(string text)
    {
        return text.Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }
}
