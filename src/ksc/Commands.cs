using System.Collections.Immutable;
using System.Diagnostics;
using KSharp.Compiler.IR;
using KSharp.Compiler.Syntax;
using KSharp.Compiler.TypeScript;
using KSharp.Compiler.Utils;

namespace KSharp.Compiler;

public class Commands
{
    /// <summary>Compiles K# source files to the target language.</summary>
    /// <param name="input">-i, Input directory containing K# source files</param>
    /// <param name="target">-t, Target language (TypeScript or CSharp)</param>
    /// <param name="output">-o, Output directory</param>
    /// <param name="generatePackageJson">-gen-pkg-json, Generate package.json for Node.js projects</param>
    /// <param name="generateTsConfig">-gen-ts-config, Generate tsconfig.json for TypeScript projects</param>
    public async Task Build(
        string input,
        Target target = Target.TypeScript,
        string output = "output",
        bool generatePackageJson = false,
        bool generateTsConfig = false
    )
    {
        try
        {
            Console.WriteLine($"Compiling from: {input}");
            Console.WriteLine($"Target: {target}");
            Console.WriteLine($"Output directory: {output}");
            Console.WriteLine($"Generate package.json: {generatePackageJson}");
            Console.WriteLine($"Generate tsconfig.json: {generateTsConfig}");

            switch (target)
            {
                case Target.TypeScript:
                    await CompileToTypeScript(input, output, generatePackageJson, generateTsConfig);
                    break;

                case Target.CSharp:
                    throw new NotImplementedException("C# target is not yet implemented.");

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(target),
                        "Unsupported target platform."
                    );
            }

            Console.WriteLine("Compilation successful.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Compilation failed: {ex.Message}");

            if (Debugger.IsAttached)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }

            throw; // ConsoleAppFramework will handle the exit code
        }
    }

    // Helper methods
    private static async Task CompileToTypeScript(
        string inputPath,
        string outputPath,
        bool generatePackageJson,
        bool generateTsConfig
    )
    {
        var timeTracker = TimeTracker.Start();

        // Create output directory if it doesn't exist
        Directory.CreateDirectory(outputPath);

        // Find all .ks source files
        var sourceFiles = Directory.GetFiles(inputPath, "*.ks", SearchOption.AllDirectories);

        if (sourceFiles.Length == 0)
        {
            throw new FileNotFoundException($"No K# source files (*.ks) found in {inputPath}");
        }

        Console.WriteLine(
            $"Found {sourceFiles.Length} K# source files in {timeTracker.GetElapsedTime(reset: true).TotalMilliseconds} ms"
        );

        // Read and parse each source file
        var syntaxReader = new KsSyntaxReader();
        var compilationUnits = new List<CompilationUnitSyntax>();

        foreach (var sourceFile in sourceFiles)
        {
            var relativePath = Path.GetRelativePath(inputPath, sourceFile);

            Console.WriteLine($"Parsing {relativePath}");

            var sourceContent = await File.ReadAllTextAsync(sourceFile);
            var syntaxNode = syntaxReader.ReadSourceFromString(sourceContent, relativePath);

            compilationUnits.Add(syntaxNode);
        }

        // Create project (compilation) syntax
        var projectName = Path.GetFileName(
            inputPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
        );

        var compilation = new CompilationSyntax
        {
            Name = new IdentifierTokenSyntax(projectName),
            RootDirectory = inputPath,
            SourceFiles = compilationUnits.ToImmutableList(),
        };

        // Print elapsed time for parsing
        Console.WriteLine(
            $"Parsed all source files in {timeTracker.GetElapsedTime(reset: true).TotalMilliseconds} ms"
        );

        // Transform to IR
        Console.WriteLine("Generating intermediate representation (IR)");

        var transformer = new SyntaxTransformer();
        var irCompilation = transformer.Transform(compilation);

        Console.WriteLine(
            $"Generated intermediate representation in {timeTracker.GetElapsedTime(reset: true).TotalMilliseconds} ms"
        );

        // Transpile to TypeScript
        Console.WriteLine("Transpiling to TypeScript");

        var tsTranspiler = new TypeScriptTranspiler();
        var tsFiles = tsTranspiler.Transpile(irCompilation);

        Console.WriteLine(
            $"Generating TypeScript AST in {timeTracker.GetElapsedTime(reset: true).TotalMilliseconds} ms"
        );

        // Write output files
        Console.WriteLine("Writing TypeScript files");

        foreach (var (filePath, tsSourceFile) in tsFiles.SourceFiles.AsParallel())
        {
            var outputFilePath = Path.Combine(outputPath, filePath);
            var tsCode = tsSourceFile.ToTypeScript();

            // Create directory if needed
            var directory = Path.GetDirectoryName(outputFilePath);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            await File.WriteAllTextAsync(outputFilePath, tsCode);

            Console.WriteLine($"Written: {outputFilePath}");
        }

        Console.WriteLine(
            $"TypeScript files written in {timeTracker.GetElapsedTime(reset: true).TotalMilliseconds} ms"
        );

        // Create package.json only if requested
        if (generatePackageJson)
        {
            var packageJsonPath = Path.Combine(outputPath, "package.json");
            if (File.Exists(packageJsonPath))
            {
                Console.WriteLine("Warning: package.json already exists, skipping generation");
            }
            else
            {
                Console.WriteLine("Creating package.json");
                var packageJson =
                    $@"{{
  ""name"": ""{projectName.ToLowerInvariant()}"",
  ""version"": ""1.0.0"",
  ""description"": ""Generated from K# project"",
  ""main"": ""index.js"",
  ""scripts"": {{
    ""test"": ""echo \""Error: no test specified\"" && exit 1""
  }},
  ""keywords"": [""ksharp"", ""generated""],
  ""author"": """",
  ""license"": ""ISC""
}}";
                await File.WriteAllTextAsync(packageJsonPath, packageJson);
            }
        }

        // Create tsconfig.json only if requested
        if (generateTsConfig)
        {
            var tsconfigPath = Path.Combine(outputPath, "tsconfig.json");
            if (File.Exists(tsconfigPath))
            {
                Console.WriteLine("Warning: tsconfig.json already exists, skipping generation");
            }
            else
            {
                Console.WriteLine("Creating tsconfig.json");
                var tsconfig =
                    @"{
  ""compilerOptions"": {
    ""target"": ""es2020"",
    ""module"": ""commonjs"",
    ""strict"": true,
    ""esModuleInterop"": true,
    ""skipLibCheck"": true,
    ""forceConsistentCasingInFileNames"": true,
    ""outDir"": ""dist""
  }
}";
                await File.WriteAllTextAsync(tsconfigPath, tsconfig);
            }
        }
    }
}
