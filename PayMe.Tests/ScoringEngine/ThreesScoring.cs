using System.Collections.Generic;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;
using PayMe.Shared.Enums;
using PayMe.Shared;
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
                new Card(4, Suites.Hearts),
                new Card(4, Suites.Diamonds),
                new Card(4, Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, GameRound.Threes);

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
                new Card(4, Suites.Hearts),
                new Card(6, Suites.Diamonds),
                new Card(7, Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, GameRound.Threes);

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
                new Card(10, Suites.Hearts),
                new Card(Constants.QUEEN, Suites.Diamonds),
                new Card(Constants.KING, Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, GameRound.Threes);

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
                new Card(10, Suites.Hearts),
                new Card(Constants.QUEEN, Suites.Diamonds),
                new Card(3, Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, GameRound.Threes);

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
                new Card(5, Suites.Hearts),
                new Card(Constants.QUEEN, Suites.Diamonds),
                new Card(Constants.JOKER, Suites.Jokers),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Threes);
        var points = ScoringEngine.ScoreHand(result, GameRound.Threes);

        Assert.False(result.AllSetsValid());
        Assert.Equal(30, points);
    }

}