using System.Linq;
using KSharp.Compiler.IR;
using KSharp.Compiler.Syntax;

namespace KSharp.Tests.IR;

public class SyntaxTransformerTest
{
    [Fact]
    public void Transform_VarsProject()
    {
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

        irNode.ShouldNotBeNull();
        irNode.ShouldBeOfType<IrCompilation>();
        irNode.Name.ShouldBe("Vars");
        irNode.RootNamespace.Value.ShouldBe("Vars");
        irNode.SourceFiles.Count.ShouldBe(1);
        irNode.SourceFiles.First().FilePath.ShouldBe("/samples/Vars/Program.ks");
        irNode.Types.ShouldBeEmpty();
        irNode.Modules.Count.ShouldBe(1);

        var module = irNode.Modules.First(x => x.FullName.Name == "ProgramKs");

        module.ShouldNotBeNull();
        module.FullName.Name.Value.ShouldBe("ProgramKs");
        module.FullName.Namespace.Value.ShouldBe("Vars");
        module.FullName.AssemblyRef.ShouldBe(new IrAssemblyReference("Vars"));
        module.FullName.FullName.ShouldBe("Vars:Vars:ProgramKs");
        module.Functions.Count.ShouldBe(0);
        module.Variables.Count.ShouldBe(3);

        var a = module.Variables.First(x => x.Name.Value == "a");
        var b = module.Variables.First(x => x.Name.Value == "b");
        var c = module.Variables.First(x => x.Name.Value == "c");

        a.Initializer.ShouldNotBeNull();
        a.Initializer.ShouldBeOfType<IrLiteralExpression>();
        ((IrLiteralExpression)a.Initializer).Value.ShouldBe(1);

        b.Initializer.ShouldNotBeNull();
        b.Initializer.ShouldBeOfType<IrLiteralExpression>();
        ((IrLiteralExpression)b.Initializer).Value.ShouldBe(2);

        c.Initializer.ShouldNotBeNull();
        c.Initializer.ShouldBeOfType<IrBinaryExpression>();

        var binaryExpression = (IrBinaryExpression)c.Initializer;

        binaryExpression.Left.ShouldBeOfType<IrLiteralExpression>();
        binaryExpression.Operator.ShouldBe(IrIntrinsicOperator.Plus);
        binaryExpression.Right.ShouldBe(new IrLiteralExpression(2));
    }
    
    [Fact]
    public void Transform_TopLevelProject()
    {
        var programFile = "IR.Inputs.TopLevel.Program.ks";
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

        irNode.ShouldNotBeNull();
        irNode.ShouldBeOfType<IrCompilation>();
        irNode.Name.ShouldBe("TopLevel");
        irNode.RootNamespace.Value.ShouldBe("TopLevel");
        irNode.SourceFiles.Count.ShouldBe(1);
        irNode.SourceFiles.First().FilePath.ShouldBe("/samples/TopLevel/Program.ks");
        irNode.Types.ShouldBeEmpty();
        irNode.Modules.Count.ShouldBe(1);

        var module = irNode.Modules.First(x => x.FullName.Name == "ProgramKs");

        module.ShouldNotBeNull();
        module.FullName.Name.Value.ShouldBe("ProgramKs");
        module.FullName.Namespace.Value.ShouldBe("TopLevel");
        module.FullName.AssemblyRef.ShouldBe(new IrAssemblyReference("TopLevel"));
        module.FullName.FullName.ShouldBe("TopLevel:TopLevel:ProgramKs");
        
        // Should have a Main function with the top-level statements
        module.Functions.Count.ShouldBe(1);
        var mainFunction = module.Functions.First();
        mainFunction.Name.Value.ShouldBe("Main");
        
        // Main function should have statements
        mainFunction.Body.Statements.Count.ShouldBeGreaterThan(0);
        mainFunction.Body.Statements[0].ShouldBeOfType<IrExpressionStatement>();
        
        // Check for if statement
        mainFunction.Body.Statements.ShouldContain(s => s is IrIfStatement);
        
        // Should have variables a and b
        module.Variables.Count.ShouldBe(2);
        var a = module.Variables.First(x => x.Name.Value == "a");
        var b = module.Variables.First(x => x.Name.Value == "b");
        
        a.Initializer.ShouldNotBeNull();
        a.Initializer.ShouldBeOfType<IrLiteralExpression>();
        ((IrLiteralExpression)a.Initializer).Value.ShouldBe(10);
        
        b.Initializer.ShouldNotBeNull();
        b.Initializer.ShouldBeOfType<IrLiteralExpression>();
        ((IrLiteralExpression)b.Initializer).Value.ShouldBe(20);
    }
}
