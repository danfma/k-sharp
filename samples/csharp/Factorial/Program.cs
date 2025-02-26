using static System.Console;

var n = int.Parse(args[0]);

WriteLine(Factorial(n));

int Factorial(int n)
{
    if (n <= 1)
    {
        return 1;
    }

    return n * Factorial(n - 1);
}
