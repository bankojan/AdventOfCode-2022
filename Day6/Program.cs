var text = File.ReadAllText(".\\Input.txt");

Console.WriteLine($"Part 1 solution: {FindStartMarker(text, 4)}");
Console.WriteLine($"Part 2 solution: {FindStartMarker(text, 14)}");

int FindStartMarker(string text, int n)
    => text
        .Select((_, i) => i)
        .First(index => text.Skip(index).Take(n).ToHashSet().Count == n) + n;