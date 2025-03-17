using System.Collections.Immutable;
using System.Diagnostics;
using ConsoleAppFramework;
using KSharp.Compiler;
using KSharp.Compiler.IR;
using KSharp.Compiler.Syntax;
using KSharp.Compiler.TypeScript;

// Register the command handlers
var app = ConsoleApp.Create(args);

// Define the build command
app.AddCommand("build", "Compiles K# source files to the target language", async (
    [Option("i", "Input directory containing K# source files")] string input,
    [Option("t", "Target language (TypeScript or CSharp)")] Target target = Target.TypeScript,
    [Option("o", "Output directory")] string output = "output",
    [Option("p", "Generate package.json for Node.js projects")] bool generatePackageJson = false,
    [Option("ts", "Generate tsconfig.json for TypeScript projects")] bool generateTsConfig = false
) =>
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
                throw new ArgumentOutOfRangeException(nameof(target), "Unsupported target platform.");
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
});

// Run the application
await app.RunAsync();

// Helper methods
async Task CompileToTypeScript(
    string inputPath, 
    string outputPath, 
    bool generatePackageJson, 
    bool generateTsConfig)
{
    // Create output directory if it doesn't exist
    Directory.CreateDirectory(outputPath);
    
    // Find all .ks source files
    var sourceFiles = Directory.GetFiles(inputPath, "*.ks", SearchOption.AllDirectories);
    
    if (sourceFiles.Length == 0)
    {
        throw new FileNotFoundException($"No K# source files (*.ks) found in {inputPath}");
    }
    
    Console.WriteLine($"Found {sourceFiles.Length} K# source files");
    
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
    var projectName = Path.GetFileName(inputPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
    var compilation = new CompilationSyntax
    {
        Name = new IdentifierTokenSyntax(projectName),
        RootDirectory = inputPath,
        SourceFiles = compilationUnits.ToImmutableList(),
    };
    
    // Transform to IR
    Console.WriteLine("Generating intermediate representation (IR)");
    var transformer = new SyntaxTransformer();
    var irCompilation = transformer.Transform(compilation);
    
    // Transpile to TypeScript
    Console.WriteLine("Transpiling to TypeScript");
    var tsTranspiler = new TypeScriptTranspiler();
    var tsFiles = tsTranspiler.Transpile(irCompilation);
    
    // Write output files
    Console.WriteLine("Writing TypeScript files");
    foreach (var (filePath, tsSourceFile) in tsFiles)
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
            var packageJson = $@"{{
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
            var tsconfig = @"{
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
