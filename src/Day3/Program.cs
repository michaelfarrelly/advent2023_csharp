namespace Day3;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Day3");

        var data = File.ReadAllLines(args[0]);

        Console.WriteLine($"Found {data.Count()} lines");

        // build engine/part model

        var engine = new Engine(data);
        Console.WriteLine($"Found {engine.parts.Count()}");

        foreach (var p in engine.parts)
        {
            Console.WriteLine($"Found {p.number} {p.x} {p.y}");
        }

        Console.WriteLine("Part1: ValidParts");
        var validParts = engine.ValidParts();
        Console.WriteLine($"Found {validParts.Count()}");
        Console.WriteLine($"Part1: Sum {validParts.Sum((v) => int.Parse(v.number))}");

        var validGears = engine.ValidGears();

        Console.WriteLine($"Part2: Sum {validGears}");
        // Console.WriteLine($"Part2: Sum {validGears.Sum((v) => int.Parse(v.number))}");
    }
}
