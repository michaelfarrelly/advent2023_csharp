using System.Diagnostics;
using System.Timers;

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
                activeSectionData.Rows.Add(new SectionRow(
                    long.Parse(parts[0]),
                    long.Parse(parts[1]),
                    long.Parse(parts[2])
                ));
            }
        }
        if (activeSectionData != null)
        {
            // add to sectionDatas
            sectionDatas.Add(activeSectionData);
        }

        var lockedSD = sectionDatas.ToArray();


        var result = seedValues.Select(sv =>
        {
            // foreach (var seed in seedValues)
            // {
            var input = sv;
            // long nextInput = sv;
            // foreach (var sd in sectionDatas)
            // {
            //     var m = sd.FindValue(nextInput);
            //     if (m.match)
            //     {
            //         // Console.WriteLine($"Found in {sd.Name} for {input}: {nextInput}");
            //         nextInput = m.value ?? -1;
            //     }

            // }
            var nextInput = ProcessInputs(sv, lockedSD);
            Console.WriteLine($"Found for {input}: {nextInput}");

            // }
            return (input, nextInput);
        }).OrderBy(item => item.nextInput);

        Console.WriteLine($"Lowest is {result.First().nextInput}");


        // part 2

    }

    public static void Part2(string[] data)
    {
        Console.WriteLine($"Part2=======================");
        var seedLine = data[0];
        var seedValueInput = seedLine
            .Replace("seeds: ", "")
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(s => long.Parse(s))
            .ToArray();

        var seedValues = new List<(long start, long range)>();
        for (var s = 0; s < seedValueInput.Length; s += 2)
        {
            seedValues.Add((seedValueInput[s], seedValueInput[s + 1]));
        }

        var dataLines = data[2..];

        var sectionName = string.Empty;

        var sectionDatas = new List<SectionData>();
        SectionData? activeSectionData = null;


        Console.WriteLine($"Part2: models");

        foreach (var line in dataLines)
        {
            // determine if new section or not.
            if (ValidSectionName(line))
            {
                if (activeSectionData != null)
                {
                    // add to sectionDatas
                    sectionDatas.Add(activeSectionData);
                    Console.WriteLine($"Part2: models '{activeSectionData.Name}' complete");
                }
                sectionName = line.Split(":")[0].Trim();
                var nextSection = new SectionData() { Name = sectionName };
                if (activeSectionData != null)
                {
                    activeSectionData.Child = nextSection;
                }
                activeSectionData = nextSection;
            }
            else if (!string.IsNullOrWhiteSpace(line) && activeSectionData != null)
            {
                // collect values
                var parts = line.Split(" ");
                activeSectionData.Rows.Add(new SectionRow(
                    long.Parse(parts[0]),
                    long.Parse(parts[1]),
                    long.Parse(parts[2])
                ));
            }
        }

        // add last.
        if (activeSectionData != null)
        {
            // add to sectionDatas
            sectionDatas.Add(activeSectionData);
            Console.WriteLine($"Part2: models '{activeSectionData.Name}' complete");
        }

        var lockedSD = sectionDatas.ToArray();

        Console.WriteLine($"Part2: model complete");

        var maxElements = seedValues.Count;
        long? lowest = null;
        for (var e = 0; e < maxElements; e += 1)
        {
            var sv = seedValues[e];
            (long start, long end) range = (sv.start, sv.start + sv.range);
            Console.WriteLine($"Part2: {range.start}->{range.end}: start");

            Stopwatch watch = new();
            watch.Start();

            var nextInput = ProcessInputs2(range.start, range.end, lockedSD);
            if (lowest == null || nextInput < lowest)
            {
                lowest = nextInput;
                // Console.WriteLine($"Part2: {s}: Lowest _n_ {lowest}");
            }


            // for (var s = range.start; s < range.end; s++)
            // {
            // Stopwatch watch3 = new();
            // watch3.Start();

            // var nextInput = ProcessInputs2(s, lockedSD);

            // watch3.Stop();
            // if (s % 1000000 == 0)
            // {
            //     // percent
            //     var percent1 = ((double)s - (double)range.start) / ((double)range.end - (double)range.start);
            //     Console.WriteLine($"Yikes {percent1} {watch3.Elapsed.TotalSeconds}s of {watch.Elapsed.TotalSeconds}s ");
            // }

            // if (lowest == null || nextInput < lowest)
            // {
            //     lowest = nextInput;
            //     // Console.WriteLine($"Part2: {s}: Lowest _n_ {lowest}");
            // }

            // if (watch.Elapsed.TotalSeconds % 10 == 0)
            // {
            //     Console.WriteLine($"Yikes {watch.Elapsed}");
            // }
            // }
            // watch.Stop();
            // Console.WriteLine($"Yikes {watch.Elapsed.TotalSeconds}s");
            // Console.WriteLine($"Part2: {sv.start}: Lowest ___ {lowest}");
        }

        Console.WriteLine($"Part2: Lowest is {lowest}");
    }

    static long ProcessInputs(long input, SectionData[] sectionDatas)
    {
        long nextInput = input;
        return sectionDatas[0].FindDeepValue(nextInput).value ?? -1;

    }

    static long ProcessInputs2(long input, long inputEnd, SectionData[] sectionDatas)
    {
        long nextInput = input;
        return sectionDatas[0].FindDeepValue(input, inputEnd).value ?? -1;

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
            // if the input matches to a Row, use it, otherwise return the same input as the output.
            foreach (var row in this.Rows)
            {
                var m = row.Matches(input);
                if (m.match)
                {
                    return (m.match, m.value);
                }
            }
            // no re-maps, so it is the original value
            return (true, input);
        }

        public (bool match, long? value) FindDeepValue(long input)
        {
            if (this.LockedRows == null)
            {
                this.LockedRows = this.Rows.ToArray();
                // find min / max uncovered values.
                this.MinValue = this.Rows.OrderBy(r => r.SourceRangeStart).First().SourceRangeStart;
                this.MaxValue = this.Rows.OrderBy(r => r.SourceRangeEnd).Last().SourceRangeEnd;
            }


            if (input < MinValue || input > MaxValue)
            {
                if (Child != null)
                {
                    return Child.FindDeepValue(input);
                }
                else
                {
                    return (true, input);
                }
            }

            // if the input matches to a Row, use it, otherwise return the same input as the output.
            foreach (var row in this.LockedRows)
            {
                var m = row.Matches(input);
                if (m.match)
                {
                    if (Child != null)
                    {
                        return Child.FindDeepValue(m.value ?? -1);
                    }
                    else
                    {
                        return (m.match, m.value);
                    }
                }
            }
            // no re-maps, so it is the original value
            // return (true, input);

            if (Child != null)
            {
                return Child.FindDeepValue(input);
            }
            else
            {
                return (true, input);
            }
        }

        public (bool match, long? value) FindDeepValue(long input, long inputEnd)
        {
            if (this.LockedRows == null)
            {
                this.LockedRows = this.Rows.ToArray();
                // find min / max uncovered values.
                this.MinValue = this.Rows.OrderBy(r => r.SourceRangeStart).First().SourceRangeStart;
                this.MaxValue = this.Rows.OrderBy(r => r.SourceRangeEnd).Last().SourceRangeEnd;
            }


            if (input < MinValue || input > MaxValue)
            {
                if (Child != null)
                {
                    return Child.FindDeepValue(input);
                }
                else
                {
                    return (true, input);
                }
            }

            // if the input matches to a Row, use it, otherwise return the same input as the output.
            foreach (var row in this.LockedRows)
            {
                var m = row.Matches(input);
                if (m.match)
                {
                    if (Child != null)
                    {
                        return Child.FindDeepValue(m.value ?? -1);
                    }
                    else
                    {
                        return (m.match, m.value);
                    }
                }
            }
            // no re-maps, so it is the original value
            // return (true, input);

            if (Child != null)
            {
                return Child.FindDeepValue(input);
            }
            else
            {
                return (true, input);
            }
        }

        public string Name;

        public List<SectionRow> Rows;

        private SectionRow[] LockedRows;

        public SectionData? Child;
        private long MaxValue;

        private long MinValue;
    }

    class SectionRow
    {
        public SectionRow(long DestRangeStart, long SourceRangeStart, long RangeLength)
        {
            this.DestRangeStart = DestRangeStart;
            this.SourceRangeStart = SourceRangeStart;
            this.RangeLength = RangeLength;
            this.DestRangeEnd = DestRangeStart + RangeLength;
            this.SourceRangeEnd = SourceRangeStart + RangeLength;
            this.Offset = -SourceRangeStart + DestRangeStart;
        }
        public long DestRangeStart;
        public long SourceRangeStart;
        public long DestRangeEnd;
        public long SourceRangeEnd;
        public long RangeLength;
        public long Offset;



        public (bool match, long? value) Matches(long input)
        {
            if (input >= SourceRangeStart && input <= SourceRangeEnd)
            {
                // return the mapped Dest value.

                // input = 79
                // 52 50 48
                // output 79 - 50 + 52
                return (true, input + Offset);
            }

            return (false, null);
        }
    }
}
