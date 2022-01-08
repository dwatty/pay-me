using Microsoft.AspNetCore.SignalR;
using PayMe.Enums;
using PayMe.Infrastructure;
using PayMe.Models;

namespace PayMe.Hubs;

public interface IGameHub
{
    Task JoinGame(Guid gameId);
    Task GameCreated();
    Task GameStarted(Stack<Card> discard, IDeck availableCards);
    Task NewDiscardAvailable(Card nextDiscard);
    Task CardDiscarded(Card discarded);
    Task EndRound(Dictionary<GameRound, List<RoundResult>> results);
    Task EndTurn(Guid nextPlayerId);
    Task RoundWon(string playerName);
}

    
public class GameHub : Hub<IGameHub>
{
    private readonly ILogger<GameHub> _logger;
    private readonly string NOT_PLAYING = "Not Playing";

    public GameHub(ILogger<GameHub> logger)
    {   
        _logger = logger;
    }
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, NOT_PLAYING);
        await base.OnConnectedAsync();
    }

    public async Task JoinGame(Guid gameId)
    {
        _logger.LogDebug("Joined Game");

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, NOT_PLAYING);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());   
    } 

}
