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

        var projectNode = new KsProjectSyntax
        {
            Name = new KsIdentifierSyntax("Vars"),
            RootDirectory = "/samples/Vars",
            SourceFiles = [syntaxNode],
        };

        var ksNode = new SyntaxTransformer().Transform(projectNode);

        ksNode.ShouldNotBeNull();
        ksNode.ShouldBeOfType<KsProject>();
        ksNode.Name.ShouldBe("Vars");
        ksNode.RootNamespace.Value.ShouldBe("Vars");
        ksNode.SourceFiles.Count.ShouldBe(1);
        ksNode.SourceFiles.First().FilePath.ShouldBe("/samples/Vars/Program.ks");
        ksNode.Types.ShouldBeEmpty();
        ksNode.Modules.Count.ShouldBe(1);

        var module = ksNode.Modules.First(x => x.FullName.Name == "ProgramKs");

        module.ShouldNotBeNull();
        module.FullName.Name.Value.ShouldBe("ProgramKs");
        module.FullName.Namespace.Value.ShouldBe("Vars");
        module.FullName.AssemblyRef.ShouldBe(new KsAssemblyReference("Vars"));
        module.FullName.FullName.ShouldBe("Vars:Vars:ProgramKs");
        module.Functions.Count.ShouldBe(0);
        module.Variables.Count.ShouldBe(3);

        var a = module.Variables.First(x => x.Name.Value == "a");
        var b = module.Variables.First(x => x.Name.Value == "b");
        var c = module.Variables.First(x => x.Name.Value == "c");

        a.Initializer.ShouldNotBeNull();
        a.Initializer.ShouldBeOfType<KsLiteralExpression>();
        ((KsLiteralExpression)a.Initializer).Value.ShouldBe(1);

        b.Initializer.ShouldNotBeNull();
        b.Initializer.ShouldBeOfType<KsLiteralExpression>();
        ((KsLiteralExpression)b.Initializer).Value.ShouldBe(2);

        c.Initializer.ShouldNotBeNull();
        c.Initializer.ShouldBeOfType<KsBinaryExpression>();

        var binaryExpression = (KsBinaryExpression)c.Initializer;

        binaryExpression.Left.ShouldBeOfType<KsLiteralExpression>();
        binaryExpression.Operator.ShouldBe(KsOperator.Plus);
        binaryExpression.Right.ShouldBe(new KsLiteralExpression(2));
    }
}
