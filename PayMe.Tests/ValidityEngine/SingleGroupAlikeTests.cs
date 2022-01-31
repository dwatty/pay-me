using System.Collections.Generic;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;
using PayMe.Shared.Enums;
using Xunit;

namespace PayMe.Tests;

public class SingleGroupAlikeTests
{
    [Fact]
    public void ThreeOfAKind_FullHand_NoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(3, Suites.Clubs),
            new Card(3, Suites.Hearts),
            new Card(3, Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKindV2_FullHand_NoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(2, Suites.Clubs),
            new Card(2, Suites.Hearts),
            new Card(2, Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_NotFullHand_NoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(4, Suites.Clubs),
            new Card(4, Suites.Hearts),
            new Card(4, Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Nines);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_OneJoker_Valid()
    {
        var hand = new List<Card>
        {
            new Card(12, Suites.Clubs),
            new Card(12, Suites.Hearts),
            new Card(0, Suites.Jokers),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_TwoJokers_Valid()
    {
        var hand = new List<Card>
        {
            new Card(12, Suites.Clubs),
            new Card(12, Suites.Hearts),
            new Card(0, Suites.Jokers),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_TwoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(3, Suites.Clubs),
            new Card(4, Suites.Hearts),
            new Card(3, Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_OneWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(6, Suites.Clubs),
            new Card(6, Suites.Hearts),
            new Card(3, Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_OneWild_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(6, Suites.Clubs),
            new Card(10, Suites.Hearts),
            new Card(3, Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.False(result);
    }
    
    [Fact]
    public void ThreeOfAKind_FullHand_NoWilds_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(6, Suites.Clubs),
            new Card(4, Suites.Hearts),
            new Card(3, Suites.Spades),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.False(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_OneJoker_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(6, Suites.Clubs),
            new Card(4, Suites.Hearts),
            new Card(0, Suites.Jokers),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.False(result);
    }

    [Fact]
    public void ThreeOfAKind_FullHand_MixedWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(12, Suites.Clubs),
            new Card(3, Suites.Hearts),
            new Card(0, Suites.Jokers),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Threes);
        Assert.True(result);
    }

    
    [Fact]
    public void FiveOfAKind_FullHand_MixedWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(10, Suites.Clubs),
            new Card(0, Suites.Jokers),
            new Card(0, Suites.Jokers),
            new Card(5, Suites.Hearts),
            new Card(5, Suites.Clubs),
        };

        var result = ValidityEngine.AssertMatchingFaces(hand, GameRound.Fives);
        Assert.True(result);
    }

}