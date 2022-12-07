var lines = File.ReadAllLines(".\\Input.txt");

var root = ExecuteCommands(lines);

Console.WriteLine($"Part 1 solution: {root.SumOfDirectoriesBelowOrAtTreshold(100000)}");
Console.WriteLine($"Part 2 solution: {root.FindSmallestAboveTreshold(30000000 - 70000000 + root.TotalSize)} ");

FileDirectory ExecuteCommands(string[] lines)
{
    var root = new FileDirectory
    {
        Name = "/",
        Parent = null,
        Size = 0
    };

    var currentDirectory = root;

    foreach (var command in lines[2..])
    {
        var commandSplit = command.Split(' ');

        var c = commandSplit[0];

        if (long.TryParse(commandSplit[0], out var size))
        {
            currentDirectory!.Size += size;
        }
        else if (commandSplit[0] == "$")
        {
            if (commandSplit[1] == "cd")
            {
                if (commandSplit[2] == "..")
                {
                    currentDirectory = currentDirectory!.Parent;
                }
                else
                {
                    currentDirectory = currentDirectory!.SubDirectories.First(sd => sd.Name == commandSplit[2]);
                }    
            }
        }
        else if (commandSplit[0] == "dir")
        {
            currentDirectory!.SubDirectories.Add(new()
            {
                Name = commandSplit[1],
                Parent = currentDirectory
            });
        }
    }

    return root;
}

class FileDirectory
{
    public string? Name { get; set; }
    public long Size { get; set; } = 0;
    public FileDirectory? Parent { get; set; }
    public List<FileDirectory> SubDirectories { get; set; } = new();

    public long TotalSize
        => Size + SubDirectories.Select(s => s.TotalSize).Sum();

    public long SumOfDirectoriesBelowOrAtTreshold(long treshold)
    {
        return TotalSize <= treshold ?
            TotalSize + SubDirectories.Select(sd => sd.SumOfDirectoriesBelowOrAtTreshold(treshold)).Sum() :
            SubDirectories.Select(sd => sd.SumOfDirectoriesBelowOrAtTreshold(treshold)).Sum();
    }

    public long FindSmallestAboveTreshold(long treshold)
    {
        var possibleSubDirectories = SubDirectories
            .Where(sd => sd.TotalSize >= treshold);

        return !possibleSubDirectories.Any() ?
            TotalSize :
            possibleSubDirectories.Min(sd => sd.FindSmallestAboveTreshold(treshold));
    }
}