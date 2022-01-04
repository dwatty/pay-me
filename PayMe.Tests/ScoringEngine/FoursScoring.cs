using System.Collections.Generic;
using PayMe.Infrastructure;
using PayMe.Models;
using Xunit;

namespace PayMe.Tests;

public class FoursScoringTests
{
    [Fact]
    public void FoursScoring_PayMe()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(3, Enums.Suites.Hearts),
                new Card(3, Enums.Suites.Diamonds),
                new Card(3, Enums.Suites.Spades),
                new Card(4, Enums.Suites.Hearts)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Fours);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Fours);

        Assert.True(result.AllSetsValid());
        Assert.Equal(0, points);
    }

    [Fact]
    public void FoursScoring_NoSets()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(3, Enums.Suites.Hearts),
                new Card(10, Enums.Suites.Diamonds),
                new Card(7, Enums.Suites.Spades),
                new Card(4, Enums.Suites.Hearts)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Fours);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Fours);

        Assert.False(result.AllSetsValid());
        Assert.Equal(35, points);
    }

    [Fact]
    public void FoursScoring_RunOfFour_PayMe()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(10, Enums.Suites.Hearts),
                new Card(Constants.JACK, Enums.Suites.Hearts),
                new Card(Constants.QUEEN, Enums.Suites.Hearts),
                new Card(Constants.KING, Enums.Suites.Hearts),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Fours);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Fours);

        Assert.True(result.AllSetsValid());
        Assert.Equal(0, points);
    }

    [Fact]
    public void FoursScoring_OneSetNoPayMe()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(10, Enums.Suites.Hearts),
                new Card(10, Enums.Suites.Diamonds),
                new Card(10, Enums.Suites.Spades)
            },
            new List<Card>
            {
                new Card(Constants.JACK, Enums.Suites.Hearts)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Fours);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Fours);

        Assert.False(result.AllSetsValid());
        Assert.Equal(10, points);
    }

}