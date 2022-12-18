using System.Text.Json;

var lines = File.ReadAllLines(".\\Input.txt")
    .Where(l => !string.IsNullOrEmpty(l))
    .ToList();

var pairLines = lines
    .Chunk(2)
    .ToList();

var test1 = string.Compare("a", "b", true);
var test2 = string.Compare("b", "a", true);

SolvePart1(pairLines);
SolvePart2(lines);


void SolvePart1(List<string[]> pairLines)
{
    var sum = 0;

    foreach (var (pair, i) in pairLines.Select((p, i) => (p, i)))
    {
        var leftRoot = JsonDocument.Parse(pair[0]).RootElement;
        var rightRoot = JsonDocument.Parse(pair[1]).RootElement;

        var compare = CompareElements(leftRoot, rightRoot);
        if (compare!.Value)
        {
            sum += i + 1;
        }
    }
    Console.WriteLine($"Part 1 solution: {sum}");
}

void SolvePart2(List<string> lines)
{
    lines.Add("[[2]]");
    lines.Add("[[6]]");

    lines.Sort((left, right) =>
    {
        var leftRoot = JsonDocument.Parse(left).RootElement;
        var rightRoot = JsonDocument.Parse(right).RootElement;

        var compare = CompareElements(leftRoot, rightRoot);

        return compare!.Value ? -1 : 1;
    });

    var divider1 = lines.IndexOf("[[2]]") + 1;
    var divider2 = lines.IndexOf("[[6]]") + 1;

    Console.WriteLine($"Part 1 solution: {divider1 * divider2}");
}

bool? CompareElements(JsonElement left, JsonElement right)
{
    if (left.ValueKind == JsonValueKind.Number && right.ValueKind == JsonValueKind.Number)
    {
        if (left.GetInt32() < right.GetInt32())
        {
            return true;
        }
        else if (left.GetInt32() > right.GetInt32())
        {
            return false;
        }
        else
        {
            return null;
        }
    }
    if (left.ValueKind == JsonValueKind.Array || right.ValueKind == JsonValueKind.Array)
    {
        if (left.ValueKind == JsonValueKind.Number)
        {
            left = ToArrayElement(left);
        }
        if (right.ValueKind == JsonValueKind.Number)
        {
            right = ToArrayElement(right);
        }

        var leftArr = left.EnumerateArray();
        var rightArr = right.EnumerateArray();

        while (true)
        {
            var moveLeft = leftArr.MoveNext();
            var moveRight = rightArr.MoveNext();

            if (!moveLeft && !moveRight)
            {
                return null;
            }
            if (!moveLeft)
            {
                return true;
            }
            if (!moveRight)
            {
                return false;
            }

            var compare = CompareElements(leftArr.Current, rightArr.Current);
            if (compare is not null)
                return compare;
        }
    }

    return null;

    JsonElement ToArrayElement(JsonElement input)
    {
        var arr = new int[] { input.GetInt32() };
        var json = JsonSerializer.Serialize(arr);

        var root = JsonDocument.Parse(json).RootElement;
        return root;
    }
}

