namespace KSharp.Compiler.AST;

public sealed record BinaryOperator : SyntaxNode
{
    private static readonly Dictionary<string, BinaryOperator> OperatorBySymbol = new();
    
    private BinaryOperator(string symbol)
    {
        Symbol = symbol;
        OperatorBySymbol.Add(symbol, this);
    }

    public string Symbol { get; }

    public static BinaryOperator Parse(string symbol)
    {
        if (!OperatorBySymbol.TryGetValue(symbol, out var binaryOperator))
            throw new ArgumentException($"Unknown binary operator '{symbol}'");

        return binaryOperator;
    }
}
