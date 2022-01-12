using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PayMe.Server.Hubs;
using PayMe.Shared.Enums;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;

namespace PayMe.Tests;

public class FakeClientNotification : IClientNotification
{
    private readonly IHubContext<GameHub, IGameHub> _gameHub;

    public FakeClientNotification(IHubContext<GameHub, IGameHub> hub)
    {
        _gameHub = hub;
    }

    public Task CardDiscarded(Card discarded)
    {
        return Task.CompletedTask;
    }

    public Task EndRound(Dictionary<GameRound, List<RoundResult>> results)
    {
        return Task.CompletedTask;
    }

    public Task EndTurn(Guid nextPlayerId)
    {
        return Task.CompletedTask;
    }

    public Task GameCreated()
    {
        return Task.CompletedTask;
    }

    public Task GameStarted(Stack<Card> discard, List<Card> availableCards)
    {
        return Task.CompletedTask;
    }

    public Task NewDiscardAvailable(Card nextDiscard)
    {
        return Task.CompletedTask;
    }

    public Task RoundWon(string playerName)
    {
        return Task.CompletedTask;
    }
}