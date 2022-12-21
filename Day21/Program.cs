var lines = File.ReadAllLines(".\\Input.txt");

//SolvePart1(lines);
SolvePart2(lines);

void SolvePart1(string[] lines)
{
	var monkeys = lines
		.Select(l => Monkey.FromLine(l))
		.ToList();

	var values = monkeys
		.Where(m => m.Value != 0)
		.ToDictionary(m => m.Name, m => m.Value);

	var toEvaluate = monkeys
		.Where(m => m.Value == 0)
		.ToList();

	foreach (var monkey in toEvaluate.ToList())
	{
		Evaluate(monkey, values, toEvaluate);
	}

	Console.WriteLine($"Part 1 solution: {values["root"]}");
}

void SolvePart2(string[] lines)
{
    var monkeys = lines
        .Select(l => Monkey.FromLine(l, true))
        .ToList();

    var values = monkeys
        .Where(m => m.Value != 0 || m.Name == "humn")
        .ToDictionary(m => m.Name, m => m.Value);

    var toEvaluate = monkeys
		.Where(m => m.Value == 0 && m.Name != "humn")
		.ToList();

	var human = monkeys.First(m => m.Name == "humn");
}


long Evaluate(Monkey monkey, Dictionary<string, long> values, List<Monkey> monkeys)
{
	if(values.TryGetValue(monkey.Name, out var val))
	{
		return val;
	}

	if(!values.TryGetValue(monkey.First, out var firstVal))
	{
        var firstMonkey = monkeys.First(m => m.Name == monkey.First);
        firstVal = Evaluate(firstMonkey, values, monkeys);

        values.TryAdd(monkey.First, firstVal);
    }

    if (!values.TryGetValue(monkey.Second, out var secondVal))
	{
        var secondMonkey = monkeys.First(m => m.Name == monkey.Second);
        secondVal = Evaluate(secondMonkey, values, monkeys);

        values.TryAdd(monkey.Second, secondVal);
    }
     
	monkeys.Remove(monkey);

	var monkeyValue = monkey.Operation((firstVal, secondVal));
	values.TryAdd(monkey.Name, monkeyValue);

    return monkeyValue;
}


record struct Monkey(string Name, long Value, string First, string Second, Func<(long, long), long> Operation)
{
	public static Monkey FromLine(string line, bool part2 = false)
	{
		var split = line.Split(":");
		var rightSplit = split[1].Split(" ");

		var name = split[0];

		long Value = 0;
		string first = "", second = "";

		Func<(long, long), long> operation = (_) => 0;

		if (rightSplit.Length == 2)
		{
			Value = (name == "humn" && part2) ? 0 : long.Parse(rightSplit[1]);
		}
		else
		{

			first = rightSplit[1];
			second = rightSplit[3];

            operation = ((long first, long second) input) =>
			{
				return rightSplit[2].Trim() switch
				{	"+" => input.first + input.second,
                    "-" => input.first - input.second,
                    "/" => input.first / input.second,
                    "*" => input.first * input.second,
                    _ => 0
				};
			};
		}

		return new(name, Value, first, second, operation);
	}
}