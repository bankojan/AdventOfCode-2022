var lines = File.ReadAllLines(".\\Input.txt");
var grid = lines
    .Select(l => l.Select(c => int.Parse(c.ToString())).ToList())
    .ToList();

SolvePart1(grid);
SolvePart2(grid);

void SolvePart1(List<List<int>> grid)
{
    var visibleTrees = grid.Count * 2 + (grid[0].Count() - 2) * 2; 

    for (int i = 1; i < grid.Count - 1; i++)
    {
        for (int j = 1; j < grid[i].Count - 1; j++)
        {
            var treeHeight = grid[i][j];

            var top = !grid.Take(i).Select(row => row[j]).Any(tree => tree >= treeHeight);
            var bottom = !grid.Skip(i + 1).Select(row => row[j]).Any(tree => tree >= treeHeight);

            var left = !grid[i].Take(j).Any(tree => tree >= treeHeight);
            var right = !grid[i].Skip(j + 1).Any(tree => tree >= treeHeight);

            visibleTrees += (top || bottom || left || right) ? 1 : 0;
        }
    }

    Console.WriteLine($"Part 1 solution: {visibleTrees}");
}

void SolvePart2(List<List<int>> grid)
{
    var maxScenicScore = 0;

    for (int i = 1; i < grid.Count - 1; i++)
    {
        for (int j = 1; j < grid[i].Count - 1; j++)
        {
            var treeHeight = grid[i][j];

            var top = grid.Take(i).Select(row => row[j]).Reverse().ToList();
            var bottom = grid.Skip(i + 1).Select(row => row[j]).ToList();

            var left = grid[i].Take(j).Reverse().ToList();
            var right = grid[i].Skip(j + 1).ToList();

            var topTree = top.Select((tree, index) => (tree, index))
                .FirstOrDefault(tree => tree.tree >= treeHeight);
            var topScore = topTree == default ? top.Count : topTree.index + 1;

            var bottomTree = bottom.Select((tree, index) => (tree, index))
                .FirstOrDefault(tree => tree.tree >= treeHeight);
            var bottomScore = bottomTree == default ? bottom.Count : bottomTree.index + 1;

            var leftTree = left.Select((tree, index) => (tree, index))
                .FirstOrDefault(tree => tree.tree >= treeHeight);
            var leftScore = leftTree == default ? left.Count : leftTree.index + 1 ;

            var rightTree = right.Select((tree, index) => (tree, index))
                .FirstOrDefault(tree => tree.tree >= treeHeight);
            var rightScore = rightTree == default ? right.Count : rightTree.index + 1;

            var scenicScore = topScore * bottomScore * leftScore * rightScore;

            maxScenicScore = int.Max(maxScenicScore, scenicScore);
        }
    }

    Console.WriteLine($"Part 2 solution: {maxScenicScore}");
}