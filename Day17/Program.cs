using System.Drawing;

var input = File.ReadAllText(".\\Input.txt");

var shapes = new List<Shape>
{
    new Shape(new List<Point>()
    {
        new(0, 0), new(1, 0), new(2, 0), new(3, 0)
    }),
    new Shape(new List<Point>()
    {
        new(0, 0), new(1, 0), new(2, 0), new(1, -1), new(1, 1)
    }),
    new Shape(new List<Point>()
    {
        new(0, 0), new(1, 0), new(2, 0), new(2, 1), new(2, 2)
    }),
    new Shape(new List<Point>()
    {
        new(0, 0), new(0, -1), new(0, -2), new(0, -3)
    }),
    new Shape(new List<Point>()
    {
        new(0, 0), new(1, 0), new(0, -1), new(1, -1)
    }),
};

SolvePart1(input, 2022);
SolvePart2(input);

void SolvePart1(string input, int rounds)
{
    var res = Simulate(input, rounds, "1");

    Console.WriteLine($"Part {1} solution: {res.height}");
}

void SolvePart2(string input)
{
    var (res1, tick1) = Simulate(input, 1936, "2");
    var (resCont, tickCont) = Simulate(input, 1735 + (1936 % shapes.Count), "", 1936 % shapes.Count, tick1 % input.Length);

    tickCont -= tick1 % input.Length;

    var allRounds = 1000000000000L;
    var repeatCount = (allRounds - 1935) / 1735;

    var start = 1936 + repeatCount * 1735;
    var afterRepeat = allRounds - start;

    var tickAfterRepeat = tick1 + (tickCont * repeatCount);

    var (res2, ticks2) = Simulate(input, (int)afterRepeat + ((int)(start % shapes.Count)), "", (int)(start % shapes.Count), (int)(tickAfterRepeat % input.Length));

    var final = res1 + resCont * repeatCount + res2;

    Console.WriteLine($"Part {2} solution: {final}");
}

(int height, int ticks) Simulate(string input, int rounds, string part, int round = 0, int tick = 0)
{
    var spawn = true;

    var floor = new int[] { 0, 0, 0, 0, 0, 0, 0 };
    var currentPosition = new Point();

    var stoppedRocks = new HashSet<Point>();

    while (true)
    {
        if(round == rounds)
        {
            break;
        }

        var currentShape = shapes[round % shapes.Count];
        
        if (spawn)
        {
            var topMost = floor.Max() + 4;
            currentPosition = new(3, topMost - currentShape.Offset);

            spawn = false;
        }

        var moveRight = input[(tick++) % input.Length] == '>' ? true : false;

        var newPosition = moveRight ?
            new Point(currentPosition.X + 1, currentPosition.Y) :
            new Point(currentPosition.X - 1, currentPosition.Y);

        if (!currentShape.Intersects(newPosition, stoppedRocks))
        {
            currentPosition = newPosition;
        }

        currentPosition.Y--;

        if(currentShape.IntersectWithBottom(currentPosition, floor, stoppedRocks))
        {
            spawn = true;
            currentPosition.Y++;
            currentShape.CalculateNewState(floor, stoppedRocks, currentPosition);

            if(floor.ToHashSet().Count == 1)
            {
                var a = "";
            }

            round++;
        }       
    }

    //3148

    return (floor.Max(), tick);
}

void Visualize(HashSet<Point> rocks)
{
    var top = rocks.Max(r => r.Y) + 10;

    for (int i = top; i >= 0; i--)
    {
        for (int j = 0; j <= 8; j++)
        {
            if((i == 0 && j == 0) || (i == 0 && j == 8))
            {
                Console.Write("+");
            }
            else if(j == 0 || j == 8)
            {
                Console.Write("|");
            }
            else if(i == 0)
            {
                Console.Write("-");
            }
            else
            {
                Console.Write(rocks.Contains(new(j, i)) ? "#" : ".");
            }           
        }
        Console.WriteLine();
    }
}

record Shape(List<Point> Points)
{
    public int Offset { get; set; } = Points.Select(p => p.Y).Min();

    public bool Intersects(Point position, HashSet<Point> stoppedRocks)
    {
        foreach (var pos in Points)
        {
            var xPos = position.X + pos.X;
            var yPos = position.Y + pos.Y;

            if (xPos == 0 || xPos == 8)
            {
                return true;
            }

            if(stoppedRocks.Contains(new(xPos, yPos)))
            {
                return true;
            }
        }

        return false;
    }

    public bool IntersectWithBottom(Point position, int[] bottomPositions, HashSet<Point> stoppedRocks)
    {
        foreach (var pos in Points)
        {
            var yPos = position.Y + pos.Y;
            var xPos = position.X + pos.X;

            var bottomPos = bottomPositions[xPos - 1];

            if (yPos == bottomPos)
            {
                return true;
            }
            if(stoppedRocks.Contains(new(xPos, yPos)))
            {
                return true;
            }
        }

        return false;
    }

    public void CalculateNewState(int[] floor, HashSet<Point> stoppedRocks, Point position)
    {
        foreach (var point in Points)
        {
            var yPos = position.Y + point.Y;
            var xPos = position.X + point.X;

            if (floor[xPos - 1] < yPos)
            {
                floor[xPos - 1] = yPos;
            }

            stoppedRocks.Add(new(xPos, yPos));
        }
    }
}

record struct Point(int X, int Y);