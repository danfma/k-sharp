using ConsoleAppFramework;
using KSharp.Compiler;

// Register the command handlers
var app = ConsoleApp.Create();
app.Add<Commands>();

await app.RunAsync(args);
