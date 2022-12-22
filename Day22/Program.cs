var lines = File.ReadAllLines(".\\Input.txt");

SolvePart1(lines);

void SolvePart1(string[] lines)
{
    var map = ParseMap(lines);

    var obstacles = map.Where(p => !p.Empty)
        .ToList();

    var width = map.Max(p => p.X);
    var height = map.Max(p => p.Y);

    var leftBorder = new Dictionary<int, int>();
    var rightBorder = new Dictionary<int, int>();
    var topBorder = new Dictionary<int, int>();
    var bottomBorder = new Dictionary<int, int>();

    for (int i = 0; i < width; i++)
    {
        var column = map.Where(p => p.X == i + 1);
        var emptyColumn = map.Where(p => p.X == i + 1 && p.Empty);

        var top = column.Min(p => p.Y);
        var bottom = column.Max(p => p.Y);

        if (top == emptyColumn.Min(p => p.Y))
        {
            topBorder.Add(i + 1, top);
        }
        if(bottom == emptyColumn.Max(p => p.Y))
        {
            bottomBorder.Add(i + 1, bottom);
        }      
    }
    for (int i = 0; i < height; i++)
    {
        var row = map.Where(p => p.Y == i + 1);
        var emptyRow = map.Where(p => p.Y == i + 1 && p.Empty);

        var left = row.Min(p => p.X);
        var right = row.Max(p => p.X);

        if(left == emptyRow.Min(p => p.X))
        {
            leftBorder.Add(i + 1, left);
        }
        if(right == emptyRow.Max(p => p.X))
        {
            rightBorder.Add(i + 1, right);
        }       
    }

    var current = map
        .Where(p => p.Y == 1 && p.Empty)
        .OrderBy(p => p.X)
        .First();

    var instructionText = lines.Last();
    var instructionPosition = 0;

    var direction = new Point(1, 0, true);

    while (true)
    {
        if (instructionPosition >= instructionText.Length)
        {
            break;
        }

        var instruction = string.Empty;

        var clockWise = true;
        var rotate = false;

        while (true)
        {
            var c = instructionText[instructionPosition];

            if (instructionPosition + 1 >= instructionText.Length)
            {
                instructionPosition++;
                instruction += c;
                break;
            }

            instructionPosition++;

            if (c == 'R' || c == 'L')
            {
                clockWise = c == 'R';
                rotate = true;
                break;
            }

            instruction += c;
        }

        var steps = int.Parse(instruction);

        for (int i = 0; i < steps; i++)
        {
            var newPos = current with { X = current.X + direction.X, Y = current.Y + direction.Y };

            if(obstacles.Contains(newPos with { Empty = false }))
            {
                break;
            }

            if (direction.X != 0 && leftBorder.ContainsKey(newPos.Y) && newPos.X < leftBorder[newPos.Y])
            {
                if(rightBorder.TryGetValue(newPos.Y, out var newX))
                {
                    newPos = newPos with { X = newX };
                }
                else
                {
                    break;
                }
            }
            else if (direction.X != 0 && rightBorder.ContainsKey(newPos.Y) && newPos.X > rightBorder[newPos.Y])
            {
                if (leftBorder.TryGetValue(newPos.Y, out var newX))
                {
                    newPos = newPos with { X = newX };
                }
                else
                {
                    break;
                }
            }
            else if (direction.Y != 0 && topBorder.ContainsKey(newPos.X) && newPos.Y < topBorder[newPos.X])
            {
                if (bottomBorder.TryGetValue(newPos.X, out var newY))
                {
                    newPos = newPos with { Y = newY };
                }
                else
                {
                    break;
                }
            }
            else if (direction.Y != 0 && bottomBorder.ContainsKey(newPos.X) && newPos.Y > bottomBorder[newPos.X])
            {
                if (topBorder.TryGetValue(newPos.X, out var newY))
                {
                    newPos = newPos with { Y = newY };
                }
                else
                {
                    break;
                }
            }

            current = newPos;
        }

        if (rotate)
        {
            direction = CalculateDirection(direction, clockWise);
        }
    }

    var solution = 1000 * current.Y + current.X * 4 + CalculateFacing(direction);
    Console.WriteLine($"Part 1 solution: {solution}");
}

Point CalculateDirection(Point direction, bool clockwise)
{
    return (direction.X, direction.Y, clockwise) switch
    {
        (1, 0, true) => new(0, 1, true),
        (1, 0, false) => new(0, -1, true),
        (-1, 0, true) => new(0, -1, true),
        (-1, 0, false) => new(0, 1, true),
        (0, 1, true) => new(-1, 0, true),
        (0, 1, false) => new(1, 0, true),
        (0, -1, true) => new(1, 0, true),
        (0, -1, false) => new(-1, 0, true),
        _ => direction
    };
}

int CalculateFacing(Point direction)
{
    return direction switch
    {
        (1, 0, _) => 0,
        (-1, 0, _) => 2,
        (0, 1, _) => 1,
        (0, -1, _) => 3,
    };
}

List<Point> ParseMap(string[] lines)
{
    var points = new List<Point>();

    var i = 0;
    while (true)
    {
        var line = lines[i];
        if (string.IsNullOrEmpty(line))
        {
            break;
        }

        for (int j = 0; j < line.Length; j++)
        {
            if (line[j] == '.')
            {
                points.Add(new(j + 1, i + 1, true));
            }
            if (line[j] == '#')
            {
                points.Add(new(j + 1, i + 1, false));
            }
        }

        i++;
    };

    return points;
}


record struct Point(int X, int Y, bool Empty);