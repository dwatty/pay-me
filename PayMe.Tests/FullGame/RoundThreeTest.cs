using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.TestingHost;
using PayMe.Shared.Enums;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;
using Xunit;

namespace PayMe.Tests;

[Collection(ClusterCollection.Name)]
public class E2E_RoundThreeTest
{
    private readonly TestCluster _cluster;

    public E2E_RoundThreeTest(ClusterFixture fixture)
    {
        _cluster = fixture.Cluster;
    }

    [Fact]
    public async Task DealsCardsCorrectly()
    {

        var gameId = Guid.NewGuid();
        var player1 = Guid.NewGuid();
        var player2 = Guid.NewGuid();

        var gameGrain = _cluster.GrainFactory.GetGrain<IGameGrain>(gameId);

        // 1. Add player 1
        var gameState = await gameGrain.AddPlayerToGame(player1);
        Assert.Equal(GameState.AwaitingPlayers, gameState);

        // 2. Add player 2
        gameState = await gameGrain.AddPlayerToGame(player2);
        Assert.Equal(GameState.InPlay, gameState);

        // 3.  Check for 3 cards in our hand
        var summary = await gameGrain.GetPlayerSummary(player1);
        Assert.Equal(3, summary.Hand.Count);

        // 4.  Draw a card
        var drawnCard = await gameGrain.DrawCard(player1);
        Assert.Equal(7, drawnCard.Value);
        Assert.Equal(Suites.Hearts, drawnCard.Suite);

        // 5. Discard our pulled card
        await gameGrain.DiscardCard(player1, drawnCard.Suite, drawnCard.Value);

        // 6. Claim a win
        summary = await gameGrain.GetPlayerSummary(player1);
        var hands = new List<List<Card>>()
        {
            summary.Hand
        };

        var claimResult = await gameGrain.ClaimWin(player1, hands);
        Assert.Equal(ClaimResult.Valid, claimResult);

    }

}