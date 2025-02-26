using static System.Console;

var rect = new Rectangle
{
    P1 = Point.Origin,
    P2 = new Point { X = 3, Y = 4 }
};

WriteLine($"Rectangle perimeter: {rect.Perimeter}");
WriteLine($"Rectangle area: {rect.Area}");

var square = new Rectangle
{
    P1 = Point.Origin,
    P2 = new Point { X = 1, Y = 1 }
};

WriteLine($"Square perimeter: {square.Perimeter}");
WriteLine($"Square area: {square.Area}");

public struct Point
{
    public required double X { get; init; }
    public required double Y { get; init; }

    public void Deconstruct(out double x, out double y)
    {
        x = X;
        y = Y;
    }

    public static Point Origin => new() { X = 0, Y = 0 };
}

public struct Rectangle
{
    public required Point P1 { get; init; }
    public required Point P2 { get; init; }

    public double Area
    {
        get
        {
            var (x1, y1) = P1;
            var (x2, y2) = P2;

            return Math.Abs(x1 - x2) * Math.Abs(y1 - y2);
        }
    }

    public double Perimeter
    {
        get
        {
            var (x1, y1) = P1;
            var (x2, y2) = P2;

            return 2 * (Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
        }
    }

    public Rectangle Translate(Point by)
    {
        return new Rectangle
        {
            P1 = new Point { X = P1.X + by.X, Y = P1.Y + by.Y },
            P2 = new Point { X = P2.X + by.X, Y = P2.Y + by.Y }
        };
    }
}
