var simplePointDictionary = new Dictionary<(string, string), int>
{
    { ("A", "X"), 4 },
    { ("A", "Y"), 8 },
    { ("A", "Z"), 3 },
    { ("B", "X"), 1 },
    { ("B", "Y"), 5 },
    { ("B", "Z"), 9 },
    { ("C", "X"), 7 },
    { ("C", "Y"), 2 },
    { ("C", "Z"), 6 },
};

var strategyPointDictionary = new Dictionary<(string, string), int>
{
    { ("A", "X"), 3 },
    { ("A", "Y"), 4 },
    { ("A", "Z"), 8 },

    { ("B", "X"), 1 },
    { ("B", "Y"), 5 },
    { ("B", "Z"), 9 },

    { ("C", "X"), 2 },
    { ("C", "Y"), 6 },
    { ("C", "Z"), 7 },
};

SolvePart1();
SolvePart2();

void SolvePart1()
{
    var sum = File.ReadAllLines(".\\Input.txt")
        .Select(line => line.Split(" "))
        .Select(pair => simplePointDictionary[(pair[0], pair[1])])
        .Sum();

    Console.WriteLine($"Part 1 solution: {sum}");
}

void SolvePart2()
{
    var sum = File.ReadAllLines(".\\Input.txt")
        .Select(line => line.Split(" "))
        .Select(pair => strategyPointDictionary[(pair[0], pair[1])])
        .Sum();

    Console.WriteLine($"Part 2 solution: {sum}");
}
