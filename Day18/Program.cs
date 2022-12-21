var cubes = File.ReadAllLines(".\\Input.txt")
    .Select(l =>
    {
        var split = l.Split(",");

        return new Point(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
    })
    .ToList();

//SolvePart1(cubes);
SolvePart2(cubes);

void SolvePart1(List<Point> cubes)
{
    var sides = cubes
        .Select(c => 6 - cubes.Except(new List<Point> { c }).Select(c2 => c.ConnectsWith(c2)).Count(c => c))
        .Sum();

    Console.WriteLine($"Part 1 solution: {sides}");
}

void SolvePart2(List<Point> cubes)
{
    var airPockets = new HashSet<Point>();
    var emptyCubes = new HashSet<Point>();

    int minX = cubes.Min(c => c.X);
    int maxX = cubes.Max(c => c.X);

    int minY = cubes.Min(c => c.Y); 
    int maxY = cubes.Max(c => c.Y);

    int maxZ = cubes.Max(c => c.Z);
    int minZ = cubes.Min(c => c.Z);

    Point entry = default;

    for (int i = minX; i <= maxX; i++)
    {
        for (int j = minY; j <= maxY; j++)
        {
            for (int k = minZ; k <= maxZ; k++)
            {
                var c2 = new Point(i, j, k);
                if (!cubes.Contains(c2))
                {
                    var connected = cubes.Select(c => c.ConnectsWith(c2)).Count(c => c);

                    if(entry == default && (i == minX || i == maxX || j == minY || j == maxY || k == minZ || k == maxZ) && connected < 6)
                    {
                        entry = c2;
                    }

                    emptyCubes.Add(c2);
                }
            }
        }
    }

    FloodFill(entry, emptyCubes);

    var airPocketSides = emptyCubes
        .Select(c => cubes.Select(c2 => c.ConnectsWith(c2)).Count(c => c))
        .Sum();

    var sides = cubes
        .Select(c => 6 - cubes.Except(new List<Point> { c }).Select(c2 => c.ConnectsWith(c2)).Count(c => c))
        .Sum();

    Console.WriteLine($"Part 2 solution: {sides - airPocketSides}");
}

// 36 pocket 3446 too high

void FloodFill(Point entry, HashSet<Point> emptyCubes)
{
    var stack = new Stack<Point>();
    stack.Push(entry);

    while (stack.TryPop(out var current))
    {
        if (emptyCubes.Contains(current))
        {
            emptyCubes.Remove(current);
        }

        var neighbours = new List<Point>
        {
            current with { X = current.X - 1 },
            current with { X = current.X + 1 },
            current with { Y = current.Y + 1 },
            current with { Y = current.Y - 1 },
            current with { Z = current.Z + 1 },
            current with { Z = current.Z - 1 }
        };

        foreach (var neighbour in neighbours)
        {
            if (emptyCubes.Contains(neighbour))
            {
                stack.Push(neighbour);
            }
        }
    }


    
}

record struct Point(int X, int Y, int Z)
{
    public bool ConnectsWith(Point p2)
    {
        var xDif = Math.Abs(p2.X - X);
        var yDif = Math.Abs(p2.Y - Y);
        var zDif = Math.Abs(p2.Z - Z);

        if(xDif >= 2 || yDif >= 2 || zDif >= 2)
            return false;

        var x = xDif > 0 ? 1 : 0;
        var y = yDif > 0 ? 1 : 0;
        var z = zDif > 0 ? 1 : 0;

        return x + y + z <= 1;
    }
}