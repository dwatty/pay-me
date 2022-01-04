using System.Collections.Generic;
using PayMe.Infrastructure;
using PayMe.Models;
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
                new Card(5, Enums.Suites.Diamonds),
                new Card(6, Enums.Suites.Diamonds),
                new Card(7, Enums.Suites.Diamonds),
                new Card(8, Enums.Suites.Diamonds)
            },
            new List<Card>
            {
                new Card(3, Enums.Suites.Hearts),
                new Card(3, Enums.Suites.Diamonds),
                new Card(Constants.JACK, Enums.Suites.Spades)
            },
            new List<Card>
            {
                new Card(Constants.ACE, Enums.Suites.Spades),
                new Card(2, Enums.Suites.Spades),
                new Card(3, Enums.Suites.Spades),
                new Card(4, Enums.Suites.Spades)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Jacks);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Jacks);

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
                new Card(5, Enums.Suites.Diamonds),
                new Card(6, Enums.Suites.Diamonds),
                new Card(7, Enums.Suites.Diamonds),
                new Card(8, Enums.Suites.Diamonds)
            },
            new List<Card>
            {
                new Card(3, Enums.Suites.Hearts),
                new Card(3, Enums.Suites.Diamonds),
                new Card(Constants.QUEEN, Enums.Suites.Clubs),
                new Card(Constants.ACE, Enums.Suites.Spades),
                new Card(2, Enums.Suites.Spades),
                new Card(3, Enums.Suites.Hearts),
                new Card(4, Enums.Suites.Spades)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Jacks);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Jacks);

        Assert.False(result.AllSetsValid());
        Assert.Equal(50, points);
    }

}