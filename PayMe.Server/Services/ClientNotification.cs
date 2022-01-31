using Microsoft.AspNetCore.SignalR;
using PayMe.Server.Hubs;
using PayMe.Shared.Enums;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;

namespace PayMe.Server.Services;

public class ClientNotifcation : IClientNotification
{
    private readonly IHubContext<GameHub, IGameHub> _gameHub;

    public ClientNotifcation(IHubContext<GameHub, IGameHub> hub)
    {
        _gameHub = hub;
    }

    public async Task CardDiscarded(Card discarded)
    {
        await _gameHub.Clients.All.CardDiscarded(discarded);
    }

    public async Task EndRound(Dictionary<GameRound, List<RoundResult>> results)
    {
        await _gameHub.Clients.All.EndRound(results);
    }

    public async Task EndTurn(Guid nextPlayerId)
    {
        await _gameHub.Clients.All.EndTurn(nextPlayerId);
    }

    public async Task GameCreated()
    {
        await _gameHub.Clients.All.GameCreated();        
    }

    public async Task GameStarted(Stack<Card> discard, List<Card> availableCards)
    {
        await _gameHub.Clients.All.GameStarted(discard, availableCards);
    }

    public async Task NewDiscardAvailable(Card nextDiscard)
    {
        await _gameHub.Clients.All.NewDiscardAvailable(nextDiscard);
    }

    public async Task RoundWon(string playerName)
    {
        await _gameHub.Clients.All.RoundWon(playerName);
    }
}