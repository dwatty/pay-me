using System.Collections.Generic;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;
using PayMe.Shared.Enums;
using PayMe.Shared;
using Xunit;

namespace PayMe.Tests;

public class JacksScoringTests
{
    [Fact]
    public void JacksScoring_PayMe()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(5, Suites.Diamonds),
                new Card(6, Suites.Diamonds),
                new Card(7, Suites.Diamonds),
                new Card(8, Suites.Diamonds)
            },
            new List<Card>
            {
                new Card(3, Suites.Hearts),
                new Card(3, Suites.Diamonds),
                new Card(Constants.JACK, Suites.Spades)
            },
            new List<Card>
            {
                new Card(Constants.ACE, Suites.Spades),
                new Card(2, Suites.Spades),
                new Card(3, Suites.Spades),
                new Card(4, Suites.Spades)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Jacks);
        var points = ScoringEngine.ScoreHand(result, GameRound.Jacks);

        Assert.True(result.AllSetsValid());
        Assert.Equal(0, points);
    }


[Fact]
    public void JacksScoring_OneSetNoPayMe()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(5, Suites.Diamonds),
                new Card(6, Suites.Diamonds),
                new Card(7, Suites.Diamonds),
                new Card(8, Suites.Diamonds)
            },
            new List<Card>
            {
                new Card(3, Suites.Hearts),
                new Card(3, Suites.Diamonds),
                new Card(Constants.QUEEN, Suites.Clubs),
                new Card(Constants.ACE, Suites.Spades),
                new Card(2, Suites.Spades),
                new Card(3, Suites.Hearts),
                new Card(4, Suites.Spades)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Jacks);
        var points = ScoringEngine.ScoreHand(result, GameRound.Jacks);

        Assert.False(result.AllSetsValid());
        Assert.Equal(50, points);
    }

}