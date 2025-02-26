FizzBuzzTo(100);

bool IsDivisibleBy(int number, int divisor)
{
    if (divisor == 0)
    {
        return false;
    }

    return number % divisor == 0;
}

void FizzBuzz(int number)
{
    if (IsDivisibleBy(number, 15))
    {
        Console.WriteLine("FizzBuzz");
    }
    else if (IsDivisibleBy(number, 3))
    {
        Console.WriteLine("Fizz");
    }
    else if (IsDivisibleBy(number, 5))
    {
        Console.WriteLine("Buzz");
    }
    else
    {
        Console.WriteLine(number);
    }
}

void FizzBuzzTo(int number)
{
    for (var i = 1; i < number; i++)
    {
        FizzBuzz(i);
    }
}
