using System.Collections.Generic;
using PayMe.Infrastructure;
using PayMe.Models;
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
                new Card(11, Enums.Suites.Hearts),
                new Card(11, Enums.Suites.Diamonds),
                new Card(11, Enums.Suites.Spades),
            },
            new List<Card>
            {
                new Card(3, Enums.Suites.Clubs),
                new Card(3, Enums.Suites.Hearts),
                new Card(3, Enums.Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Sixes);
        Assert.True(result.AllSetsValid());
    }

    [Fact]
    public void Nines_TwoAlike_NoWilds_Invalid()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(11, Enums.Suites.Hearts),
                new Card(11, Enums.Suites.Diamonds),
                new Card(11, Enums.Suites.Spades),
            },
            new List<Card>
            {
                new Card(3, Enums.Suites.Clubs),
                new Card(3, Enums.Suites.Hearts),
                new Card(3, Enums.Suites.Spades),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Nines);
        Assert.True(result.IsInvalidCollection);
    }

    [Fact]
    public void Nines_TwoRunsOneAlike_NoWilds_Valid()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(11, Enums.Suites.Hearts),
                new Card(11, Enums.Suites.Diamonds),
                new Card(11, Enums.Suites.Spades),
            },
            new List<Card>
            {
                new Card(3, Enums.Suites.Hearts),
                new Card(4, Enums.Suites.Hearts),
                new Card(5, Enums.Suites.Hearts),
            },
            new List<Card>
            {
                new Card(11, Enums.Suites.Hearts),
                new Card(12, Enums.Suites.Hearts),
                new Card(13, Enums.Suites.Hearts),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Nines);
        Assert.True(result.AllSetsValid());
    }    


 [Fact]
    public void Nines_TwoAlikeOneRun_ThreeOfAWild_Valid()
    {
        var hand = new List<List<Card>>
        {
            new List<Card>
            {
                new Card(11, Enums.Suites.Hearts),
                new Card(11, Enums.Suites.Diamonds),
                new Card(11, Enums.Suites.Spades),
            },
            new List<Card>
            {
                new Card(9, Enums.Suites.Hearts),
                new Card(9, Enums.Suites.Diamonds),
                new Card(9, Enums.Suites.Spades),
            },
            new List<Card>
            {
                new Card(2, Enums.Suites.Hearts),
                new Card(3, Enums.Suites.Hearts),
                new Card(4, Enums.Suites.Hearts),
            }
        };

        var result = ValidityEngine.ValidateHand(hand, Enums.GameRound.Nines);
        Assert.True(result.AllSetsValid());
    }    

}