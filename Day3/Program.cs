var lines = File.ReadAllLines(".\\Input.txt");

SolvePart1(lines);
SolvePart2(lines);

void SolvePart1(string[] lines)
{
    var sum = 0;
    foreach (var line in lines)
    {
        var charDictionary = new Dictionary<char, bool>();

        var firstHalf = line.Substring(0, (line.Length / 2));
        var secondHalf = line.Substring(line.Length / 2);

        for (int i = 0; i < firstHalf.Length; i++)
        {
            charDictionary.TryAdd(firstHalf[i], true);
        }

        var repeat = secondHalf
            .Select(c => (repeats: charDictionary.ContainsKey(c), character: c))
            .FirstOrDefault(c => c.repeats);

        sum += ToNumber(repeat.character);
    }

    Console.WriteLine($"Part 1 solution: {sum}");
}

void SolvePart2(string[] lines)
{
    var sum = 0;

    var groups = lines.Chunk(3);

    foreach (var group in groups)
    {
        var dict1 = new Dictionary<char, bool>();
        var dict2 = new Dictionary<char, bool>();

        for (int i = 0; i < group[0].Length; i++)
        {
            dict1.TryAdd(group[0][i], true);
        }
        for (int i = 0; i < group[1].Length; i++)
        {
            dict2.TryAdd(group[1][i], true);
        }

        var repeat = group[2]
            .Select(c => (repeats: dict1.ContainsKey(c) && dict2.ContainsKey(c), character: c))
            .FirstOrDefault(c => c.repeats);

        sum += ToNumber(repeat.character);
    }

    Console.WriteLine($"Part 2 solution: {sum}");
}

int ToNumber(char c) => c >= 97 ? c - 96 : c - 38;