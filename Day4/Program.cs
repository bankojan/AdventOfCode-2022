var lines = File.ReadAllLines(".\\Input.txt");

SolvePart1(lines);
SolvePart2(lines);

void SolvePart1(string[] lines)
{
    var completeIntersections = lines
        .Where(line =>
        {
            var ranges = line
                .Split(",")
                .Select(rangeString => rangeString.Split("-"))
                .Select(range => (start: int.Parse(range[0]), end: int.Parse(range[1])))
                .ToList();

            var range1 = Enumerable.Range(ranges[0].start, ranges[0].end - ranges[0].start + 1);
            var range2 = Enumerable.Range(ranges[1].start, ranges[1].end - ranges[1].start + 1);

            return CompleteIntersect(range1, range2);
        });

    Console.WriteLine($"Part 1 solution: {completeIntersections.Count()}");
}

void SolvePart2(string[] lines)
{
    var partialIntersections = lines
        .Where(line =>
        {
            var ranges = line
                .Split(",")
                .Select(rangeString => rangeString.Split("-"))
                .Select(range => (start: int.Parse(range[0]), end: int.Parse(range[1])))
                .ToList();

            var range1 = Enumerable.Range(ranges[0].start, ranges[0].end - ranges[0].start + 1);
            var range2 = Enumerable.Range(ranges[1].start, ranges[1].end - ranges[1].start + 1);

            return PartialIntersect(range1, range2);
        });

    Console.WriteLine($"Part 2 solution: {partialIntersections.Count()}");
}

bool CompleteIntersect(IEnumerable<int> first, IEnumerable<int> second)
{
    if(first.Count() > second.Count())
    {
        return second.Intersect(first).Count() == second.Count();
    }
    else
    {
        return first.Intersect(second).Count() == first.Count();
    }
}

bool PartialIntersect(IEnumerable<int> first, IEnumerable<int> second)
{
    return first.Intersect(second).Any();
}