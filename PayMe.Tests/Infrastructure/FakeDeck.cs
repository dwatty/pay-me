using System.Collections.Generic;
using System.Linq;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;
using PayMe.Shared.Enums;
using PayMe.Shared;

namespace PayMe.Tests;

/**
 * Mock Card Deck for E2E testing.
 * FillDeck is primed with cards in a specific pattern
 */
public class FakeDeck : IDeck
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
        _cards.Add(new Card(2, Suites.Hearts));
        _cards.Add(new Card(2, Suites.Clubs));
        _cards.Add(new Card(2, Suites.Diamonds));

        // P2
        _cards.Add(new Card(3, Suites.Hearts));
        _cards.Add(new Card(4, Suites.Hearts));
        _cards.Add(new Card(5, Suites.Hearts));

        // Discard
        _cards.Add(new Card(6, Suites.Hearts));

        // First Draw
        _cards.Add(new Card(7, Suites.Hearts));
        
        _cards.Add(new Card(8, Suites.Hearts));
        _cards.Add(new Card(9, Suites.Hearts));
        _cards.Add(new Card(10, Suites.Hearts));
        _cards.Add(new Card(Constants.JACK, Suites.Hearts));
        _cards.Add(new Card(Constants.QUEEN, Suites.Hearts));
        _cards.Add(new Card(Constants.KING, Suites.Hearts));
        _cards.Add(new Card(Constants.ACE, Suites.Hearts));
        
        _cards.Add(new Card(3, Suites.Clubs));
        _cards.Add(new Card(4, Suites.Clubs));
        _cards.Add(new Card(5, Suites.Clubs));
        _cards.Add(new Card(6, Suites.Clubs));
        _cards.Add(new Card(7, Suites.Clubs));
        _cards.Add(new Card(8, Suites.Clubs));
        _cards.Add(new Card(9, Suites.Clubs));
        _cards.Add(new Card(10, Suites.Clubs));
        _cards.Add(new Card(Constants.JACK, Suites.Clubs));
        _cards.Add(new Card(Constants.QUEEN, Suites.Clubs));
        _cards.Add(new Card(Constants.KING, Suites.Clubs));
        _cards.Add(new Card(Constants.ACE, Suites.Clubs));
        
        _cards.Add(new Card(3, Suites.Diamonds));
        _cards.Add(new Card(4, Suites.Diamonds));
        _cards.Add(new Card(5, Suites.Diamonds));
        _cards.Add(new Card(6, Suites.Diamonds));
        _cards.Add(new Card(7, Suites.Diamonds));
        _cards.Add(new Card(8, Suites.Diamonds));
        _cards.Add(new Card(9, Suites.Diamonds));
        _cards.Add(new Card(10, Suites.Diamonds));
        _cards.Add(new Card(Constants.JACK, Suites.Diamonds));
        _cards.Add(new Card(Constants.QUEEN, Suites.Diamonds));
        _cards.Add(new Card(Constants.KING, Suites.Diamonds));
        _cards.Add(new Card(Constants.ACE, Suites.Diamonds));
        _cards.Add(new Card(2, Suites.Spades));
        _cards.Add(new Card(3, Suites.Spades));
        _cards.Add(new Card(4, Suites.Spades));
        _cards.Add(new Card(5, Suites.Spades));
        _cards.Add(new Card(6, Suites.Spades));
        _cards.Add(new Card(7, Suites.Spades));
        _cards.Add(new Card(8, Suites.Spades));
        _cards.Add(new Card(9, Suites.Spades));
        _cards.Add(new Card(10, Suites.Spades));
        _cards.Add(new Card(Constants.JACK, Suites.Spades));
        _cards.Add(new Card(Constants.QUEEN, Suites.Spades));
        _cards.Add(new Card(Constants.KING, Suites.Spades));
        _cards.Add(new Card(Constants.ACE, Suites.Spades));
    }

    public List<Card> GetCards()
    {
        return _cards;
    }

    public void ShuffleDeck()
    {
        return;
    }
}