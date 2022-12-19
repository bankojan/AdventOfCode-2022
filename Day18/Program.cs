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

    for (int i = cubes.Min(c => c.X); i <= cubes.Max(c => c.X); i++)
    {
        for (int j = cubes.Min(c => c.Y); j <= cubes.Max(c => c.Y); j++)
        {
            for (int k = cubes.Min(c => c.Z); k <= cubes.Max(c => c.Z); k++)
            {
                emptyCubes.Add(new(i, j, k));
            }
        }
    }

    for (int i = cubes.Min(c => c.X); i <= cubes.Max(c => c.X); i++)
    {
        for (int j = cubes.Min(c => c.Y); j <= cubes.Max(c => c.Y); j++)
        {
            for (int k = cubes.Min(c => c.Z); k <= cubes.Max(c => c.Z); k++)
            {
                var c2 = new Point(i, j, k);
                if (!cubes.Contains(c2))
                {
                    var connected = cubes.Select(c => c.ConnectsWith(c2)).Count(c => c);
                    var connectedWithEmpty = cubes.Concat(emptyCubes.Except(new List<Point> { c2 })).Select(c => c.ConnectsWith(c2)).Count(c => c);

                    if (connected == 6 || (connected > 1 && connectedWithEmpty == 6))
                    {
                        airPockets.Add(c2);
                    }
                }
            }
        }
    }

    var sides = cubes
        .Select(c => 6 - cubes.Except(new List<Point> { c }).Select(c2 => c.ConnectsWith(c2)).Count(c => c))
        .Sum();

    Console.WriteLine($"Part 2 solution: {sides - airPockets.Count * 6}");
}

// 36 pocket 3446 too high

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