using System.Text;

var lines = File.ReadAllLines(".\\Input.txt");

var (stacks, instructionIndex) = ExtractStacks(lines);
var instructionLines = lines.Skip(instructionIndex).ToArray();

SolvePart1(stacks, instructionLines);

(stacks, instructionIndex) = ExtractStacks(lines);
SolvePart2(stacks, instructionLines);

(List <Stack<char>> stacks, int instructionLineStart) ExtractStacks(string[] lines)
{
    var indexLine = lines
        .Select((l, i) => (line: l, index: i))
        .FirstOrDefault(line => line.line[1] == '1');

    var maxStackHeight = indexLine.index;
    var numberOfStacks = int.Parse(indexLine.line.Split(' ')[^2]);

    var stacks = new List<Stack<char>>();
    for (int i = 0; i < numberOfStacks; i++)
    {
        stacks.Add(new());
    }

    foreach (var line in lines.Take(maxStackHeight).Reverse())
    {
        for (int i = 0; i < numberOfStacks; i++)
        {
            var crate = line[1 + (i * 4)];

            if (crate != ' ')
            {
                stacks[i].Push(crate);
            }
        }
    }

    return (stacks, maxStackHeight + 2);
}

void SolvePart1(List<Stack<char>> stacks, string[] instructionLines)
{
    foreach (var instructionLine in instructionLines)
    {
        var instructionSplit = instructionLine.Split(" ");

        var toPop = int.Parse(instructionSplit[1]);
        var from = int.Parse(instructionSplit[3]) - 1;
        var to = int.Parse(instructionSplit[5]) - 1;

        for (int i = 0; i < toPop; i++)
        {
            stacks[to].Push(stacks[from].Pop());
        }
    }

    var resultBuilder = new StringBuilder();
    for (int i = 0; i < stacks.Count; i++)
    {
        resultBuilder.Append(stacks[i].Pop());
    }

    Console.WriteLine($"Part 1 solution: {resultBuilder}");
}

void SolvePart2(List<Stack<char>> stacks, string[] instructionLines)
{
    foreach (var instructionLine in instructionLines)
    {
        var instructionSplit = instructionLine.Split(" ");

        var toPop = int.Parse(instructionSplit[1]);
        var from = int.Parse(instructionSplit[3]) - 1;
        var to = int.Parse(instructionSplit[5]) - 1;

        var crates = new List<char>();
        for (int i = 0; i < toPop; i++)
        {
            crates.Add(stacks[from].Pop());
        }

        for (int i = toPop - 1; i >= 0; i--)
        {
            stacks[to].Push(crates[i]);
        }
    }

    var resultBuilder = new StringBuilder();
    for (int i = 0; i < stacks.Count; i++)
    {
        resultBuilder.Append(stacks[i].Pop());
    }

    Console.WriteLine($"Part 2 solution: {resultBuilder}");
}