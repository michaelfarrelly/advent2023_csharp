namespace Day5;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Day5");

        var data = File.ReadAllLines(args[0]);

        Console.WriteLine($"Found {data.Count()} lines");

        Part1(data);

        Part2(data);
    }

    public static void Part1(string[] data)
    {
        var seedLine = data[0];
        var seedValues = seedLine
            .Replace("seeds: ", "")
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(s => long.Parse(s));

        var dataLines = data[2..];

        var sectionName = string.Empty;

        var sectionDatas = new List<SectionData>();
        SectionData? activeSectionData = null;

        foreach (var line in dataLines)
        {
            // determine if new section or not.
            if (ValidSectionName(line))
            {
                if (activeSectionData != null)
                {
                    // add to sectionDatas
                    sectionDatas.Add(activeSectionData);
                }
                sectionName = line.Split(":")[0].Trim();
                activeSectionData = new SectionData() { Name = sectionName };
            }
            else if (!string.IsNullOrWhiteSpace(line) && activeSectionData != null)
            {
                // collect values
                var parts = line.Split(" ");
                activeSectionData.Rows.Add(new SectionRow()
                {
                    DestRangeStart = long.Parse(parts[0]),
                    SourceRangeStart = long.Parse(parts[1]),
                    RangeLength = long.Parse(parts[2])
                });
            }
        }
        if (activeSectionData != null)
        {
            // add to sectionDatas
            sectionDatas.Add(activeSectionData);
        }


        var result = seedValues.Select(sv =>
        {
            // foreach (var seed in seedValues)
            // {
            var input = sv;
            long nextInput = sv;
            foreach (var sd in sectionDatas)
            {
                var m = sd.FindValue(nextInput);
                if (m.match)
                {
                    // Console.WriteLine($"Found in {sd.Name} for {input}: {nextInput}");
                    nextInput = m.value ?? -1;
                }

            }
            Console.WriteLine($"Found for {input}: {nextInput}");

            // }
            return (input, nextInput);
        }).OrderBy(item => item.nextInput);

        Console.WriteLine($"Lowest is {result.First().nextInput}");


        // part 2

    }

    public static void Part2(string[] data)
    {
        var seedLine = data[0];
        var seedValueInput = seedLine
            .Replace("seeds: ", "")
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(s => long.Parse(s))
            .ToArray();

        var seedValues = new List<(long start, long range)>();
        for (var s = 0; s < seedValueInput.Count(); s += 2)
        {
            seedValues.Add((seedValueInput[s], seedValueInput[s + 1]));
        }

        var dataLines = data[2..];

        var sectionName = string.Empty;

        var sectionDatas = new List<SectionData>();
        SectionData? activeSectionData = null;

        foreach (var line in dataLines)
        {
            // determine if new section or not.
            if (ValidSectionName(line))
            {
                if (activeSectionData != null)
                {
                    // add to sectionDatas
                    sectionDatas.Add(activeSectionData);
                }
                sectionName = line.Split(":")[0].Trim();
                activeSectionData = new SectionData() { Name = sectionName };
            }
            else if (!string.IsNullOrWhiteSpace(line) && activeSectionData != null)
            {
                // collect values
                var parts = line.Split(" ");
                activeSectionData.Rows.Add(new SectionRow()
                {
                    DestRangeStart = long.Parse(parts[0]),
                    SourceRangeStart = long.Parse(parts[1]),
                    RangeLength = long.Parse(parts[2])
                });
            }
        }
        if (activeSectionData != null)
        {
            // add to sectionDatas
            sectionDatas.Add(activeSectionData);
        }


        var result = seedValues.Select(sv =>
        {
            // foreach (var seed in seedValues)
            // {

            // foreach (var seedRange in Enumerable.Range(sv.start, sv.range))
            // {

            // var resultInner = new List<(long input, long nextInput)>();
            Console.WriteLine($"Part2: {sv.start}+{sv.range}: start");
            long? lowest = null;
            for (var s = sv.start; s < sv.start + sv.range; s += 1)
            {
                var input = s;
                long nextInput = s;
                foreach (var sd in sectionDatas)
                {
                    var m = sd.FindValue(nextInput);
                    if (m.match)
                    {
                        // Console.WriteLine($"Found in {sd.Name} for {input}: {nextInput}");
                        nextInput = m.value ?? -1;
                    }

                }
                // Console.WriteLine($"Found for {input}: {nextInput}");

                // }
                // return (input, nextInput);

                // resultInner.Add((input, nextInput));
                if (lowest == null)
                {
                    lowest = nextInput;
                }
                else if (nextInput < lowest)
                {
                    lowest = nextInput;
                }
            }
            // var lowestInner = resultInner.OrderBy(item => item.nextInput).First().nextInput;
            Console.WriteLine($"Part2: {sv.start}: Lowest ___ {lowest}");
            return lowest;
        }).OrderBy(item => item);

        Console.WriteLine($"Part2: Lowest is {result.First()}");


        // part 2

    }


    static bool ValidSectionName(string line)
    {
        return line.StartsWith("seed-to-soil map:")
            || line.StartsWith("soil-to-fertilizer map:")
            || line.StartsWith("fertilizer-to-water map:")
            || line.StartsWith("water-to-light map:")
            || line.StartsWith("light-to-temperature map:")
            || line.StartsWith("temperature-to-humidity map:")
            || line.StartsWith("humidity-to-location map:");
    }



    class SectionData
    {
        public SectionData()
        {
            this.Rows = new List<SectionRow>();
        }

        public (bool match, long? value) FindValue(long input)
        {
            foreach (var row in this.Rows)
            {
                var m = row.Matches(input);
                if (m.match)
                {
                    return (m.match, m.value);
                }
            }
            return (true, input);
        }

        public string Name;

        public List<SectionRow> Rows;
    }

    class SectionRow
    {
        public long DestRangeStart;
        public long SourceRangeStart;
        public long RangeLength;

        public (bool match, long? value) Matches(long input)
        {
            if (input >= SourceRangeStart && input <= SourceRangeStart + RangeLength)
            {
                // return the mapped Dest value.

                // input = 79
                // 52 50 48
                // output 79 - 50 + 52
                return (true, input - SourceRangeStart + DestRangeStart);
            }

            return (false, null);
        }
    }
}
