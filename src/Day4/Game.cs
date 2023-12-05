namespace Day4;

class Game
{
    public static void Part2(string[] data)
    {
        var cards = new List<Card>();
        foreach (var line in data)
        {
            var card = Card.From(line);
            cards.Add(card);
        }

        var done = false;
        var rounds = 0;
        var pile = new List<Card>();

        pile.AddRange(cards);
        var pileSize = cards.Count();

        var checkPile = new List<Card>();

        checkPile.AddRange(cards);
        while (!done)
        {
            // Console.WriteLine("Round");
            var newCards = new List<Card>();
            foreach (var card in checkPile)
            {
                var winningCardIds = card.CheckWinnings2();
                // Console.WriteLine($"Winning Ids from {card.cardId}: {string.Join(",", winningCardIds)}");
                var winningCards = cards.Where(c => winningCardIds.Contains(c.cardId));

                if (winningCards.Count() > 0)
                {
                    newCards.AddRange(winningCards);
                }
            }

            if (newCards.Count() > 0)
            {
                // Console.WriteLine($"Adding {newCards.Count()}");
                // pile.AddRange(newCards);
                pileSize += newCards.Count();
                checkPile.Clear();
                checkPile.AddRange(newCards);
            }
            else
            {
                done = true;
            }
            rounds++;
            // if (rounds > 2)
            // {
            //     done = true;
            // }

        }

        Console.WriteLine($"Part2: Pile is now {pileSize}");
    }
}