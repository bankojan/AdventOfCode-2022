using System.Collections.Generic;

var lines = File.ReadAllLines(".\\Input.txt");

SolvePart1(lines);
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
        .Select(l => Monkey.FromLine(l))
        .ToList();

    var values = monkeys
        .Where(m => m.Value != 0)
        .ToDictionary(m => m.Name, m => m.Value);

    var root = monkeys.First(m => m.Name == "root");
	var leftMonkey = monkeys.First(m => m.Name == root.First);
	var rightMonkey = monkeys.First(m => m.Name == root.Second);

    var left = ContainsHuman(leftMonkey, monkeys);
    var right = ContainsHuman(rightMonkey, monkeys);

	long leftValue = 0;

	if (!left)
	{
		leftValue = Evaluate(leftMonkey, values, monkeys);

		values.TryAdd(rightMonkey.Name, leftValue);
	}
	else
	{
		leftValue = Evaluate(rightMonkey, values, monkeys);

        values.TryAdd(leftMonkey.Name, leftValue);
    }

	foreach (var monkey in monkeys.Where(m => m.Name != "humn").ToList())
	{
		if(!ContainsHuman(monkey, monkeys))
		{
			Evaluate(monkey, values, monkeys);
		}
	}

	var human = monkeys.First(m => m.Name == "humn");

	var humanValue = ReverseEvaluate(human, values, monkeys);

	Console.WriteLine($"Part 2 solution: {humanValue}");
}

long ReverseEvaluate(Monkey monkey, Dictionary<string, long> values, List<Monkey> monkeys)
{
    if (monkey.Name != "humn"&& values.TryGetValue(monkey.Name, out var val))
    {
        return val;
    }

	var parent = monkeys
		.First(m => m.First == monkey.Name || m.Second == monkey.Name);

	long firstVal;
	bool reverse = false;

	if(parent.First == monkey.Name)
	{
        if (!values.TryGetValue(parent.Second, out firstVal))
        {
            var firstMonkey = monkeys.First(m => m.Name == parent.Second);
            firstVal = Evaluate(firstMonkey, values, monkeys);

            values.TryAdd(parent.Second, firstVal);
        }
    }
	else
	{
        if (!values.TryGetValue(parent.First, out firstVal))
        {
            var firstMonkey = monkeys.First(m => m.Name == parent.First);
            firstVal = Evaluate(firstMonkey, values, monkeys);

            values.TryAdd(parent.First, firstVal);
        }
        reverse = true;
    }

    if (!values.TryGetValue(parent.Name, out var secondVal))
	{
		secondVal = ReverseEvaluate(parent, values, monkeys);
	}

    var monkeyValue = parent.ReverseOperation((secondVal, firstVal, reverse));
    values.TryAdd(monkey.Name, monkeyValue);

    return monkeyValue;
}

bool ContainsHuman(Monkey monkey, List<Monkey> monkeys)
{
	if(monkey.Name == "humn")
	{
		return true;
	}
	if(monkey.Value != 0)
	{
		return false;
	}	

	return ContainsHuman(monkeys.First(m => m.Name == monkey.First), monkeys) ||
		ContainsHuman(monkeys.First(m => m.Name == monkey.Second), monkeys);
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
     
	var monkeyValue = monkey.Operation((firstVal, secondVal));
	values.TryAdd(monkey.Name, monkeyValue);

    return monkeyValue;
}

record struct Monkey(string Name, long Value, string First, string Second, Func<(long, long), long> Operation, Func<(long, long, bool), long> ReverseOperation)
{
	public static Monkey FromLine(string line)
	{
		var split = line.Split(":");
		var rightSplit = split[1].Split(" ");

		var name = split[0];

		long Value = 0;
		string first = "", second = "";

		Func<(long, long), long> operation = (_) => 0;
        Func<(long, long, bool), long> reverseOperation = (_) => 0;

        if (rightSplit.Length == 2)
		{
			Value = long.Parse(rightSplit[1]);
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
            reverseOperation = ((long first, long second, bool reverse) input) =>
            {
                return (rightSplit[2].Trim(), input.reverse) switch
                {
                    ("+", _) => input.first - input.second,
                    ("-", false) => input.first + input.second,
                    ("-", true) => input.second - input.first,
                    ("/", false) => input.first * input.second,
                    ("/", true) => input.second / input.first,
                    ("*", _) => input.first / input.second,
                    _ => 0
                };
            };
        }

		return new(name, Value, first, second, operation, reverseOperation);
	}
}