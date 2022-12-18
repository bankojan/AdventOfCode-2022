var lines = File.ReadAllLines(".\\Input.txt");

//SolvePart1(lines);
SolvePart2(lines);

void SolvePart1(string[] lines)
{
    var cavern = new bool[1000, 1000];
    var maxX = 0;

    foreach (var line in lines)
    {
        var points = new List<(int x, int y)>();

        var split = line.Split("->");

        foreach (var item in split)
        {
            var pair = item.Split(",");
            var x = int.Parse(pair[1]);
            var y = int.Parse(pair[0]);

            points.Add((y, x));

            if (x > maxX)
                maxX = x;
        }

        BuildCavern(points, cavern);
    }

    var iteration = 0;

    while (true)
    {
        var currentLocation = (x: 0, y: 500);
        iteration++;

        while (true)
        {
            var (x, y) = currentLocation;

            if (!cavern[x + 1, y])
            {
                if(x + 1 > maxX)
                {
                    break;
                }
                currentLocation = (x + 1, y);
                continue;
            }
            if (!cavern[x + 1, y - 1])
            {
                if (x + 1 > maxX)
                {
                    break;
                }
                currentLocation = (x + 1, y - 1);
                continue;
            }
            if (!cavern[x + 1, y + 1])
            {
                if (x + 1 > maxX)
                {
                    break;
                }
                currentLocation = (x + 1, y + 1);
                continue;
            }

            cavern[x, y] = true;
            break;
        }

        if (currentLocation.x >= maxX)
        {
            //VisualizeCavern(cavern);
            Console.WriteLine();
            Console.WriteLine($"Part 1 solution: {iteration - 1} ({currentLocation})");
            break;
        }
    }
}

void SolvePart2(string[] lines)
{
    var cavern = new bool[1000, 1000];
    var maxX = 0;

    foreach (var line in lines)
    {
        var points = new List<(int x, int y)>();

        var split = line.Split("->");

        foreach (var item in split)
        {
            var pair = item.Split(",");
            var x = int.Parse(pair[1]);
            var y = int.Parse(pair[0]);

            points.Add((y, x));

            if (x > maxX)
                maxX = x;
        }

        BuildCavern(points, cavern);
    }

    for (int i = 0; i < 1000; i++)
    {
        cavern[maxX + 2, i] = true;
    }

    var iteration = 0;

    while (true)
    {
        var currentLocation = (x: 0, y: 500);
        iteration++;

        while (true)
        {
            var (x, y) = currentLocation;

            if (!cavern[x + 1, y])
            {                
                currentLocation = (x + 1, y);
                continue;
            }
            if (!cavern[x + 1, y - 1])
            {            
                currentLocation = (x + 1, y - 1);
                continue;
            }
            if (!cavern[x + 1, y + 1])
            {               
                currentLocation = (x + 1, y + 1);
                continue;
            }

            cavern[x, y] = true;
            break;
        }

        if (currentLocation.x == 0 && currentLocation.y == 500)
        {
            Console.WriteLine();
            Console.WriteLine($"Part 2 solution: {iteration} ({currentLocation})");
            break;
        }
    }
}

void VisualizeCavern(bool[,] cavern)
{
    for (int i = 0; i < 200; i++)
    {
        for (int j = 480; j < 600; j++)
        {
            Console.Write(cavern[i, j] ? "#" : ".");
        }
        Console.WriteLine();
    }

}


void BuildCavern(List<(int y, int x)> points, bool[,] cavern)
{
    for (int p = 0; p < points.Count - 1; p++)
    {
        var point1 = points[p];
        var point2 = points[p + 1];

        if (point1.x == point2.x)
        {
            if (point1.y > point2.y)
            {
                for (int i = point2.y; i <= point1.y; i++)
                {
                    cavern[point1.x, i] = true;
                }
            }
            else
            {
                for (int i = point1.y; i <= point2.y; i++)
                {
                    cavern[point1.x, i] = true;
                }
            }
        }
        if (point1.y == point2.y)
        {
            if (point1.x > point2.x)
            {
                for (int i = point2.x; i <= point1.x; i++)
                {
                    cavern[i, point1.y] = true;
                }
            }
            else
            {
                for (int i = point1.x; i <= point2.x; i++)
                {
                    cavern[i, point1.y] = true;
                }
            }
        }
    }
}