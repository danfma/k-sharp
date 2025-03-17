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
}
