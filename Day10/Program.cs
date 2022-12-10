var commandLines = File.ReadAllLines(".\\Input.txt");

var commands = commandLines
    .Select(c => c.Split(' '))
    .Select(split => (split[0], split.Length > 1 ? int.Parse(split[1]) : 0))
    .ToList();

SolvePart1(commands);
SolvePart2(commands);

void SolvePart1(List<(string exectute, int value)> commands)
{
    var x = 1;

    var cycles = new List<int>();

    foreach (var (execute, value) in commands)
    {
        if(execute == "addx")
        {
            cycles.Add(x);
            cycles.Add(x);
            x += value;
        }
        else
        {
            cycles.Add(x);
        }
    }

    var sum = (cycles[19] * 20) +
        (cycles[59] * 60) +
        (cycles[99] * 100) +
        (cycles[139] * 140) +
        (cycles[179] * 180) +
        (cycles[219] * 220);

    Console.WriteLine($"Part 1 solution: {sum}");
}

void SolvePart2(List<(string exectute, int value)> commands)
{
    var x = 1;

    var cycles = new List<int>();

    foreach (var (execute, value) in commands)
    {
        if (execute == "addx")
        {
            cycles.Add(x);
            cycles.Add(x);
            x += value;
        }
        else
        {
            cycles.Add(x);
        }
    }

    var lines = Enumerable.Range(0, 6)
        .Select(_ => String.Empty)
        .ToList();

    for (int i = 0; i < cycles.Count; i++)
    {
        var cycleValue = cycles[i];
        var crtValue = i % 40;

        lines[i / 40] += crtValue >= cycleValue - 1 && crtValue <= cycleValue + 1 ? "#" : ".";
    }

    foreach (var line in lines)
    {
        Console.WriteLine(line);
    }
}