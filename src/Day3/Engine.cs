
using System.Runtime.InteropServices;

namespace Day3;

class Engine
{
    private int maxY;
    private int maxX;
    private string[] data;

    public Engine(string[] data)
    {

        this.maxY = data.Length;
        this.maxX = data[0].Length;
        this.data = data;

        var data1 = this.generateParts(data);
        this.engineSymbols = data1.symbols;
        this.parts = data1.parts;
    }

    public List<EnginePart> parts = [];

    public List<EngineSymbol> engineSymbols = [];

    private (List<EnginePart> parts, List<EngineSymbol> symbols) generateParts(string[] data)
    {
        var parts = new List<EnginePart>();
        var symbols = new List<EngineSymbol>();
        // foreach (var line in data)
        // {
        for (var i = 0; i < data.Length; i++)
        {
            var line = data[i];
            for (var j = 0; j < line.Length; j++)
            {
                var char1 = line[j];
                if (isNumChar(char1))
                {
                    // now check the next chars until not a number.
                    var nextPartNumber = line
                        .Skip(j)
                        .TakeWhile((c, n) => isNumChar(c))
                        .Aggregate("", (p, n) => $"{p}{n}");
                    var part = new EnginePart(nextPartNumber, i, j);
                    parts.Add(part);
                    j += nextPartNumber.Length - 1;
                }

                if (isSymbol(char1))
                {
                    var symbol = new EngineSymbol(char1.ToString(), i, j);
                    symbols.Add(symbol);
                }
            }
        }
        return (parts, symbols);
    }

    public List<EnginePart> ValidParts()
    {
        // after generate, run validParts
        var validParts = this.parts.Where((part) =>
        {
            // check that its near a symbol.

            // $$$$$
            // $111$
            // $$$$$



            return this.isNear(part, this.engineSymbols);
        }).ToList();

        return validParts;
    }


    internal int ValidGears()
    {
        // after generate, run validParts
        var gears = this.engineSymbols.Where(s => s.number == "*").ToList();
        var validPartsAroundGears = this.parts.Where((part) =>
        {
            return this.isNear(part, gears);
        }).ToList();

        var partGearDetails = validPartsAroundGears.Select(part =>
        {
            return this.isNearDetails(part, gears);
        });

        var validGears = gears.Select((g) =>
        {
            var nearParts = partGearDetails.Where(p =>
            {
                return p.matchedSymbols.Contains(g);
            });
            if (nearParts.Count() == 2)
            {
                var partNumbs = nearParts.Select(p => int.Parse(p.part.number));

                var total = 1;
                foreach (var pn in partNumbs)
                {
                    total *= pn;
                }
                return total;
            }
            return 0;
        }).Sum();

        return validGears;
    }

    private bool isNumChar(char char1)
    {
        return char1 >= '0' && char1 <= '9';
    }

    private string symbols = "~!@#$%^&*()_+=-";

    private bool isSymbol(char char1)
    {
        return !isNumChar(char1) && char1 != '.';
    }

    private bool isNear(EnginePart part, List<EngineSymbol> symbols)
    {
        var partSize = part.number.Length;
        // check part.x - 1, part.x + 1
        var checkPositions = new List<(int x, int y)>
        {
            (part.x - 1, part.y),
            (part.x + partSize , part.y)
        };

        for (var i = part.x - 1; i <= part.x + partSize; i++)
        {
            checkPositions.Add((i, part.y - 1));
            checkPositions.Add((i, part.y + 1));
        }

        // Console.WriteLine($"cp {part.number} {partSize} {checkPositions.Count()}");
        // foreach (var cp in checkPositions)
        // {
        //     Console.WriteLine($" + cp {cp.x},{cp.y}");
        // }

        var matches = symbols.Where((s) =>
        {
            foreach (var cp in checkPositions)
            {
                if (cp.x == s.x && cp.y == s.y)
                {
                    // Console.WriteLine($"cp-near {part.number} {s.number} {s.x}, {s.y}");
                    return true;
                }
            }
            // Console.WriteLine($"cp-fail {part.number} {s.number} {s.x}, {s.y}");
            return false;
        });

        if (matches.Count() > 0)
        {
            return true;
        }
        return false;
    }

    private (bool result, EnginePart part, List<EngineSymbol> matchedSymbols) isNearDetails(EnginePart part, List<EngineSymbol> symbols)
    {
        var partSize = part.number.Length;
        // check part.x - 1, part.x + 1
        var checkPositions = new List<(int x, int y)>
        {
            (part.x - 1, part.y),
            (part.x + partSize , part.y)
        };

        for (var i = part.x - 1; i <= part.x + partSize; i++)
        {
            checkPositions.Add((i, part.y - 1));
            checkPositions.Add((i, part.y + 1));
        }

        // Console.WriteLine($"cp {part.number} {partSize} {checkPositions.Count()}");
        // foreach (var cp in checkPositions)
        // {
        //     Console.WriteLine($" + cp {cp.x},{cp.y}");
        // }

        var matches = symbols.Where((s) =>
        {
            foreach (var cp in checkPositions)
            {
                if (cp.x == s.x && cp.y == s.y)
                {
                    // Console.WriteLine($"cp-near {part.number} {s.number} {s.x}, {s.y}");
                    return true;
                }
            }
            // Console.WriteLine($"cp-fail {part.number} {s.number} {s.x}, {s.y}");
            return false;
        }).ToList();

        if (matches.Count() > 0)
        {
            return (true, part, matches);
        }
        return (false, part, new List<EngineSymbol>());
    }

}

class EngineGear
{
    public EngineGear()
    {

    }
}