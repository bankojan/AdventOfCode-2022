var lines = File.ReadAllLines(".\\Input.txt");

//SolvePart1(lines, 2000000);
SolvePart2(lines, 4000000L);

void SolvePart1(string[] lines, int row)
{
	var covered = new HashSet<long>();

    foreach (var line in lines)
	{
		var s1 = line.Split(":");

		var l = s1[0].Split(",");
		var r = s1[1].Split(",");

		var sensorY = long.Parse(l[1].Split('=')[1]);
		var sensorX = long.Parse(l[0].Split('=')[1]);

        var beaconY = long.Parse(r[1].Split('=')[1]);
        var beaconX = long.Parse(r[0].Split('=')[1]);

		var sensor = new Point(sensorX, sensorY);
		var beacon = new Point(beaconX, beaconY);

		var distance = sensor.ManhattanDistance(beacon);

		var vertDist = sensor.ManhattanDistance(new(sensor.X, row));

		var actualDistance = distance - vertDist;

		if(actualDistance > 0)
		{
            for (long i = sensor.X - actualDistance; i <= sensor.X + actualDistance; i++)
			{
				if (!beacon.Equals(new(i, row)))
				{
					covered.Add(i);
				}
			}
        }
    }

	Console.WriteLine($"Part 1 solution: {covered.Count}");
}

void SolvePart2(string[] lines, long searchSpace)
{
    var pairs = new List<Pair>();

    foreach (var line in lines)
    {
        var s1 = line.Split(":");

        var l = s1[0].Split(",");
        var r = s1[1].Split(",");

        var sensorY = long.Parse(l[1].Split('=')[1]);
        var sensorX = long.Parse(l[0].Split('=')[1]);

        var beaconY = long.Parse(r[1].Split('=')[1]);
        var beaconX = long.Parse(r[0].Split('=')[1]);

        var sensor = new Point(sensorX, sensorY);
        var beacon = new Point(beaconX, beaconY);

        pairs.Add(new Pair(sensor, beacon, sensor.ManhattanDistance(beacon)));
    }

    var possiblePoints = new HashSet<Point>();

    foreach (var pair in pairs)
    {
        for (long i= pair.Sensor.Y - pair.ManhattanDistance; i <= pair.Sensor.Y + pair.ManhattanDistance; i++)
        {
            var vertDist = Math.Abs(pair.Sensor.Y - i);

            var actualDistance = pair.ManhattanDistance - vertDist;

            if(actualDistance > 0)
            {
                possiblePoints.Add(new(pair.Sensor.X + actualDistance + 1, pair.Sensor.Y - vertDist));
                possiblePoints.Add(new(pair.Sensor.X - actualDistance - 1, pair.Sensor.Y - vertDist));
                
            }
        }

        possiblePoints.Add(new Point(pair.Sensor.X, pair.Sensor.Y + pair.ManhattanDistance + 1));
        possiblePoints.Add(new Point(pair.Sensor.X, pair.Sensor.Y - pair.ManhattanDistance - 1));
    }

    possiblePoints = possiblePoints
        .Where(p => p.X >= 0 && p.X <= searchSpace && p.Y >= 0 && p.Y <= searchSpace)
        .ToHashSet();

    Parallel.ForEach(possiblePoints, (point) =>
    {
        var ok = true;
        foreach (var pair in pairs)
        {
            if (pair.Sensor.ManhattanDistance(point) <= pair.ManhattanDistance)
            {
                ok = false;
                break;
            }
        }

        if (ok)
        {
            var tuningFreq = point.X * 4000000L + point.Y;

            Console.WriteLine($"Part 2 solution {tuningFreq}");
            return;
        }
    });
}

record struct Pair(Point Sensor, Point Beacon, long ManhattanDistance);

record struct Point(long X, long Y)
{
	public long ManhattanDistance(Point p2)
	{
		return Math.Abs(X - p2.X) + Math.Abs(Y - p2.Y);
	}
}

