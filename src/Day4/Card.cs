namespace Day4;

class Card
{
    public int cardId;
    private List<int> winningNumbers;
    private List<int> cardNumbers;

    public Card(int cardId, List<int> winningNumbers, List<int> cardNumbers)
    {
        this.cardId = cardId;
        this.winningNumbers = winningNumbers;
        this.cardNumbers = cardNumbers;
    }

    public Card Clone()
    {
        return new Card(cardId, winningNumbers.ToArray().ToList(), cardNumbers.ToArray().ToList());
    }

    public static Card From(string input)
    {
        // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53

        var parts = input.Split(":");
        var cardId = int.Parse(parts[0].Trim().Replace("Card ", ""));
        var cardValues = parts[1];
        var valueParts = cardValues.Split("|");
        var winningNumbers = valueParts[0].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var cardNumbers = valueParts[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        return new Card(cardId, winningNumbers, cardNumbers);
    }

    public int CheckWinnings()
    {
        var winningCardNumbers = this.cardNumbers.Where(cn =>
        {
            return this.winningNumbers.Any(wn => wn == cn);
        });


        var result = Math.Pow(2d, winningCardNumbers.Count() - 1);
        return (int)result;
    }

    public List<int> CheckWinnings2()
    {
        var winningCardNumbers = this.cardNumbers.Where(cn =>
        {
            return this.winningNumbers.Any(wn => wn == cn);
        });

        // var result = Math.Pow(2d, winningCardNumbers.Count() - 1);

        // when cardId = 1 and winningCardNumbers.Count is 4, return 2,3,4,5
        var result = Enumerable.Range(cardId + 1, winningCardNumbers.Count()).ToList();

        return result;
    }
}