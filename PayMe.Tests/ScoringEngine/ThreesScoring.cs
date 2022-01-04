using System.Collections.Generic;
using PayMe.Infrastructure;
using PayMe.Models;
using Xunit;

namespace PayMe.Tests;

public class ThreesScoringTests
{
    [Fact]
    public void ThreesScoring_PayMe()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(4, Enums.Suites.Hearts),
                new Card(4, Enums.Suites.Diamonds),
                new Card(4, Enums.Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Threes);

        Assert.True(result.AllSetsValid());
        Assert.Equal(0, points);
    }

    [Fact]
    public void ThreesScoring_NoPayMe_AllLowCard()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(4, Enums.Suites.Hearts),
                new Card(6, Enums.Suites.Diamonds),
                new Card(7, Enums.Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Threes);

        Assert.False(result.AllSetsValid());
        Assert.Equal(15, points);
    }

    [Fact]
    public void ThreesScoring_NoPayMe_AllHighCard()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(10, Enums.Suites.Hearts),
                new Card(Constants.QUEEN, Enums.Suites.Diamonds),
                new Card(Constants.KING, Enums.Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Threes);

        Assert.False(result.AllSetsValid());
        Assert.Equal(30, points);
    }

    [Fact]
    public void ThreesScoring_NoPayMe_OneWild()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(10, Enums.Suites.Hearts),
                new Card(Constants.QUEEN, Enums.Suites.Diamonds),
                new Card(3, Enums.Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Threes);

        Assert.False(result.AllSetsValid());
        Assert.Equal(35, points);
    }

    [Fact]
    public void ThreesScoring_NoPayMe_OneJoker()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(5, Enums.Suites.Hearts),
                new Card(Constants.QUEEN, Enums.Suites.Diamonds),
                new Card(Constants.JOKER, Enums.Suites.Jokers),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, Enums.GameRound.Threes);

        Assert.False(result.AllSetsValid());
        Assert.Equal(30, points);
    }

}