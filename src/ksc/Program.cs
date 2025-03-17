using System.Collections.Immutable;
using System.Diagnostics;
using KSharp.Compiler;
using KSharp.Compiler.IR;
using KSharp.Compiler.Syntax;
using KSharp.Compiler.TypeScript;

// Default output directory
const string DefaultOutputDir = "output";

// Check command line arguments
if (args.Length < 1)
{
    Console.WriteLine("Usage: ksc <input-path> [--target <target>] [--output <output-dir>]");
    Console.WriteLine("  <input-path>: Path to K# project directory");
    Console.WriteLine("  --target, -t: Target language (TypeScript or CSharp). Default: TypeScript");
    Console.WriteLine("  --output, -o: Output directory. Default: ./output");
    return 1;
}

// Parse command line arguments
var input = args[0];
var target = Target.TypeScript;
var output = DefaultOutputDir;

// Parse additional arguments
for (var i = 1; i < args.Length; i++)
{
    if ((args[i] == "--target" || args[i] == "-t") && i + 1 < args.Length)
    {
        if (Enum.TryParse<Target>(args[i + 1], true, out var parsedTarget))
        {
            target = parsedTarget;
            i++;
        }
    }
    else if ((args[i] == "--output" || args[i] == "-o") && i + 1 < args.Length)
    {
        output = args[i + 1];
        i++;
    }
}

Console.WriteLine($"Compiling from: {input}");
Console.WriteLine($"Target: {target}");
Console.WriteLine($"Output directory: {output}");

try
{
    // Compile to the selected target
    switch (target)
    {
        case Target.TypeScript:
            await CompileToTypeScript(input, output);
            break;
            
        case Target.CSharp:
            throw new NotImplementedException("C# target is not yet implemented.");
            
        default:
            throw new ArgumentOutOfRangeException(nameof(target), "Unsupported target platform.");
    }
    
    Console.WriteLine("Compilation successful.");
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Compilation failed: {ex.Message}");
    
    if (Debugger.IsAttached)
    {
        Console.Error.WriteLine(ex.StackTrace);
    }
    
    return 1;
}

// Transpile K# to TypeScript
async Task CompileToTypeScript(string inputPath, string outputPath)
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
    
    // Create package.json if it doesn't exist
    var packageJsonPath = Path.Combine(outputPath, "package.json");
    if (!File.Exists(packageJsonPath))
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
    
    // Create tsconfig.json if it doesn't exist
    var tsconfigPath = Path.Combine(outputPath, "tsconfig.json");
    if (!File.Exists(tsconfigPath))
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
