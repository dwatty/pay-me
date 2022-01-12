using System.Collections.Generic;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;
using PayMe.Shared.Enums;
using PayMe.Shared;
using Xunit;

namespace PayMe.Tests;

public class SingleGroupRunTests
{

    #region Runs of Three

    [Fact]
    public void RunOfThree_FullHand_NoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(4, Suites.Clubs),
            new Card(5, Suites.Clubs),
            new Card(6, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void RunOfThree_FullHand_AceHighWithWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(Constants.ACE, Suites.Clubs),
            new Card(3, Suites.Clubs),
            new Card(Constants.QUEEN, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void RunOfThree_FullHand_LowRunWithWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(2, Suites.Clubs),
            new Card(10, Suites.Clubs),
            new Card(4, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Tens);
        Assert.True(result);
    }

    [Fact]
    public void RunOfThree_FullHand_AceLowWithWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(Constants.ACE, Suites.Clubs),
            new Card(2, Suites.Clubs),
            new Card(3, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void RunOfThree_FullHand_AceHighWithQueenWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(Constants.ACE, Suites.Clubs),
            new Card(3, Suites.Clubs),
            new Card(Constants.KING, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void RunOfThree_FullHand_NoWilds_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(4, Suites.Clubs),
            new Card(5, Suites.Clubs),
            new Card(6, Suites.Hearts),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.False(result);
    }

    [Fact]
    public void RunOfThree_FullHand_OneWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Clubs),
            new Card(3, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void RunOfThree_FullHand_OneJoker_Valid()
    {
        var hand = new List<Card>
        {
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Clubs),
            new Card(0, Suites.Jokers),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.True(result);
    }

    [Fact]
    public void RunOfThree_FullHand_OneJoker_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Diamonds),
            new Card(0, Suites.Jokers),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.False(result);
    }

    [Fact]
    public void RunOfThree_FullHand_OneWild_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Hearts),
            new Card(3, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.False(result);
    }
   
    [Fact]
    public void RunOfThree_NotFullHand_OneWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Hearts),
            new Card(7, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Sevens);
        Assert.False(result);
    }

    #endregion

    #region Runs of Four

    [Fact]
    public void RunOfFour_AceHigh_FullHand_Valid()
    {
        var hand = new List<Card>
        {
            new Card(Constants.JACK, Suites.Clubs),
            new Card(Constants.QUEEN, Suites.Clubs),
            new Card(Constants.KING, Suites.Clubs),
            new Card(Constants.ACE, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Fours);
        Assert.True(result);
    }

    [Fact]
    public void RunOfFour_AceHigh_NotFullHand_Valid()
    {
        var hand = new List<Card>
        {
            new Card(Constants.JACK, Suites.Clubs),
            new Card(Constants.QUEEN, Suites.Clubs),
            new Card(Constants.KING, Suites.Clubs),
            new Card(Constants.ACE, Suites.Clubs),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Aces);
        Assert.True(result);
    }

    [Fact]
    public void RunOfFour_AceLow_NotFullHand_NoWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(Constants.ACE, Suites.Hearts),
            new Card(2, Suites.Hearts),
            new Card(3, Suites.Hearts),
            new Card(4, Suites.Hearts),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Sevens);
        Assert.True(result);
    }

    // Physical: 2H, 9D, 5H, 9S
    // Logical: 2H, 3H, 4H, 5G
    [Fact]
    public void RunOfFour_NotFullHand_ConsecutiveWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(2, Suites.Hearts),
            new Card(9, Suites.Diamonds),
            new Card(5, Suites.Hearts),
            new Card(9, Suites.Spades),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Nines);
        Assert.True(result);
    }

    [Fact]
    public void RunOfFour_AceLow_FullHand_OneWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(Constants.ACE, Suites.Hearts),
            new Card(2, Suites.Hearts),
            new Card(3, Suites.Hearts),
            new Card(4, Suites.Hearts),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Fours);
        Assert.True(result);
    }


    [Fact]
    public void RunOfFour_AceLow_NotFullHand_OneWildPlayedAsTwo_Valid()
    {
        var hand = new List<Card>
        {
            new Card(Constants.ACE, Suites.Hearts),
            new Card(10, Suites.Hearts),
            new Card(3, Suites.Hearts),
            new Card(4, Suites.Hearts),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Tens);
        Assert.True(result);
    }

    // Physical: 10H, 10D, 4H, 5H, AH
    // Logical: AH, 2H, 3H, 4H, 5H
    [Fact]
    public void RunOfFour_AceLow_NotFullHand_MultipleWildsOnePlayedAsTwo_Valid()
    {
        var hand = new List<Card>
        {
            new Card(10, Suites.Hearts),
            new Card(10, Suites.Diamonds),
            new Card(4, Suites.Hearts),
            new Card(Constants.ACE, Suites.Hearts),
            new Card(5, Suites.Hearts),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Tens);
        Assert.True(result);
    }

    #endregion

    #region Runs of Five

    [Fact]
    public void RunOfFive_FullHand_TwoWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(12, Suites.Clubs),
            new Card(5, Suites.Diamonds),
            new Card(5, Suites.Clubs),
            new Card(11, Suites.Clubs),
            new Card(10, Suites.Clubs)
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Fives);
        Assert.True(result);
    }

    [Fact]
    public void RunOfFive_FullHand_TwoWild_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(12, Suites.Clubs),
            new Card(5, Suites.Diamonds),
            new Card(5, Suites.Clubs),
            new Card(11, Suites.Spades),
            new Card(10, Suites.Clubs)
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Fives);
        Assert.False(result);
    }

    // Physical: 2H, 9D, 5H, 9S, 9H
    // Logical: 2H, 3H, 4H, 5H, 6H
    [Fact]
    public void RunOfFive_FullHand_ThreeWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(2, Suites.Hearts),
            new Card(9, Suites.Diamonds),
            new Card(5, Suites.Hearts),
            new Card(9, Suites.Spades),
            new Card(9, Suites.Hearts),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Nines);
        Assert.True(result);
    }    

    // Physical: 2H, 9D, 10H, 9S, 9H
    // Logical: N/A
    [Fact]
    public void RunOfFive_FullHand_ThreeWilds_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(2, Suites.Hearts),
            new Card(9, Suites.Diamonds),
            new Card(10, Suites.Hearts),
            new Card(9, Suites.Spades),
            new Card(9, Suites.Hearts),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Nines);
        Assert.False(result);
    }        

    [Fact]
    public void RunOfFive_FulllHand_WrongAmount_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(3, Suites.Diamonds),
            new Card(3, Suites.Clubs),
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Clubs),
            new Card(12, Suites.Clubs)                
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Threes);
        Assert.False(result);
    }

    [Fact]
    public void RunOfFive_NotFulllHand_MultipleWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(10, Suites.Clubs),
            new Card(Constants.JOKER, Suites.Jokers),
            new Card(2, Suites.Diamonds),
            new Card(4, Suites.Diamonds),
            new Card(6, Suites.Diamonds)
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Tens);
        Assert.True(result);
    }    

    #endregion

    #region Runs of Seven

    [Fact]
    public void RunOfSeven_FullHand_OneMatchingWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(5, Suites.Clubs),
            new Card(6, Suites.Clubs),
            new Card(7, Suites.Clubs),
            new Card(8, Suites.Clubs),
            new Card(9, Suites.Clubs),
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Clubs)
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Sevens);
        Assert.True(result);
    }


    [Fact]
    public void RunOfSeven_FullHand_OneNonMatchingWild_Valid()
    {
        var hand = new List<Card>
        {
            new Card(5, Suites.Clubs),
            new Card(6, Suites.Clubs),
            new Card(7, Suites.Diamonds),
            new Card(8, Suites.Clubs),
            new Card(9, Suites.Clubs),
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Clubs)
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Sevens);
        Assert.True(result);
    }


   [Fact]
    public void RunOfSeven_FullHand_MixedMatches_Valid()
    {
        var hand = new List<Card>
        {
            new Card(5, Suites.Clubs),
            new Card(8, Suites.Clubs),
            new Card(9, Suites.Clubs),
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Clubs),
            new Card(7, Suites.Diamonds),
            new Card(0, Suites.Jokers),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Sevens);
        Assert.True(result);
    }

   [Fact]
    public void RunOfSeven_FullHand_MixedMatches_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(5, Suites.Clubs),
            new Card(8, Suites.Diamonds),
            new Card(9, Suites.Clubs),
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Clubs),
            new Card(7, Suites.Diamonds),
            new Card(0, Suites.Jokers),
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Sevens);
        Assert.False(result);
    }

    #endregion

    #region Runs of Thirteen

   [Fact]
    public void RunOfThirtteen_NotFullHand_NoWilds_Valid()
    {
        var hand = new List<Card>
        {
            new Card(2, Suites.Clubs),
            new Card(3, Suites.Clubs),
            new Card(4, Suites.Clubs),
            new Card(5, Suites.Clubs),
            new Card(6, Suites.Clubs),
            new Card(7, Suites.Clubs),
            new Card(8, Suites.Clubs),
            new Card(9, Suites.Clubs),
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Clubs),
            new Card(12, Suites.Clubs),
            new Card(13, Suites.Clubs),
            new Card(14, Suites.Clubs)
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Aces);
        Assert.True(result);
    }

    #endregion

    #region Runs of Ten

   [Fact]
    public void RunOfTen_NotFullHand_GapsInRun_Invalid()
    {
        var hand = new List<Card>
        {
            new Card(2, Suites.Clubs),
            new Card(4, Suites.Clubs),
            new Card(5, Suites.Clubs),
            new Card(6, Suites.Clubs),
            new Card(7, Suites.Clubs),
            new Card(8, Suites.Clubs),
            new Card(9, Suites.Clubs),
            new Card(10, Suites.Clubs),
            new Card(11, Suites.Clubs),
            new Card(12, Suites.Clubs)
        };

        var result = ValidityEngine.AssertRun(hand, GameRound.Aces);
        Assert.False(result);
    }

    #endregion

}