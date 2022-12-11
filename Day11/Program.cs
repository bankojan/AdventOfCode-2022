var lines = File.ReadAllLines(".\\Input.txt");

SolvePart1(lines);
SolvePart2(lines);

void SolvePart1(string[] lines)
{
    var monkeys = lines
        .Chunk(7)
        .Select(Monkey.FromLines)
        .ToList();

    for (int round = 0; round < 20; round++)
    {
        foreach (var monkey in monkeys)
        {
            while(monkey.Items.TryDequeue(out var item))
            {
                var newWorryLevel = monkey.Operation((item, true, 1));
                
                var newMonkey = monkey.Test(newWorryLevel);
                monkey.NumberOfInspections++;

                monkeys[newMonkey].Items.Enqueue(newWorryLevel);
            }
        }
    }

    var topMonkeys = monkeys.OrderByDescending(m => m.NumberOfInspections).ToList();

    var monkeyBusiness = topMonkeys[0].NumberOfInspections * 
        topMonkeys[1].NumberOfInspections;

    Console.WriteLine($"Part 1 solution: {monkeyBusiness}");
}

void SolvePart2(string[] lines)
{
    var monkeys = lines
        .Chunk(7)
        .Select(Monkey.FromLines)
        .ToList();

    var lcd = monkeys[0].Divisor;
    foreach (var monkey in monkeys.Skip(1))
    {
        lcd *= monkey.Divisor;
    }

    for (int round = 0; round < 10_000; round++)
    {
        foreach (var monkey in monkeys)
        {
            while (monkey.Items.TryDequeue(out var item))
            {
                var newWorryLevel = monkey.Operation((item, false, lcd));

                var newMonkey = monkey.Test(newWorryLevel);
                monkey.NumberOfInspections++;

                monkeys[newMonkey].Items.Enqueue(newWorryLevel);
            }
        }
    }

    var topMonkeys = monkeys.OrderByDescending(m => m.NumberOfInspections).ToList();

    var monkeyBusiness = topMonkeys[0].NumberOfInspections *
        topMonkeys[1].NumberOfInspections;

    Console.WriteLine($"Part 2 solution: {monkeyBusiness}");
}

class Monkey
{
    public Queue<ulong> Items { get; set; }
    public Func<(ulong, bool, ulong), ulong> Operation { get; set; }
    public Func<ulong, int> Test { get; set; }

    public ulong Divisor { get; set; }

    public ulong NumberOfInspections { get; set; }

    public static Monkey FromLines(string[] lines)
    {
        var items = lines[1]
            .Split(':')[1]
            .Split(",")
            .Select(n => ulong.Parse(n));

        var operationSplit = lines[2]
            .Split('=')[1]
            .Split(' ')[1..];

        var divisor = ulong.Parse(lines[3].Split(" ").Last());

        var operation = ((ulong input, bool divide, ulong lcd) input) =>
        {            
            if(!ulong.TryParse(operationSplit.Last(), out var secondOperand))
            {
                secondOperand = input.input;
            }

            var op = operationSplit[1];

            var result = op switch
            {
                "*" => input.input * secondOperand,
                "+" => input.input + secondOperand,
                "-" => input.input - secondOperand,
                "/" => input.input / secondOperand,
                _ => throw new Exception()
            };

            if(input.divide)
                return result / 3;

            result %= input.lcd;

            return result;
        };

        var Test = (ulong input) =>
        {
            var firstMonkey = int.Parse(lines[4].Split(" ").Last());
            var secondMonkey = int.Parse(lines[5].Split(" ").Last());

            return input % divisor == 0 ? 
                firstMonkey : 
                secondMonkey;
        };

        return new()
        {
            Items = new Queue<ulong>(items),
            Operation = operation,
            Test = Test,
            NumberOfInspections = 0,
            Divisor = divisor
        };
    }
}
