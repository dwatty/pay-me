using System.Collections.Generic;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Models;
using PayMe.Shared;
using PayMe.Shared.Enums;
using Xunit;
using System.Threading.Tasks;
using System;
using Moq;
using PayMe.Shared.Interfaces;
using PayMe.Grains;
using PayMe.Tests;
using Microsoft.AspNetCore.SignalR;
using PayMe.Server.Hubs;
using Orleans;

namespace PayMe.Testing;

///
///
/// Attempts ot test grain in isolation
/// currently blowing up trying to use the persistance object
/// that's being passed in
///


public class GameGrainTests
{

    // [Fact]
    // public async Task GameGrain_DealsCardsCorrectly()
    // {

    //     var gameId = Guid.NewGuid();
    //     var player1 = Guid.NewGuid();
    //     var player2 = Guid.NewGuid();
        
    //     var mockHub = new Mock<IGameHub>();
        
    //     var mockClients = new Mock<IHubClients<IGameHub>>();
    //     mockClients.Setup(clients => clients.All).Returns(mockHub.Object);

    //     var mockHubContext = new Mock<IHubContext<GameHub, IGameHub>>();
    //     mockHubContext.Setup(x => x.Clients).Returns(() => mockClients.Object);

    //     var mockYar = new Mock<Orleans.Runtime.IPersistentState<PayMe.Shared.Models.GameGrainState>>();
    //     mockYar.Setup(l => l.State).Returns(new GameGrainState());

    //     var mockNotify = new MockClientNotifcation(mockHubContext.Object);
    //     var mockStore = new MockGrainStore();
    //     var mockDeck = new MockDeck();


    //     var summary = new Mock<IPlayerGrain>();

    //     var factor = new Mock<IGrainFactory>(x => (x as IGrainFactory).GetGrain<IPlayerGrain>(Guid.Empty, null) == summary);

    //     // var factory = new Mock<IGrainFactory>(_ => _.GetGrain<IPlayerGrain>(Guid.Empty, null) == summary);







    //     var gameGrain = new GameGrain(mockNotify, mockYar.Object, mockDeck) { Callbase = true; }






    //     // 1. Add player 1
    //     var gameState = await gameGrain.AddPlayerToGame(player1);
    //     Assert.Equal(GameState.AwaitingPlayers, gameState);

    //     // 2. Add player 2
    //     gameState = await gameGrain.AddPlayerToGame(player2);
    //     Assert.Equal(GameState.InPlay, gameState);

    //     // 3.  Check for 3 cards in our hand
    //     var summary = await gameGrain.GetPlayerSummary(player1);
    //     Assert.Equal(3, summary.Hand.Count);

    //     // 4.  Draw a card
    //     var drawnCard = await gameGrain.DrawCard(player1);
    //     Assert.Equal(7, drawnCard.Value);
    //     Assert.Equal(Suites.Hearts, drawnCard.Suite);

    //     // 5. Discard our pulled card
    //     await gameGrain.DiscardCard(player1, drawnCard.Suite, drawnCard.Value);
        
    //     // 6. Claim a win
    //     summary = await gameGrain.GetPlayerSummary(player1);
    //     var hands = new List<List<Card>>()
    //     {
    //         summary.Hand
    //     };

    //     var  claimResult = await gameGrain.ClaimWin(player1, hands);
    //     Assert.Equal(ClaimResult.Valid, claimResult);
        
    // }

}