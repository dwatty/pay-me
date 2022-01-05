using System.Collections.Generic;
using System.Linq;
using PayMe.Infrastructure;
using PayMe.Models;

namespace PayMe.Tests;

/**
 * Mock Card Deck for E2E testing.
 * FillDeck is primed with cards in a specific pattern
 */
public class MockDeck : IDeck
{
    private List<Card> _cards = new List<Card>();

    public Card DrawCard()
    {
        var nextCard = _cards.ElementAt(0);
        _cards.RemoveAt(0);
        return nextCard;
    }

    public void FillDeck()
    {
        // P1
        _cards.Add(new Card(2, Enums.Suites.Hearts));
        _cards.Add(new Card(2, Enums.Suites.Clubs));
        _cards.Add(new Card(2, Enums.Suites.Diamonds));

        // P2
        _cards.Add(new Card(3, Enums.Suites.Hearts));
        _cards.Add(new Card(4, Enums.Suites.Hearts));
        _cards.Add(new Card(5, Enums.Suites.Hearts));

        // Discard
        _cards.Add(new Card(6, Enums.Suites.Hearts));

        // First Draw
        _cards.Add(new Card(7, Enums.Suites.Hearts));
        
        _cards.Add(new Card(8, Enums.Suites.Hearts));
        _cards.Add(new Card(9, Enums.Suites.Hearts));
        _cards.Add(new Card(10, Enums.Suites.Hearts));
        _cards.Add(new Card(Constants.JACK, Enums.Suites.Hearts));
        _cards.Add(new Card(Constants.QUEEN, Enums.Suites.Hearts));
        _cards.Add(new Card(Constants.KING, Enums.Suites.Hearts));
        _cards.Add(new Card(Constants.ACE, Enums.Suites.Hearts));
        
        _cards.Add(new Card(3, Enums.Suites.Clubs));
        _cards.Add(new Card(4, Enums.Suites.Clubs));
        _cards.Add(new Card(5, Enums.Suites.Clubs));
        _cards.Add(new Card(6, Enums.Suites.Clubs));
        _cards.Add(new Card(7, Enums.Suites.Clubs));
        _cards.Add(new Card(8, Enums.Suites.Clubs));
        _cards.Add(new Card(9, Enums.Suites.Clubs));
        _cards.Add(new Card(10, Enums.Suites.Clubs));
        _cards.Add(new Card(Constants.JACK, Enums.Suites.Clubs));
        _cards.Add(new Card(Constants.QUEEN, Enums.Suites.Clubs));
        _cards.Add(new Card(Constants.KING, Enums.Suites.Clubs));
        _cards.Add(new Card(Constants.ACE, Enums.Suites.Clubs));
        
        _cards.Add(new Card(3, Enums.Suites.Diamonds));
        _cards.Add(new Card(4, Enums.Suites.Diamonds));
        _cards.Add(new Card(5, Enums.Suites.Diamonds));
        _cards.Add(new Card(6, Enums.Suites.Diamonds));
        _cards.Add(new Card(7, Enums.Suites.Diamonds));
        _cards.Add(new Card(8, Enums.Suites.Diamonds));
        _cards.Add(new Card(9, Enums.Suites.Diamonds));
        _cards.Add(new Card(10, Enums.Suites.Diamonds));
        _cards.Add(new Card(Constants.JACK, Enums.Suites.Diamonds));
        _cards.Add(new Card(Constants.QUEEN, Enums.Suites.Diamonds));
        _cards.Add(new Card(Constants.KING, Enums.Suites.Diamonds));
        _cards.Add(new Card(Constants.ACE, Enums.Suites.Diamonds));
        _cards.Add(new Card(2, Enums.Suites.Spades));
        _cards.Add(new Card(3, Enums.Suites.Spades));
        _cards.Add(new Card(4, Enums.Suites.Spades));
        _cards.Add(new Card(5, Enums.Suites.Spades));
        _cards.Add(new Card(6, Enums.Suites.Spades));
        _cards.Add(new Card(7, Enums.Suites.Spades));
        _cards.Add(new Card(8, Enums.Suites.Spades));
        _cards.Add(new Card(9, Enums.Suites.Spades));
        _cards.Add(new Card(10, Enums.Suites.Spades));
        _cards.Add(new Card(Constants.JACK, Enums.Suites.Spades));
        _cards.Add(new Card(Constants.QUEEN, Enums.Suites.Spades));
        _cards.Add(new Card(Constants.KING, Enums.Suites.Spades));
        _cards.Add(new Card(Constants.ACE, Enums.Suites.Spades));
    }

    public void ShuffleDeck()
    {
        return;
    }
}