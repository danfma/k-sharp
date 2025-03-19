using KSharp.Compiler.IR;
using KSharp.Compiler.Syntax;
using KSharp.Compiler.TypeScript;

namespace KSharp.Tests.TypeScript;

public class TypeScriptTranspilerTest
{
    [Fact]
    public void Transpile_VarsProject()
    {
        // Setup - Parse the KSharp code and convert to IR
        var programFile = "Examples.Vars.Program.ks";
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
        tsOutput.SourceFiles.Count.ShouldBe(1);
        tsOutput.SourceFiles.ShouldContainKey("ProgramKs.ts");

        var tsSourceFile = tsOutput.SourceFiles["ProgramKs.ts"];
        var tsCode = tsSourceFile.ToTypeScript();

        // We're now generating more complete TypeScript with types, so let's update our expectations
        tsCode.ShouldContain("const a: System.Int32");
        tsCode.ShouldContain("const b: System.Int32");
        tsCode.ShouldContain("Ks.toInt32(1)");
        tsCode.ShouldContain("Ks.toInt32(2)");
    }

    [Fact]
    public void TranspileCompilationUnit_VarsProject()
    {
        // Setup - Parse the KSharp code and convert to IR
        var programFile = "Examples.Vars.Program.ks";
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
        // We're now generating more complete TypeScript with types, so let's update our expectations
        tsCode.ShouldContain("const a: System.Int32");
        tsCode.ShouldContain("const b: System.Int32");
        tsCode.ShouldContain("Ks.toInt32(1)");
        tsCode.ShouldContain("Ks.toInt32(2)");
    }

    [Fact]
    public void Transpile_TopLevelProject()
    {
        // Setup - Parse the KSharp code and convert to IR
        var programFile = "Examples.TopLevel.TopLevel.ks";
        var programFileContent = SingleFileReader.Read(programFile);

        var syntaxNode = new KsSyntaxReader().ReadSourceFromString(
            programFileContent,
            "Program.ks"
        );

        var projectNode = new CompilationSyntax
        {
            Name = new IdentifierTokenSyntax("TopLevel"),
            RootDirectory = "/samples/TopLevel",
            SourceFiles = [syntaxNode],
        };

        var irNode = new SyntaxTransformer().Transform(projectNode);

        // Act - Transpile IR to TypeScript syntax trees
        var transpiler = new TypeScriptTranspiler();
        var tsProject = transpiler.Transpile(irNode);

        // Assert
        tsProject.ShouldNotBeNull();
        tsProject.SourceFiles.Count.ShouldBe(1);
        tsProject.SourceFiles.ShouldContainKey("ProgramKs.ts");

        var tsSourceFile = tsProject.SourceFiles["ProgramKs.ts"];
        var tsCode = tsSourceFile.ToTypeScript();
        var expected = SingleFileReader.Read(programFile.Replace(".ks", ".ts"));

        tsCode.ShouldBe(expected);
    }
}
