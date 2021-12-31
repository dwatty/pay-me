using PayMe.Enums;

namespace PayMe.Models
{

    public class Deck
    {
        public List<Card> Cards = new List<Card>();

        //
        // Initialize our deck of 52 cards plus 2 jokers
        public void FillDeck()
        {
            Cards = new List<Card>();
            
            //Can use a single loop utilising the mod operator % and Math.Floor
            //Using divition based on 13 cards in a suited
            for (int i = 0; i < 52; i++)
            {
                var suite = (Suites)(Math.Floor((decimal)i / 13));
                
                //Add 2 to value as a cards start a 2
                int val = i % 13 + 2;
                Cards.Add(new Card(val, suite));
            }

            Cards.Add(new Card(0, Suites.Jokers));
            Cards.Add(new Card(0, Suites.Jokers));
        }

        //
        // Shuffle our deck of cards
        public void ShuffleDeck()
        {
            var rand = new Random();

            var cardAry = Cards.ToArray();

            for (var n = Cards.Count - 1; n > 0; --n)
            {
                 var k = rand.Next(n+1);

                Card temp = cardAry[n];
                cardAry[n] = cardAry[k];
                cardAry[k] = temp;
            }

            Cards = cardAry.ToList();
        }

        //
        // Draw the next card in the list and return it
        public Card DrawCard()
        {
            var nextCard = Cards.ElementAt(0);
            Cards.RemoveAt(0);
            return nextCard;
        }

    }
}