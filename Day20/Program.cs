using System.Collections.Generic;

var lines = File.ReadAllLines(".\\Input.txt");

//SolvePart1(lines);
SolvePart2(lines);

void SolvePart1(string[] lines)
{
    var initialList = lines
        .Select(x => int.Parse(x))
        .ToList();

    var circularList = new CircularList();

    for (int i = 0; i < initialList.Count; i++)
    {
        circularList.Add(new() { Index = i, Value = initialList[i] });
    }

    for (int i = 0; i < initialList.Count; i++)
    {
        circularList.Move(i, initialList[i] % (initialList.Count - 1));
    }

    var decoded = circularList.ToList();

    var zeroIndex = decoded.IndexOf(decoded.First(n => n.Value == 0));

    var r1 = decoded[(zeroIndex + 1000) % decoded.Count];
    var r2 = decoded[(zeroIndex + 2000) % decoded.Count];
    var r3 = decoded[(zeroIndex + 3000) % decoded.Count];

    Console.WriteLine($"Part 1 solution: {r1.Value + r2.Value + r3.Value}");
}

void SolvePart2(string[] lines)
{
    var initialList = lines
        .Select(x => long.Parse(x) * 811589153L)
        .ToList();

    var circularList = new CircularList();

    for (int i = 0; i < initialList.Count; i++)
    {
        circularList.Add(new() { Index = i, Value = initialList[i] });
    }

    for (int j = 0; j < 10; j++)
    {
        for (int i = 0; i < initialList.Count; i++)
        {
            circularList.Move(i, initialList[i] % (initialList.Count - 1));

            var test = circularList.ToList().Select(x => x.Value).ToList();
        }
    }

    var decoded = circularList.ToList();

    var zeroIndex = decoded.IndexOf(decoded.First(n => n.Value == 0));

    var r1 = decoded[(zeroIndex + 1000) % decoded.Count];
    var r2 = decoded[(zeroIndex + 2000) % decoded.Count];
    var r3 = decoded[(zeroIndex + 3000) % decoded.Count];

    Console.WriteLine($"Part 2 solution: {r1.Value + r2.Value + r3.Value}");
}

class CircularList
{
    public Node Head { get; set; }
    public Node Tail { get; set; }

    public void Add(Node node)
    {
        if (Head == default)
        {
            Head = node;
            Tail = node;
        }
        else
        {
            var newTail = node;
            newTail.Prev = Tail;
            newTail.Next = Head;

            Tail.Next = newTail;
            Head.Prev = newTail;

            Tail = newTail;
        }
    }

    public List<Node> ToList()
    {
        var list = new List<Node>();

        var current = Head;

        while (true)
        {
            list.Add(current);

            if (current.Next == Head)
            {
                break;
            }

            current = current.Next;
        }

        return list;
    }

    private Node Find(int index)
    {
        var current = Head;

        while (true)
        {
            if (current.Index == index)
            {
                break;
            }
            current = current.Next;
        }

        return current;
    }

    public void Move(int index, long move)
    {
        var moveRight = move > 0;
        var steps = Math.Abs(move);

        var current = Find(index);

        var prev = current.Prev;
        var next = current.Next;

        prev.Next = next;
        next.Prev = prev;

        var start = moveRight ? prev : next;

        for (int i = 0; i < steps; i++)
        {
            start = moveRight ? start.Next : start.Prev;
        }

        if (moveRight)
        {
            var cNext = start.Next;

            cNext.Prev = current;
            start.Next = current;

            current.Next = cNext;
            current.Prev = start;
        }
        else
        {
            var cPrev = start.Prev;

            cPrev.Next = current;
            start.Prev= current;

            current.Prev = cPrev;
            current.Next = start;
        }
    }
}

class Node
{
    public long Value { get; set; }
    public Node Prev { get; set; }
    public Node Next { get; set; }
    public int Index { get; set; }

}