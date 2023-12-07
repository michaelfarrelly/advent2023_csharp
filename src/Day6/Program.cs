namespace Day6;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Day6");

        var data = File.ReadAllLines(args[0]);

        Console.WriteLine($"Found {data.Count()} lines");

        Part1(data);

        Part2(data);
    }

    static void Part1(string[] data)
    {
        // parse data.
        // Time:      7  15   30
        // Distance:  9  40  200
        var timeData = data[0]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(int.Parse)
            .ToArray();
        var distanceData = data[1]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(int.Parse)
            .ToArray();

        var races = Enumerable.Range(0, timeData.Count())
            .Select(raceId =>
            {

                return new
                {
                    RaceId = raceId,
                    TotalTime = timeData[raceId],
                    TotalDistance = distanceData[raceId]
                };
            })
            .ToArray();

        var computeTotal = 1;
        foreach (var race in races)
        {
            // find all winnable races
            // win is > race.TotalDistance
            var speed = 0;
            var winList = 0;

            for (var tick = speed; tick <= race.TotalTime; tick++)
            {
                // try at speed:0, no increase in speed.
                // each increase in speed will "waste" a tick.

                // tick= 0, no speed, no move.
                // tick=1, speed=1, moveTicks = 6
                var moveTicks = race.TotalTime - tick;
                var totalDistance = moveTicks * tick;
                if (totalDistance > race.TotalDistance)
                {
                    // add to win list.
                    winList++;
                }
            }

            computeTotal *= winList;
        }

        Console.WriteLine($"Part1:ComputeTotal {computeTotal}");
    }

    static void Part2(string[] data)
    {
        // parse data.
        // Time:      7  15   30
        // Distance:  9  40  200
        var timeData = data[0]
            .Split(":", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(p => p.Replace(" ", "").Trim())
            .Select(long.Parse)
            .ToArray();
        var distanceData = data[1]
            .Split(":", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(p => p.Replace(" ", "").Trim())
            .Select(long.Parse)
            .ToArray();

        var races = Enumerable.Range(0, timeData.Count())
            .Select(raceId =>
            {

                return new
                {
                    RaceId = raceId,
                    TotalTime = timeData[raceId],
                    TotalDistance = distanceData[raceId]
                };
            })
            .ToArray();

        var computeTotal = 1;
        foreach (var race in races)
        {
            // find all winnable races
            // win is > race.TotalDistance
            var speed = 0;
            var winList = 0;

            Console.WriteLine($"Part1:Race {race.TotalTime}, {race.TotalDistance}");

            for (var tick = speed; tick <= race.TotalTime; tick++)
            {
                // try at speed:0, no increase in speed.
                // each increase in speed will "waste" a tick.

                // tick= 0, no speed, no move.
                // tick=1, speed=1, moveTicks = 6
                var moveTicks = race.TotalTime - tick;
                var totalDistance = moveTicks * tick;
                if (totalDistance > race.TotalDistance)
                {
                    // add to win list.
                    winList++;
                }
            }

            computeTotal *= winList;
        }

        Console.WriteLine($"Part1:ComputeTotal {computeTotal}");
    }
}
