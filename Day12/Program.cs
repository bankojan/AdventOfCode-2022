using System.IO;

var input = File.ReadAllLines(".\\Input.txt");

var mapHeight = input.Length;
var mapWidth = input[0].Trim().Length;

var grid = new int[mapHeight, mapWidth];

int startX = 0, startY = 0, endX = 0, endY = 0;

var startingPoints = new List<(int, int)>();

for (int x = 0; x < mapHeight; x++)
{
    for (int y = 0; y < mapWidth; y++)
    {
        if (input[x][y] == 'S')
        {
            grid[x, y] = 'a';
            startX = x;
            startY = y;
            startingPoints.Add((x, y));
        }
        else if (input[x][y] == 'E')
        {
            grid[x, y] = 'z';
            endX = x;
            endY = y;
        }
        else
        {
            grid[x, y] = input[x][y];
            if (input[x][y] == 'a')
            {
                startingPoints.Add((x, y));
            }
        }
            
    }
}

int[] xMoves = new int[] { -1, 1, 0, 0 };
int[] yMoves = new int[] { 0, 0, -1, 1 };

SolvePart1(grid);
SolvePart2(grid);

void SolvePart1(int[,] grid)
{
    var visited = new bool[grid.GetLength(0), grid.GetLength(1)];
    var queue = new Queue<(int, int, int)>();

    queue.Enqueue((startX, startY, 0));

    int path = BreadthFirstSearch(grid, queue, visited, startX, startY, endX, endY);
    Console.WriteLine($"Part 1 solution: {path}");
}

void SolvePart2(int[,] grid)
{
    int minPath = int.MaxValue;

    foreach (var (x, y) in startingPoints)
    {
        var visited = new bool[grid.GetLength(0), grid.GetLength(1)];

        var queue = new Queue<(int, int, int)>();
        queue.Enqueue((x, y, 0));

        int path = BreadthFirstSearch(grid, queue, visited, startX, startY, endX, endY);
        if(path < minPath)
        {
            minPath = path;
        }
    }

    Console.WriteLine($"Part 2 solution: {minPath}");
}

int BreadthFirstSearch(int[,] grid, Queue<(int, int, int)> queue, bool[,] visited, int startX, int startY, int endX, int endY)
{
    while (true)
    {
        if (!queue.Any())
        {
            break;
        }

        var (currentX, currentY, pathLength) = queue.Dequeue();

        if (visited[currentX, currentY])
        {
            continue;
        }

        visited[currentX, currentY] = true;

        if (currentX == endX && currentY == endY)
        {
            return pathLength;
        }

        for (int i = 0; i < 4; i++)
        {
            var newX = currentX + xMoves[i];
            var newY = currentY + yMoves[i];

            if(newX >= 0 && newX < mapHeight && newY >= 0 && newY < mapWidth)
            {
                var parent = grid[currentX, currentY];
                var child = grid[newX, newY];

                if(child - parent <= 1)
                {
                    queue.Enqueue((newX, newY, pathLength + 1));
                }
            }
        }
    }

    return int.MaxValue;
}