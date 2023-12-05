namespace Day4;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Day4");

        var data = File.ReadAllLines(args[0]);

        Console.WriteLine($"Found {data.Count()} lines");

        var total = 0;
        foreach (var line in data)
        {
            var card = Card.From(line);
            total += card.CheckWinnings();
        }

        Console.WriteLine($"Part 1 Total {total}");

        // part 2
        Game.Part2(data);
    }
}
