using System.Collections.Generic;
using PayMe.Infrastructure;
using PayMe.Models;
using Xunit;

namespace PayMe.Tests;

public class SingleGroupAlikeTests
{
    [Fact]
    public void ThreeOfAKind_FullHand_NoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(3, Enums.Suites.Clubs),
            new Card(3, Enums.Suites.Hearts),
            new Card(3, Enums.Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKindV2_FullHand_NoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(2, Enums.Suites.Clubs),
            new Card(2, Enums.Suites.Hearts),
            new Card(2, Enums.Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_NotFullHand_NoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(4, Enums.Suites.Clubs),
            new Card(4, Enums.Suites.Hearts),
            new Card(4, Enums.Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Nines);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_OneJoker_Valid()
    {
        var hand = new List<Card>
        {
            new Card(12, Enums.Suites.Clubs),
            new Card(12, Enums.Suites.Hearts),
            new Card(0, Enums.Suites.Jokers),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_TwoJokers_Valid()
    {
        var hand = new List<Card>
        {
            new Card(12, Enums.Suites.Clubs),
            new Card(12, Enums.Suites.Hearts),
            new Card(0, Enums.Suites.Jokers),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_TwoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(3, Enums.Suites.Clubs),
            new Card(4, Enums.Suites.Hearts),
            new Card(3, Enums.Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_OneWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(6, Enums.Suites.Clubs),
            new Card(6, Enums.Suites.Hearts),
            new Card(3, Enums.Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_OneWild_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(6, Enums.Suites.Clubs),
            new Card(10, Enums.Suites.Hearts),
            new Card(3, Enums.Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.False(result);
    }
    
    [Fact]
    public void ThreeOfAKind_FullHand_NoWilds_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(6, Enums.Suites.Clubs),
            new Card(4, Enums.Suites.Hearts),
            new Card(3, Enums.Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.False(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_OneJoker_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(6, Enums.Suites.Clubs),
            new Card(4, Enums.Suites.Hearts),
            new Card(0, Enums.Suites.Jokers),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.False(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_MixedWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(12, Enums.Suites.Clubs),
            new Card(3, Enums.Suites.Hearts),
            new Card(0, Enums.Suites.Jokers),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Threes);
        Assert.True(result);
    }

    
    [Fact]
    public void FiveOfAKind_FullHand_MixedWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(10, Enums.Suites.Clubs),
            new Card(0, Enums.Suites.Jokers),
            new Card(0, Enums.Suites.Jokers),
            new Card(5, Enums.Suites.Hearts),
            new Card(5, Enums.Suites.Clubs),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, Enums.GameRound.Fives);
        Assert.True(result);
    }

}