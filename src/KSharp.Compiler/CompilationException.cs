namespace KSharp.Compiler;

/// <summary>
/// Exceção lançada quando ocorre um erro de compilação
/// </summary>
public class CompilationException(string message, string[] errors) : Exception(message)
{
    public string[] Errors { get; } = errors;
}
