using System.Collections.Generic;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;
using PayMe.Shared.Enums;
using Xunit;

namespace PayMe.Tests;

public class MultiGroupTests
{
    [Fact]
    public void Sixes_TwoAlike_NoWilds_Valid()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(11, Suites.Hearts),
                new Card(11, Suites.Diamonds),
                new Card(11, Suites.Spades),
            },
            new List<Card>
            {
                new Card(3, Suites.Clubs),
                new Card(3, Suites.Hearts),
                new Card(3, Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Sixes);
        Assert.True(result.AllSetsValid());
    }

    [Fact]
    public void Nines_TwoAlike_NoWilds_Invalid()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(11, Suites.Hearts),
                new Card(11, Suites.Diamonds),
                new Card(11, Suites.Spades),
            },
            new List<Card>
            {
                new Card(3, Suites.Clubs),
                new Card(3, Suites.Hearts),
                new Card(3, Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Nines);
        Assert.True(result.IsInvalidCollection);
    }

    [Fact]
    public void Nines_TwoRunsOneAlike_NoWilds_Valid()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(11, Suites.Hearts),
                new Card(11, Suites.Diamonds),
                new Card(11, Suites.Spades),
            },
            new List<Card>
            {
                new Card(3, Suites.Hearts),
                new Card(4, Suites.Hearts),
                new Card(5, Suites.Hearts),
            },
            new List<Card>
            {
                new Card(11, Suites.Hearts),
                new Card(12, Suites.Hearts),
                new Card(13, Suites.Hearts),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Nines);
        Assert.True(result.AllSetsValid());
    }    


 [Fact]
    public void Nines_TwoAlikeOneRun_ThreeOfAWild_Valid()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(11, Suites.Hearts),
                new Card(11, Suites.Diamonds),
                new Card(11, Suites.Spades),
            },
            new List<Card>
            {
                new Card(9, Suites.Hearts),
                new Card(9, Suites.Diamonds),
                new Card(9, Suites.Spades),
            },
            new List<Card>
            {
                new Card(2, Suites.Hearts),
                new Card(3, Suites.Hearts),
                new Card(4, Suites.Hearts),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, GameRound.Nines);
        Assert.True(result.AllSetsValid());
    }    

}