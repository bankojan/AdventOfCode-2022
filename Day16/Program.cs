using System.Diagnostics;

var lines = File.ReadAllLines(".\\Input.txt");

Dictionary<string, int> subSolutions = new Dictionary<string, int>();
var hits = 0;

var sw = new Stopwatch();
sw.Start();

//SolvePart1(lines);
SolvePart2(lines);

sw.Stop();
Console.WriteLine($"{sw.ElapsedMilliseconds} ms ({hits} hits)");

List<Valve> ParseLines(string[] lines)
{
    var valves = lines
        .Select(l =>
        {
            var split = l.Split(" has");
            var name = split[0].Split(" ").Last();

            var flowRate = int.Parse(split[1].Split(";").First().Split("=").Last());

            return new Valve(name, flowRate);
        })
        .ToList();

    for (int i = 0; i < valves.Count; i++)
    {
        var connectedValves = lines[i].Split("to valve")
            .Last()[1..]
            .Trim()
            .Split(", ")
            .Select(vs => valves.First(v => v.Name == vs));

        valves[i].ConnectedValves = connectedValves.ToList();
    }

    return valves;
}

void SolvePart1(string[] lines)
{
    var valves = ParseLines(lines);

    var result = FindPath(valves.First(v => v.Name == "AA"), 30, new());

    Console.WriteLine($"Part 1 solution: {result}");
}

void SolvePart2(string[] lines)
{
    var valves = ParseLines(lines);

    var start = valves.First(v => v.Name == "AA");

    var result = FindPathWithHelp(start, start, 26, new());

    Console.WriteLine($"Part 2 solution: {result}");
}

int FindPath(Valve current, int minute, HashSet<Valve> openValves)
{
    if (minute == 0)
    {
        return 0;
    }

    var k1 = $"{string.Join(',', openValves.Select(v => v.Name).OrderBy(n => n))}";
    var k2 = current.Name;
    var k3 = $"{minute}";

    var dictKey = $"{k1}-{k2}-{k3}";

    if (subSolutions.TryGetValue(dictKey, out int value))
    {
        hits++;
        return value;
    }

    var maxValue = 0;
    if(current.FlowRate > 0 && !openValves.Contains(current))
    {
        var openValvesNew = new HashSet<Valve>(openValves)
        {
            current
        };
        maxValue = FindPath(current, minute - 1, openValvesNew);
    }

    foreach (var valve in current.ConnectedValves)
    {
        maxValue = int.Max(maxValue, FindPath(valve, minute - 1, openValves));
    }

    maxValue += openValves.Sum(v => v.FlowRate);

    subSolutions.TryAdd(dictKey, maxValue);

    return maxValue;
}

int FindPathWithHelp(Valve current, Valve elephant, int minute, HashSet<Valve> openValves)
{
    if (minute == 0)
    {
        return 0;
    }

    var k1 = $"{string.Join(',', openValves.Select(v => v.Name).OrderBy(n => n))}";
    var k2 = current.Name;
    var k22 = elephant.Name;
    var k3 = $"{minute}";

    var dictKey = $"{k1}-{k2}-{k22}-{k3}";

    if (subSolutions.TryGetValue(dictKey, out int value))
    {
        hits++;
        return value;
    }

    var possibleValvesCurrent = new HashSet<Valve>(current.ConnectedValves);
    var possibleValvesElephant = new HashSet<Valve>(elephant.ConnectedValves);

    if(current.FlowRate > 0 && !openValves.Contains(current))
    {
        possibleValvesCurrent.Add(current);
    }
    if (elephant.FlowRate > 0 && !openValves.Contains(elephant))
    {
        possibleValvesElephant.Add(elephant);
    }

    var maxValue = 0;

    foreach (var valveCurrent in possibleValvesCurrent)
    {
        foreach (var valveElephant in possibleValvesElephant)
        {
            if (valveCurrent.Equals(valveElephant))
            {
                continue;
            }

            var openValvesNew = new HashSet<Valve>(openValves);
            if (valveCurrent.Equals(current))
            {
                openValvesNew.Add(current);
            }
            if (valveElephant.Equals(elephant))
            {
                openValvesNew.Add(elephant);
            }

            maxValue = int.Max(maxValue, FindPathWithHelp(valveCurrent, valveElephant, minute - 1, openValvesNew));
        }
    }

    maxValue += openValves.Sum(v => v.FlowRate);

    subSolutions.TryAdd(dictKey, maxValue);

    return maxValue;
}

record Valve(string Name, int FlowRate)
{
    public List<Valve> ConnectedValves { get; set; } = new();

    public int FindPath(int totalFlow, int minute, HashSet<Valve> openValves)
    {
        if(minute == 0)
        {
            return totalFlow;
        }

        //var dictKey = $"{string.Join(',', openValves.Select(v => v.Name).OrderBy(n => n))}-{Name}-{minute}";

        //if(subSolutions.TryGetValue(out var val))
        //{

        //}

        var addFlowRate = openValves.Sum(v => v.FlowRate);

        var openValvesNew = new HashSet<Valve>(openValves)
        {
            this
        };
        var maxValue = FindPath(totalFlow + addFlowRate, minute - 1, openValvesNew);

        foreach (var valve in ConnectedValves)
        {
            maxValue = int.Max(maxValue, valve.FindPath(totalFlow + addFlowRate, minute - 1, openValves));
        }

        return maxValue;
    }
}


