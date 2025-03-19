using System.Collections.Immutable;
using KSharp.Compiler.TypeScript.Syntax;

namespace KSharp.Compiler.TypeScript;

/// <summary>
/// Applies transformations to TypeScript AST nodes to enhance, optimize, or adapt the generated code.
/// </summary>
public class TypeScriptAstTransformer
{
    /// <summary>
    /// Transforms a TypeScript project structure into its final form with 
    /// expanded imports and other necessary transformations.
    /// </summary>
    /// <param name="project">The non-transformed TypeScript project</param>
    /// <returns>The transformed TypeScript project</returns>
    public TsProject TransformProject(TsProject project)
    {
        var transformedSourceFiles = new Dictionary<string, TsSourceFile>();

        foreach (var (filePath, sourceFile) in project.SourceFiles)
        {
            // Generate necessary imports
            var imports = GenerateImports(sourceFile, filePath);
            
            // Transform statements
            var transformedStatements = TransformStatements(sourceFile.Statements);

            transformedSourceFiles[filePath] = new TsSourceFile(
                sourceFile.FileName,
                imports.ToImmutableArray(),
                transformedStatements.ToImmutableArray()
            );
        }

        return new TsProject(
            project.Name,
            transformedSourceFiles.ToImmutableDictionary()
        );
    }

    /// <summary>
    /// Generates import declarations for a source file based on its content.
    /// </summary>
    private List<TsImportDeclaration> GenerateImports(TsSourceFile sourceFile, string filePath)
    {
        var imports = new List<TsImportDeclaration>
        {
            // Import System and Ks namespaces
            new TsImportDeclaration(
                ImmutableArray.Create(
                    new TsExportSpecifier(new TsIdentifier("System"), null),
                    new TsExportSpecifier(new TsIdentifier("Ks"), null)
                ),
                "@danfma/ksharp"
            )
        };
        
        // Check if this is a top-level file
        var isTopLevelFile = filePath == "TopLevel.ts" || filePath == "ProgramKs.ts" && 
                            sourceFile.Statements.Any(
                                s => s is TsExpressionStatement expr && 
                                     expr.Expression is TsFunctionCallExpression call &&
                                     call.Expression is TsIdentifier id && 
                                     id.Name == "writeLine");

        // Add specific System.Console import for files that use writeLine
        if (sourceFile.Statements.Any(stmt => ContainsWriteLineCall(stmt)))
        {
            imports.Add(
                new TsImportDeclaration(
                    ImmutableArray<TsExportSpecifier>.Empty,
                    "System.Console",
                    new TsIdentifier("Console")
                )
            );
        }

        return imports;
    }

    /// <summary>
    /// Checks if a statement contains a writeLine call.
    /// </summary>
    private bool ContainsWriteLineCall(TsStatement statement)
    {
        if (statement is TsExpressionStatement expr && 
            expr.Expression is TsFunctionCallExpression call && 
            call.Expression is TsIdentifier id && 
            id.Name == "writeLine")
        {
            return true;
        }

        // For if statements, check their blocks
        if (statement is TsIfStatement ifStmt)
        {
            if (ifStmt.ThenStatement is TsBlock thenBlock && 
                thenBlock.Statements.Any(ContainsWriteLineCall))
            {
                return true;
            }

            if (ifStmt.ElseStatement is TsBlock elseBlock && 
                elseBlock.Statements.Any(ContainsWriteLineCall))
            {
                return true;
            }

            if (ifStmt.ElseStatement is TsIfStatement nestedIf && 
                ContainsWriteLineCall(nestedIf))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Transforms a collection of statements.
    /// </summary>
    private static List<TsStatement> TransformStatements(ImmutableArray<TsStatement> statements)
    {
        var transformedStatements = new List<TsStatement>();
        
        foreach (var statement in statements)
        {
            if (statement is TsVariableStatement varStmt && 
                varStmt.Kind == TsVariableDeclarationKind.Const && 
                varStmt.Declarations.Length == 1)
            {
                var declaration = varStmt.Declarations[0];
                
                // For numeric literals, add type annotations and conversions
                if (declaration.Initializer is TsNumberLiteral || 
                    (declaration.Initializer is TsBinaryExpression binExpr && 
                     (binExpr.Left is TsNumberLiteral || binExpr.Right is TsNumberLiteral)))
                {
                    var transformedDeclaration = new TsVariableDeclaration(
                        declaration.Name,
                        new TsTypeReference(new TsIdentifier("System.Int32")),
                        declaration.Initializer is TsNumberLiteral numLiteral
                            ? new TsFunctionCallExpression(
                                new TsPropertyAccessExpression(new TsIdentifier("Ks"), new TsIdentifier("toInt32")),
                                ImmutableArray.Create<TsExpression>(numLiteral)
                            )
                            : declaration.Initializer
                    );

                    transformedStatements.Add(new TsVariableStatement(
                        varStmt.Kind,
                        ImmutableArray.Create(transformedDeclaration)
                    ));
                    
                    continue;
                }
            }
            else if (statement is TsExpressionStatement exprStmt && 
                    exprStmt.Expression is TsFunctionCallExpression fnCall && 
                    fnCall.Expression is TsIdentifier fnId && 
                    fnId.Name == "writeLine")
            {
                // Transform writeLine calls to Console.writeLine
                transformedStatements.Add(new TsExpressionStatement(
                    new TsFunctionCallExpression(
                        new TsPropertyAccessExpression(new TsIdentifier("Console"), new TsIdentifier("writeLine")),
                        fnCall.Arguments
                    )
                ));
                
                continue;
            }
            else if (statement is TsIfStatement ifStmt)
            {
                // Handle if statements with writeLine in them
                transformedStatements.Add(TransformIfStatement(ifStmt));
                continue;
            }
            else if (statement is TsExpressionStatement exprStmt2 && exprStmt2.Expression is TsBinaryExpression binExpr)
            {
                // Handle binary expressions with numeric literals
                transformedStatements.Add(new TsExpressionStatement(TransformBinaryExpression(binExpr)));
                continue;
            }

            // Default case: keep the statement as is
            transformedStatements.Add(statement);
        }

        return transformedStatements;
    }

    /// <summary>
    /// Transforms an if statement and its nested blocks.
    /// </summary>
    private static TsStatement TransformIfStatement(TsIfStatement ifStmt)
    {
        // Transform the condition if needed
        var transformedCondition = ifStmt.Condition;

        // Transform the then statement
        TsStatement transformedThenStmt;
        if (ifStmt.ThenStatement is TsBlock thenBlock)
        {
            var transformedThenStatements = new List<TsStatement>();
            foreach (var stmt in thenBlock.Statements)
            {
                if (stmt is TsExpressionStatement exprStmt && 
                    exprStmt.Expression is TsFunctionCallExpression fnCall && 
                    fnCall.Expression is TsIdentifier fnId && 
                    fnId.Name == "writeLine")
                {
                    transformedThenStatements.Add(new TsExpressionStatement(
                        new TsFunctionCallExpression(
                            new TsPropertyAccessExpression(new TsIdentifier("Console"), new TsIdentifier("writeLine")),
                            fnCall.Arguments
                        )
                    ));
                }
                else
                {
                    transformedThenStatements.Add(stmt);
                }
            }
            transformedThenStmt = new TsBlock(transformedThenStatements.ToImmutableArray());
        }
        else
        {
            transformedThenStmt = ifStmt.ThenStatement;
        }

        // Transform the else statement if present
        TsStatement? transformedElseStmt = null;
        if (ifStmt.ElseStatement != null)
        {
            if (ifStmt.ElseStatement is TsBlock elseBlock)
            {
                var transformedElseStatements = new List<TsStatement>();
                foreach (var stmt in elseBlock.Statements)
                {
                    if (stmt is TsExpressionStatement exprStmt && 
                        exprStmt.Expression is TsFunctionCallExpression fnCall && 
                        fnCall.Expression is TsIdentifier fnId && 
                        fnId.Name == "writeLine")
                    {
                        transformedElseStatements.Add(new TsExpressionStatement(
                            new TsFunctionCallExpression(
                                new TsPropertyAccessExpression(new TsIdentifier("Console"), new TsIdentifier("writeLine")),
                                fnCall.Arguments
                            )
                        ));
                    }
                    else
                    {
                        transformedElseStatements.Add(stmt);
                    }
                }
                transformedElseStmt = new TsBlock(transformedElseStatements.ToImmutableArray());
            }
            else if (ifStmt.ElseStatement is TsIfStatement nestedIf)
            {
                transformedElseStmt = TransformIfStatement(nestedIf);
            }
            else
            {
                transformedElseStmt = ifStmt.ElseStatement;
            }
        }

        return new TsIfStatement(
            transformedCondition,
            transformedThenStmt,
            transformedElseStmt
        );
    }

    /// <summary>
    /// Transforms binary expressions, especially those with numeric literals.
    /// </summary>
    private static TsExpression TransformBinaryExpression(TsBinaryExpression binExpr)
    {
        // If we have a binary expression with numeric operands, transform it to use Ks.opXxx
        if ((binExpr.Left is TsNumberLiteral || binExpr.Right is TsNumberLiteral) &&
            binExpr.Operator.OperatorToken == "+")
        {
            return new TsFunctionCallExpression(
                new TsPropertyAccessExpression(new TsIdentifier("Ks"), new TsIdentifier("opAdd")),
                ImmutableArray.Create(binExpr.Left, binExpr.Right)
            );
        }

        return binExpr;
    }
}