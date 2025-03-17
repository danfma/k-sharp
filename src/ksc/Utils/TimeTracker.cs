using System.Diagnostics;

namespace KSharp.Compiler.Utils;

public struct TimeTracker
{
    public long StartTime { get; private set; }

    public TimeSpan GetElapsedTime(bool reset = true)
    {
        var now = Stopwatch.GetTimestamp();
        var elapsedTime = Stopwatch.GetElapsedTime(StartTime, now);

        if (reset)
        {
            StartTime = now;
        }

        return elapsedTime;
    }

    public void PrintElapsedTime(string context, bool reset = true)
    {
        var elapsedTime = GetElapsedTime(reset);

        Console.WriteLine($"Elapsed time for {context}: {elapsedTime.TotalMilliseconds} ms");
    }

    public static TimeTracker Start()
    {
        return new TimeTracker { StartTime = Stopwatch.GetTimestamp() };
    }
}
