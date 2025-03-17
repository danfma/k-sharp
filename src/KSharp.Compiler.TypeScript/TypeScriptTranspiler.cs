using System.Collections.Immutable;
using KSharp.Compiler.IR;
using KSharp.Compiler.TypeScript.Syntax;

namespace KSharp.Compiler.TypeScript;

/// <summary>
/// Transforms IR nodes into TypeScript syntax nodes
/// </summary>
public class TypeScriptTranspiler
{
    /// <summary>
    /// Transpiles a KSharp compilation to TypeScript source files
    /// </summary>
    /// <param name="compilation">The IR compilation</param>
    /// <returns>A dictionary mapping file paths to TypeScript source files</returns>
    public Dictionary<string, TsSourceFile> Transpile(IrCompilation compilation)
    {
        var result = new Dictionary<string, TsSourceFile>();

        // Process each module into a separate TypeScript file
        foreach (var module in compilation.Modules)
        {
            var tsSourceFile = TranspileModule(module);
            var filePath = GetTypeScriptFilePath(module);
            
            result[filePath] = tsSourceFile;
        }

        return result;
    }

    /// <summary>
    /// Transpiles a single IR compilation unit to a TypeScript source file
    /// </summary>
    /// <param name="compilationUnit">The IR compilation unit</param>
    /// <returns>The generated TypeScript source file</returns>
    public TsSourceFile TranspileCompilationUnit(IrCompilationUnit compilationUnit)
    {
        // Find all modules in the compilation unit
        var modules = compilationUnit.Declarations.OfType<IrModule>().ToList();
        
        if (modules.Count == 0)
            return new TsSourceFile(
                compilationUnit.FilePath,
                ImmutableArray<TsImportDeclaration>.Empty,
                ImmutableArray<TsStatement>.Empty
            );
        
        // For simplicity, combine all modules into a single TypeScript file
        var statements = TranspileModules(modules).ToImmutableArray();
        
        return new TsSourceFile(
            compilationUnit.FilePath,  // FileName
            ImmutableArray<TsImportDeclaration>.Empty,  // Imports
            statements  // Statements
        );
    }
    
    private static string GetTypeScriptFilePath(IrModule module)
    {
        // Format: <moduleName>.ts
        return $"{module.FullName.Name.Value}.ts";
    }
    
    private TsSourceFile TranspileModule(IrModule module)
    {
        var statements = new List<TsStatement>();
        
        // Add variables
        foreach (var variable in module.Variables)
        {
            statements.Add(TranspileVariable(variable));
        }
        
        // Add functions
        foreach (var function in module.Functions)
        {
            statements.Add(TranspileFunction(function));
        }
        
        return new TsSourceFile(
            module.FullName.Name.Value + ".ts",  // FileName
            ImmutableArray<TsImportDeclaration>.Empty,  // Imports
            statements.ToImmutableArray()  // Statements
        );
    }
    
    private IEnumerable<TsStatement> TranspileModules(IEnumerable<IrModule> modules)
    {
        foreach (var module in modules)
        {
            // Add variables
            foreach (var variable in module.Variables)
            {
                yield return TranspileVariable(variable);
            }
            
            // Add functions
            foreach (var function in module.Functions)
            {
                yield return TranspileFunction(function);
            }
        }
    }
    
    private TsVariableStatement TranspileVariable(IrVariable variable)
    {
        var declarationKind = variable.IsMutable 
            ? TsVariableDeclarationKind.Let 
            : TsVariableDeclarationKind.Const;
        
        var declaration = new TsVariableDeclaration(
            new TsIdentifier(variable.Name.Value),  // Name
            variable.Type != null ? TranspileTypeReference(variable.Type) : null,  // Type
            variable.Initializer != null ? TranspileExpression(variable.Initializer) : null  // Initializer
        );
        
        return new TsVariableStatement(
            declarationKind,  // Kind
            ImmutableArray.Create(declaration)  // Declarations
        );
    }
    
    private TsFunctionDeclaration TranspileFunction(IrFunction function)
    {
        var parameters = function.Parameters
            .Select(p => new TsParameter(
                new TsIdentifier(p.Name.Value),  // Name
                p.Type != null ? TranspileTypeReference(p.Type) : null,  // Type
                null  // Initializer
            ))
            .ToImmutableArray();
        
        var body = TranspileBlock(function.Body);
        
        return new TsFunctionDeclaration(
            new TsIdentifier(function.Name.Value),  // Name
            parameters,  // Parameters
            function.ReturnType != null ? TranspileTypeReference(function.ReturnType) : null,  // ReturnType
            body,  // Body
            false,  // IsAsync
            false   // IsGenerator
        );
    }
    
    private TsBlock TranspileBlock(IrBlock block)
    {
        var statements = block.Statements
            .Select(TranspileStatement)
            .ToImmutableArray();
        
        return new TsBlock(statements);
    }
    
    private TsStatement TranspileStatement(IrStatement statement)
    {        
        // Handle different statement types
        var statementType = statement.GetType();
        var typeName = statementType.Name;
        
        if (typeName == "IrBlock")
        {
            // Manually handle property access
            var statementsProperty = statementType.GetProperty("Statements");
            var statements = statementsProperty?.GetValue(statement) as ImmutableList<IrStatement>;
            
            if (statements != null)
            {
                var tsStatements = statements
                    .Select(TranspileStatement)
                    .ToImmutableArray();
                
                return new TsBlock(tsStatements);
            }
        }
        
        if (statement is IrExpressionStatement exprStmt)
            return new TsExpressionStatement(TranspileExpression(exprStmt.Expression));
            
        if (statement is IrReturnStatement returnStmt)
            return new TsReturnStatement(
                returnStmt.Expression != null 
                    ? TranspileExpression(returnStmt.Expression) 
                    : null
            );
            
        if (statement is IrIfStatement ifStmt)
            return TranspileIfStatement(ifStmt);
            
        if (statement is IrForEachStatement forEachStmt)
            return TranspileForEachStatement(forEachStmt);
            
        if (statement is IrVariableDeclaration varDecl)
            return TranspileVariableDeclaration(varDecl);
            
        return new TsEmptyStatement();  // Fallback for unsupported statements
    }
    
    private TsIfStatement TranspileIfStatement(IrIfStatement ifStmt)
    {
        var condition = TranspileExpression(ifStmt.Condition);
        var thenStmt = TranspileBlock(ifStmt.ThenBlock);
        TsStatement? elseStmt = null;
        
        if (ifStmt.ElseBlock != null)
        {
            // Check if the else block contains only an if statement
            if (ifStmt.ElseBlock.Statements.Count == 1 && 
                ifStmt.ElseBlock.Statements[0] is IrIfStatement nestedIf)
            {
                elseStmt = TranspileIfStatement(nestedIf);
            }
            else
            {
                elseStmt = TranspileBlock(ifStmt.ElseBlock);
            }
        }
        
        return new TsIfStatement(
            condition,  // Condition
            thenStmt,   // ThenStatement
            elseStmt    // ElseStatement
        );
    }
    
    private TsForOfStatement TranspileForEachStatement(IrForEachStatement forEachStmt)
    {
        return new TsForOfStatement(
            new TsIdentifier(forEachStmt.Identifier.Value),  // Identifier
            TranspileExpression(forEachStmt.Collection),    // Expression
            TranspileBlock(forEachStmt.Body)                // Statement
        );
    }
    
    private TsVariableStatement TranspileVariableDeclaration(IrVariableDeclaration varDecl)
    {
        var declarationKind = varDecl.IsMutable 
            ? TsVariableDeclarationKind.Let 
            : TsVariableDeclarationKind.Const;
        
        var declaration = new TsVariableDeclaration(
            new TsIdentifier(varDecl.Name.Value),  // Name
            varDecl.Type != null ? TranspileTypeReference(varDecl.Type) : null,  // Type
            varDecl.Initializer != null ? TranspileExpression(varDecl.Initializer) : null  // Initializer
        );
        
        return new TsVariableStatement(
            declarationKind,  // Kind
            ImmutableArray.Create(declaration)  // Declarations
        );
    }
    
    private TsExpression TranspileExpression(IrExpression expression)
    {
        if (expression is IrLiteralExpression literal)
            return TranspileLiteral(literal);
            
        if (expression is IrIdentifierName identifier)
            return new TsIdentifier(identifier.Identifier.Value);
            
        if (expression is IrBinaryExpression binary)
            return TranspileBinaryExpression(binary);
            
        if (expression is IrInvocation invocation)
            return TranspileInvocation(invocation);
            
        throw new NotImplementedException($"Expression type {expression.GetType().Name} not supported");
    }
    
    private static TsExpression TranspileLiteral(IrLiteralExpression literal)
    {
        if (literal.Value is int intValue)
            return new TsNumberLiteral(Convert.ToDouble(intValue));
            
        if (literal.Value is double doubleValue)
            return new TsNumberLiteral(doubleValue);
            
        if (literal.Value is bool boolValue)
            return new TsBooleanLiteral(boolValue);
            
        if (literal.Value is string strValue)
            return new TsStringLiteral(strValue);
            
        if (literal.Value is null)
            return new TsNullLiteral();
            
        throw new NotImplementedException($"Literal type {literal.Value?.GetType().Name} not supported");
    }
    
    private TsBinaryExpression TranspileBinaryExpression(IrBinaryExpression binary)
    {
        var left = TranspileExpression(binary.Left);
        var right = TranspileExpression(binary.Right);
        
        var op = binary.Operator switch
        {
            IrIntrinsicOperator intrinsicOp => MapIntrinsicOperator(intrinsicOp),
            IrConcreteOperator concreteOp => new TsBinaryOperator(concreteOp.Symbol),
            _ => throw new NotImplementedException($"Operator type {binary.Operator.GetType().Name} not supported")
        };
        
        return new TsBinaryExpression(
            left,  // Left
            op,    // Operator
            right  // Right
        );
    }
    
    private static TsBinaryOperator MapIntrinsicOperator(IrIntrinsicOperator op)
    {
        if (op == IrIntrinsicOperator.Plus) return new TsBinaryOperator("+");
        if (op == IrIntrinsicOperator.Minus) return new TsBinaryOperator("-");
        if (op == IrIntrinsicOperator.Multiply) return new TsBinaryOperator("*");
        if (op == IrIntrinsicOperator.Divide) return new TsBinaryOperator("/");
        if (op == IrIntrinsicOperator.Modulo) return new TsBinaryOperator("%");
        if (op == IrIntrinsicOperator.Equal) return new TsBinaryOperator("===");
        if (op == IrIntrinsicOperator.NotEqual) return new TsBinaryOperator("!==");
        if (op == IrIntrinsicOperator.LessThan) return new TsBinaryOperator("<");
        if (op == IrIntrinsicOperator.LessThanOrEqual) return new TsBinaryOperator("<=");
        if (op == IrIntrinsicOperator.GreaterThan) return new TsBinaryOperator(">");
        if (op == IrIntrinsicOperator.GreaterThanOrEqual) return new TsBinaryOperator(">=");
        if (op == IrIntrinsicOperator.And) return new TsBinaryOperator("&&");
        if (op == IrIntrinsicOperator.Or) return new TsBinaryOperator("||");
        if (op == IrIntrinsicOperator.BitwiseAnd) return new TsBinaryOperator("&");
        if (op == IrIntrinsicOperator.BitwiseOr) return new TsBinaryOperator("|");
        if (op == IrIntrinsicOperator.BitwiseXor) return new TsBinaryOperator("^");
        
        throw new NotImplementedException($"Intrinsic operator {op} not supported");
    }
    
    private TsFunctionCallExpression TranspileInvocation(IrInvocation invocation)
    {
        var arguments = invocation.Arguments
            .Select(TranspileExpression)
            .ToImmutableArray();
        
        return new TsFunctionCallExpression(
            new TsIdentifier(invocation.MethodName.Value),  // Expression
            arguments  // Arguments
        );
    }
    
    private static TsType TranspileTypeReference(IrTypeReference typeRef)
    {
        // Basic implementation - will need to be expanded as type system grows
        return new TsTypeReference(
            new TsIdentifier(typeRef.Name)  // TypeName
        );
    }
}
