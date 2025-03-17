using KSharp.Compiler.IR;
using KSharp.Compiler.Syntax;
using KSharp.Compiler.TypeScript;
using KSharp.Compiler.TypeScript.Syntax;

namespace KSharp.Tests.TypeScript;

public class TypeScriptTranspilerTest
{
    [Fact]
    public void Transpile_VarsProject()
    {
        // Setup - Parse the KSharp code and convert to IR
        var programFile = "IR.Inputs.Vars.Program.ks";
        var programFileContent = SingleFileReader.Read(programFile);

        var syntaxNode = new KsSyntaxReader().ReadSourceFromString(
            programFileContent,
            "Program.ks"
        );

        var projectNode = new CompilationSyntax
        {
            Name = new IdentifierTokenSyntax("Vars"),
            RootDirectory = "/samples/Vars",
            SourceFiles = [syntaxNode],
        };

        var irNode = new SyntaxTransformer().Transform(projectNode);
        
        // Act - Transpile IR to TypeScript syntax trees
        var transpiler = new TypeScriptTranspiler();
        var tsOutput = transpiler.Transpile(irNode);
        
        // Assert
        tsOutput.ShouldNotBeNull();
        tsOutput.Count.ShouldBe(1);
        tsOutput.ShouldContainKey("ProgramKs.ts");
        
        var tsSourceFile = tsOutput["ProgramKs.ts"];
        var tsCode = tsSourceFile.ToTypeScript();
        
        tsCode.ShouldContain("const a = 1");
        tsCode.ShouldContain("const b = 2");
        tsCode.ShouldContain("const c = 1 + 2");
    }
    
    [Fact]
    public void TranspileCompilationUnit_VarsProject()
    {
        // Setup - Parse the KSharp code and convert to IR
        var programFile = "IR.Inputs.Vars.Program.ks";
        var programFileContent = SingleFileReader.Read(programFile);

        var syntaxNode = new KsSyntaxReader().ReadSourceFromString(
            programFileContent,
            "Program.ks"
        );

        var projectNode = new CompilationSyntax
        {
            Name = new IdentifierTokenSyntax("Vars"),
            RootDirectory = "/samples/Vars",
            SourceFiles = [syntaxNode],
        };

        var irNode = new SyntaxTransformer().Transform(projectNode);
        var compilationUnit = irNode.SourceFiles[0];
        
        // Act - Transpile IR to TypeScript
        var transpiler = new TypeScriptTranspiler();
        var tsSourceFile = transpiler.TranspileCompilationUnit(compilationUnit);
        var tsCode = tsSourceFile.ToTypeScript();
        
        // Assert
        tsCode.ShouldNotBeNull();
        tsCode.ShouldContain("const a = 1");
        tsCode.ShouldContain("const b = 2");
        tsCode.ShouldContain("const c = 1 + 2");
    }
}