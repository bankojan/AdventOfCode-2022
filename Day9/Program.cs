var lines = File.ReadAllLines(".\\Input.txt");

SolvePart1(lines);
SolvePart2(lines);

void SolvePart1(string[] movementInstructions)
{
    var visitedDictionary = new Dictionary<(int x, int y), bool>();

    var headPosition = (x: 0, y: 0);
    var tailPosition = (x: 0, y: 0);

    visitedDictionary.Add(tailPosition, true);

    foreach (var movementInstruction in movementInstructions)
    {
        var movementInstructionSplit = movementInstruction.Split(' ');

        var moveLength = int.Parse(movementInstructionSplit[1]);
        var (x, y) = PerformMove(movementInstructionSplit[0]);

        for (int i = 0; i < moveLength; i++)
        {
            headPosition.x += x;
            headPosition.y += y;

            var (moveX, moveY) = PerfomTailMove(headPosition, tailPosition);

            tailPosition.x += moveX;
            tailPosition.y += moveY;

            visitedDictionary.TryAdd(tailPosition, true);
        }
    }

    Console.WriteLine($"Part 1 solution: {visitedDictionary.Keys.Count}");
}

void SolvePart2(string[] movementInstructions)
{
    var visitedDictionary = new Dictionary<(int x, int y), bool>();

    var positions = new List<(int x, int y)>();

    for (int i = 0; i < 10; i++)
    {
        positions.Add((0, 0));
    }

    visitedDictionary.Add((0, 0), true);

    foreach (var movementInstruction in movementInstructions)
    {
        var movementInstructionSplit = movementInstruction.Split(' ');

        var moveLength = int.Parse(movementInstructionSplit[1]);
        var (x, y) = PerformMove(movementInstructionSplit[0]);

        for (int i = 0; i < moveLength; i++)
        {
            var head = positions[0];
            head.x += x;
            head.y += y;

            positions[0] = head;

            for (int j = 1; j < 10; j++)
            {
                var (moveX, moveY) = PerfomTailMove(positions[j - 1], positions[j]);

                var node = positions[j];
                node.x += moveX;
                node.y += moveY;

                positions[j] = node;
            }

            visitedDictionary.TryAdd(positions.Last(), true);
        }
    }

    Console.WriteLine($"Part 2 solution: {visitedDictionary.Keys.Count}");
}

(int x, int y) PerformMove(string move)
    => move switch
    {
        "R" => (1, 0),
        "L" => (-1, 0),
        "U" => (0, 1),
        "D" => (0, -1),
        _ => default
    };


(int x, int y) PerfomTailMove((int x, int y) head, (int x, int y) tail)
{
    var xDistance = Math.Abs(head.x - tail.x);
    var yDistance = Math.Abs(head.y - tail.y);

    var xMultiplier = head.x > tail.x ? 1 : -1;
    var yMultiplier = head.y > tail.y ? 1 : -1;

    return (xDistance, yDistance) switch
    {
        ( <= 1, <= 1) => (0, 0),
        ( > 1, 0) => (xMultiplier, 0),
        (0, > 1) => (0, yMultiplier),
        (2, _) => (xMultiplier, yMultiplier),
        (_, 2) => (xMultiplier, yMultiplier),
        _ => (0, 0),
    };
}