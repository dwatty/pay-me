using System.Collections.Generic;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;
using PayMe.Shared;
using PayMe.Shared.Enums;
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
                new Card(3, Suites.Hearts),
                new Card(3, Suites.Diamonds),
                new Card(3, Suites.Spades),
                new Card(4, Suites.Hearts)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Fours);
        var points = ScoringEngine.ScoreHand(result, GameRound.Fours);

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
                new Card(3, Suites.Hearts),
                new Card(10, Suites.Diamonds),
                new Card(7, Suites.Spades),
                new Card(4, Suites.Hearts)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Fours);
        var points = ScoringEngine.ScoreHand(result, GameRound.Fours);

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
                new Card(10, Suites.Hearts),
                new Card(Constants.JACK, Suites.Hearts),
                new Card(Constants.QUEEN, Suites.Hearts),
                new Card(Constants.KING, Suites.Hearts),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Fours);
        var points = ScoringEngine.ScoreHand(result, GameRound.Fours);

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
                new Card(10, Suites.Hearts),
                new Card(10, Suites.Diamonds),
                new Card(10, Suites.Spades)
            },
            new List<Card>
            {
                new Card(Constants.JACK, Suites.Hearts)
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Fours);
        var points = ScoringEngine.ScoreHand(result, GameRound.Fours);

        Assert.False(result.AllSetsValid());
        Assert.Equal(10, points);
    }

}