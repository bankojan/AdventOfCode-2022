var input = File.ReadAllText(".\\Input1.txt");

CalculatePart1(input);
CalculatePart2(input);

void CalculatePart1(string input)
{
    var max = input
        .Split($"{Environment.NewLine}{Environment.NewLine}")
        .Select(line => line.Split(Environment.NewLine))
        .Select(lines => lines.Select(line => int.Parse(line)))
        .Select(group => group.Sum())
        .Max();

    Console.WriteLine($"Part 1 solution: {max}");
}

void CalculatePart2(string input)
{
    var sum = input
        .Split($"{Environment.NewLine}{Environment.NewLine}")
        .Select(line => line.Split(Environment.NewLine))
        .Select(lines => lines.Select(line => int.Parse(line)))
        .Select(group => group.Sum())
        .OrderByDescending(sum => sum)
        .Take(3)
        .Sum();

    Console.WriteLine($"Part 2 solution: {sum}");
}