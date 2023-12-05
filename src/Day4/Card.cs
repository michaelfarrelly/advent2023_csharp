namespace Day4;

class Card
{
    private string cardId;
    private List<int> winningNumbers;
    private List<int> cardNumbers;

    public Card(string cardId, List<int> winningNumbers, List<int> cardNumbers)
    {
        this.cardId = cardId;
        this.winningNumbers = winningNumbers;
        this.cardNumbers = cardNumbers;
    }

    public static Card From(string input)
    {
        // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53

        var parts = input.Split(":");
        var cardId = parts[0].Trim();
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
}