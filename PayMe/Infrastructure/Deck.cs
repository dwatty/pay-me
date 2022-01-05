using PayMe.Enums;
using PayMe.Models;

namespace PayMe.Infrastructure;

public interface IDeck 
{
    void FillDeck();
    void ShuffleDeck();
    Card DrawCard();
}

public class StandardDeck : IDeck
{
    private List<Card> _cards = new List<Card>();

    //
    // Initialize our deck of 52 cards plus 2 jokers
    public void FillDeck()
    {
        _cards = new List<Card>();
    
        for (int i = 0; i < 52; i++)
        {
            var suite = (Suites)(Math.Floor((decimal)i / 13));
            
            //Add 2 to value as a cards start a 2
            int val = i % 13 + 2;
            _cards.Add(new Card(val, suite));
        }

        _cards.Add(new Card(0, Suites.Jokers));
        _cards.Add(new Card(0, Suites.Jokers));
    }

    //
    // Shuffle our deck of cards
    public void ShuffleDeck()
    {
        var rand = new Random();

        var cardAry = _cards.ToArray();

        for (var n = _cards.Count - 1; n > 0; --n)
        {
                var k = rand.Next(n+1);

            Card temp = cardAry[n];
            cardAry[n] = cardAry[k];
            cardAry[k] = temp;
        }

        _cards = cardAry.ToList();
    }

    //
    // Draw the next card in the list and return it
    public Card DrawCard()
    {
        var nextCard = _cards.ElementAt(0);
        _cards.RemoveAt(0);
        return nextCard;
    }

}
